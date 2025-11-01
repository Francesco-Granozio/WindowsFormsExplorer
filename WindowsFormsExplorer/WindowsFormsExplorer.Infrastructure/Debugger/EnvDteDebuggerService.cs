using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WindowsFormsExplorer.Core.Common;
using WindowsFormsExplorer.Core.Domain;
using WindowsFormsExplorer.Core.Interfaces;

namespace WindowsFormsExplorer.Infrastructure.Debugger
{
    /// <summary>
    /// Implementazione ottimizzata del servizio di debugging usando EnvDTE
    /// Con caching e batch queries per migliorare le performance
    /// </summary>
    public class EnvDteDebuggerService : IDebuggerService
    {
        private readonly EnvDTE.Process _debuggedProcess;
        private readonly EnvDTE80.DTE2 _dte;
        private readonly ExpressionCache _cache;
        private bool _disposed = false;

        public EnvDteDebuggerService(object debuggedProcess, object dte)
        {
            _debuggedProcess = (EnvDTE.Process)debuggedProcess;
            _dte = (EnvDTE80.DTE2)dte;
            _cache = new ExpressionCache(TimeSpan.FromSeconds(30));
        }

        public Result<int> CanQuery()
        {
            try
            {
                // Verifica threads disponibili
                if (_debuggedProcess == null || _debuggedProcess.Collection.Count == 0)
                {
                    return new Error(ErrorCode.NoThreadsAvailableInProcess,
                        "No threads available in the process!");
                }

                // Verifica che il debugger sia in pausa
                if (_dte.Debugger.CurrentMode != EnvDTE.dbgDebugMode.dbgBreakMode)
                {
                    return new Error(ErrorCode.DebuggerMustBePaused,
                        "The debugger MUST be in pause (break) mode!");
                }

                // Ottiene il numero di form
                string formCountStr = EvaluateExpressionInternal(
                    "System.Windows.Forms.Application.OpenForms.Count");

                if (string.IsNullOrEmpty(formCountStr))
                {
                    return new Error(ErrorCode.UnableToGetFormCount,
                        "Unable to get form count!");
                }

                if (!int.TryParse(formCountStr, out int formCount))
                {
                    return new Error(ErrorCode.InvalidFormCountValue,
                        $"Invalid count value: {formCountStr}");
                }

                return formCount;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.Exception, ex.Message, ex);
            }
        }

        public async Task<Result<List<ControlInfo>>> GetOpenFormsAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Result<int> countResult = CanQuery();
                    if (countResult.IsFailure)
                        return Result<List<ControlInfo>>.Failure(countResult.Error);

                    int formCount = countResult.Value;
                    List<ControlInfo> forms = new List<ControlInfo>();

                    for (int i = 0; i < formCount; i++)
                    {
                        // OTTIMIZZAZIONE: Batch query per ridurre le chiamate a EnvDTE
                        string baseExpr = $"System.Windows.Forms.Application.OpenForms[{i}]";
                        
                        var expressions = new[]
                        {
                            $"{baseExpr}.Name",
                            $"{baseExpr}.Text",
                            $"{baseExpr}.Visible",
                            $"{baseExpr}.Handle.ToInt32()",
                            $"{baseExpr}.GetType().FullName"
                        };

                        var results = EvaluateBatchExpressionsInternal(expressions);

                        string name = GetValueOrDefault(results, $"{baseExpr}.Name", $"Form_{i}");
                        string text = GetValueOrDefault(results, $"{baseExpr}.Text", "N/A");
                        string visibleStr = GetValueOrDefault(results, $"{baseExpr}.Visible", "false");
                        bool visible = visibleStr == "true";
                        string handle = GetValueOrDefault(results, $"{baseExpr}.Handle.ToInt32()", "N/A");
                        string type = GetValueOrDefault(results, $"{baseExpr}.GetType().FullName", "N/A");

                        forms.Add(new ControlInfo(baseExpr, name, type, text, visible, handle));
                    }

