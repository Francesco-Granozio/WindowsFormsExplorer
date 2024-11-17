using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsExplorer.Domain;
using WindowsFormsExplorer.Utility;

namespace WindowsFormsExplorer.Services
{
    public class Debugger
    {
        private readonly EnvDTE.Process m_DebuggedProcess;
        private EnvDTE80.DTE2 m_DTE;


        public EnvDTE80.DTE2 DTE => m_DTE;
        public bool HasThreads => m_DebuggedProcess != null && m_DebuggedProcess.Collection.Count > 0;


        public Debugger(EnvDTE.Process debuggedProcess, EnvDTE80.DTE2 dte)
        {

            this.m_DebuggedProcess = debuggedProcess;
            this.m_DTE = dte;

        }


        public Result<bool> CanQuery(ref int formCount)
        {
            try
            {
                //Controllo se ci sono thread dispoibili
                if (!HasThreads)
                {
                    return new Error(ErrorCode.NoThreadsAvailableInProcess, "No threads available in the process!");
                }

                //Il debugger deve essere in pausa
                if (m_DTE.Debugger.CurrentMode != EnvDTE.dbgDebugMode.dbgBreakMode)
                {
                    return new Error(ErrorCode.DebuggerMustBePaused, "The debugger MUST be in pause (break) mode!");
                }

                string formCountStr = GetExpressionValue("System.Windows.Forms.Application.OpenForms.Count");

                if (string.IsNullOrEmpty(formCountStr))
                {
                    return new Error(ErrorCode.UnableToGetFormCount, "Unable to get form count!");
                }

                if (!int.TryParse(formCountStr, out int formCountTmp))
                {
                    return new Error(ErrorCode.InvalidFormCountValue, $"Invalid count value: {formCountStr}");
                }
                formCount = formCountTmp;

            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.Exception, ex.Message, ex);
            }

            return true;
        }


        public IEnumerable<FormInfo> GetFormInfo(int formCount)
        {
            for (int i = 0; i < formCount; i++)
            {
                string baseExpr = $"System.Windows.Forms.Application.OpenForms[{i}]";

                EnvDTE.Expression formExpr = m_DTE.Debugger.GetExpression(baseExpr);
                if (formExpr == null)
                {
                    continue;
                }

                string text = GetExpressionValue($"{baseExpr}.Text") ?? "N/A";
                string type = formExpr.Type ?? "N/A";
                string visible = GetExpressionValue($"{baseExpr}.Visible") ?? "N/A";
                string handle = GetExpressionValue($"{baseExpr}.Handle.ToInt32()") ?? "N/A";
                string name = GetExpressionValue($"{baseExpr}.Name") ?? $"Form_{i}";

                yield return new FormInfo(name, type, text, visible, handle);
            }
        }



        public string GetExpressionValue(string expression)
        {
            int retryCount = 3;
            int delayBetweenRetries = 100; // Millisecondi

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    EnvDTE.Expression expr = m_DTE.Debugger.GetExpression(expression, false, 30000);
                    return expr?.Value;
                }
                catch (COMException ex) when ((uint)ex.HResult == 0x80010001) // RPC_E_CALL_REJECTED
                {
                    if (i == retryCount - 1)
                    {
                        Console.WriteLine($"Exception during expression fetching: {ex.Message}");
                        throw; // Rilancia l'eccezione se siamo all'ultimo tentativo
                    }

                    // Aspetta prima del prossimo tentativo
                    System.Threading.Thread.Sleep(delayBetweenRetries);
                }
            }
            return null;
        }


        public void ReleaseDTE()
        {
            if (m_DTE != null)
            {
                Marshal.ReleaseComObject(m_DTE);
                m_DTE = null;
            }
        }



    }
}