                    return forms;
                }
                catch (Exception ex)
                {
                    return new Error(ErrorCode.Exception, ex.Message, ex);
                }
            });
        }

        public async Task<Result<ControlInfo>> ExploreControlsAsync(string baseExpression, int depth = 0, int maxDepth = 100)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (depth >= maxDepth)
                        return Result<ControlInfo>.Failure(new Error(ErrorCode.ControlQueryFailed, "Max depth reached"));

                    var stopwatch = Stopwatch.StartNew();

                    // Ottiene il numero di controlli figli
                    string controlsExpr = $"{baseExpression}.Controls";
                    string controlsValue = EvaluateExpressionInternal(controlsExpr);

                    if (controlsValue == null)
                    {
                        return Result<ControlInfo>.Success(new ControlInfo { Expression = baseExpression, Children = new List<ControlInfo>() });
                    }

                    string countExpr = $"{baseExpression}.Controls.Count";
                    string countStr = EvaluateExpressionInternal(countExpr);

                    if (!int.TryParse(countStr, out int controlCount))
                    {
                        return Result<ControlInfo>.Success(new ControlInfo { Expression = baseExpression, Children = new List<ControlInfo>() });
                    }

                    ControlInfo root = new ControlInfo { Expression = baseExpression };

                    Debug.WriteLine($"Exploring {controlCount} controls at depth {depth} for: {baseExpression}");

                    for (int i = 0; i < controlCount; i++)
                    {
                        string childExpr = $"{baseExpression}.Controls[{i}]";

                        // OTTIMIZZAZIONE: Batch query per ogni controllo
                        var expressions = new[]
                        {
                            $"{childExpr}.Name",
                            $"{childExpr}.GetType().Name",
                            $"{childExpr}.Text",
                            $"{childExpr}.Visible"
                        };

                        var results = EvaluateBatchExpressionsInternal(expressions);

                        string name = GetValueOrDefault(results, $"{childExpr}.Name", $"Control_{i}");
                        string type = GetValueOrDefault(results, $"{childExpr}.GetType().Name", "Unknown");
                        string text = GetValueOrDefault(results, $"{childExpr}.Text", "");
                        string visibleStr = GetValueOrDefault(results, $"{childExpr}.Visible", "true");
                        bool visible = visibleStr == "true";

                        ControlInfo child = new ControlInfo(childExpr, name, type, text, visible, "");

                        // Ricorsione per i controlli figli
                        var childResult = ExploreControlsAsync(childExpr, depth + 1, maxDepth).Result;
                        if (childResult.IsSuccess && childResult.Value.Children != null)
                        {
                            child.Children = childResult.Value.Children;
                        }

                        root.AddChild(child);
                    }

                    stopwatch.Stop();
                    Debug.WriteLine($"Explored {controlCount} controls in {stopwatch.ElapsedMilliseconds}ms at depth {depth}");

                    return Result<ControlInfo>.Success(root);
                }
                catch (Exception ex)
                {
                    return Result<ControlInfo>.Failure(new Error(ErrorCode.ControlQueryFailed, ex.Message, ex));
                }
            });
        }

        public Result<string> EvaluateExpression(string expression)
        {
            try
            {
                string value = EvaluateExpressionInternal(expression);
                return value != null ? Result<string>.Success(value) : 
                    Result<string>.Failure(new Error(ErrorCode.ControlQueryFailed, "Failed to evaluate expression"));
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.Exception, ex.Message, ex);
            }
        }

        public async Task<Result<Dictionary<string, string>>> EvaluateExpressionsAsync(IEnumerable<string> expressions)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var results = EvaluateBatchExpressionsInternal(expressions);
                    return Result<Dictionary<string, string>>.Success(results);
                }
                catch (Exception ex)
                {
                    return Result<Dictionary<string, string>>.Failure(
                        new Error(ErrorCode.Exception, ex.Message, ex));
                }
            });
        }

        /// <summary>
        /// OTTIMIZZAZIONE: Valuta espressioni in batch per ridurre overhead
        /// </summary>
        private Dictionary<string, string> EvaluateBatchExpressionsInternal(IEnumerable<string> expressions)
        {
            var results = new Dictionary<string, string>();

            foreach (var expr in expressions)
            {
                string value = EvaluateExpressionInternal(expr);
                results[expr] = value;
            }

            return results;
        }

        /// <summary>
        /// Helper method per compatibilità con .NET Framework 4.7.2
        /// Sostituisce Dictionary.GetValueOrDefault che non esiste in questa versione
        /// </summary>
        private string GetValueOrDefault(Dictionary<string, string> dictionary, string key, string defaultValue)
        {
            if (dictionary.TryGetValue(key, out string value))
            {
                return value ?? defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// Valuta un'espressione con retry logic e caching
        /// </summary>
        private string EvaluateExpressionInternal(string expression)
        {
            // Controlla prima nella cache
            if (_cache.TryGet(expression, out string cachedValue))
            {
                return cachedValue;
            }

            int retryCount = 3;
            int delayBetweenRetries = 50; // Ridotto a 50ms per essere più veloce

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    EnvDTE.Expression expr = _dte.Debugger.GetExpression(expression, false, 15000);
                    string value = expr?.Value;

                    // Memorizza nella cache
                    if (value != null)
                    {
                        _cache.Set(expression, value);
                    }

                    return value;
                }
                catch (COMException ex) when ((uint)ex.HResult == 0x80010001) // RPC_E_CALL_REJECTED
                {
                    if (i == retryCount - 1)
                    {
                        Debug.WriteLine($"Failed to evaluate expression after {retryCount} attempts: {expression}");
                        return null;
                    }

                    System.Threading.Thread.Sleep(delayBetweenRetries);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception evaluating expression '{expression}': {ex.Message}");
                    return null;
                }
            }

            return null;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cache?.Clear();
                
                if (_dte != null)
                {
                    try
                    {
                        Marshal.ReleaseComObject(_dte);
                    }
                    catch { }
                }

                _disposed = true;
            }
        }
    }
}

