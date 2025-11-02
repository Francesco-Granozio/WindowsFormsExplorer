using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using WindowsFormsExplorer.Core.Common;
using WindowsFormsExplorer.Core.Domain;
using WindowsFormsExplorer.Core.Interfaces;

namespace WindowsFormsExplorer.Infrastructure.Discovery
{
    /// <summary>
    /// Servizio per scoprire istanze di Visual Studio in esecuzione
    /// usando la Running Object Table (ROT)
    /// </summary>
    public class VisualStudioDiscoveryService : IVisualStudioDiscovery
    {
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable pprot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

        public Result<List<VisualStudioInstance>> GetRunningInstances()
        {
            try
            {
                List<VisualStudioInstance> instances = new List<VisualStudioInstance>();

                // Ottiene la Running Object Table (ROT)
                GetRunningObjectTable(0, out IRunningObjectTable rot);
                rot.EnumRunning(out IEnumMoniker enumMoniker);
                enumMoniker.Reset();

                IMoniker[] monikers = new IMoniker[1];
                IntPtr fetched = IntPtr.Zero;
                int index = 0;

                // Enumera gli oggetti nella ROT
                while (enumMoniker.Next(1, monikers, fetched) == 0)
                {
                    CreateBindCtx(0, out IBindCtx bindCtx);
                    monikers[0].GetDisplayName(bindCtx, null, out string displayName);

                    // Verifica se è un'istanza di Visual Studio
                    if (!displayName.StartsWith("!VisualStudio.DTE"))
                    {
                        continue;
                    }

                    rot.GetObject(monikers[0], out object obj);
                    if (obj is EnvDTE80.DTE2 dte)
                    {
                        string solutionName = "No solution open";
                        string solutionFullPath = null;
                        bool hasOpenSolution = false;

                        try
                        {
                            if (dte.Solution != null && !string.IsNullOrEmpty(dte.Solution.FullName))
                            {
                                solutionFullPath = dte.Solution.FullName;
                                solutionName = System.IO.Path.GetFileNameWithoutExtension(solutionFullPath);
                                hasOpenSolution = true;
                            }
                        }
                        catch
                        {
                            // Ignora errori nell'ottenere le info della solution
                        }

                        instances.Add(new VisualStudioInstance(
                            index++,
                            solutionName,
                            solutionFullPath,
                            hasOpenSolution,
                            dte
                        ));
                    }
                }

                if (instances.Count == 0)
                {
                    return new Error(ErrorCode.NoVSInstanceFound,
                        "No Visual Studio instance found.");
                }

                return instances;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.DebuggerConnectionError, ex.Message, ex);
            }
        }

        public Result<List<DebugProcess>> GetDebugProcesses(VisualStudioInstance instance)
        {
            try
            {
                if (instance == null || instance.NativeInstance == null)
                {
                    return new Error(ErrorCode.NoInstanceSelected,
                        "No Visual Studio instance selected.");
                }

                EnvDTE80.DTE2 dte = (EnvDTE80.DTE2)instance.NativeInstance;
                List<DebugProcess> processes = new List<DebugProcess>();

                foreach (EnvDTE80.Process2 process in dte.Debugger.DebuggedProcesses)
                {
                    if (process != null)
                    {
                        processes.Add(new DebugProcess(
                            process.ProcessID,
                            process.Name,
                            process
                        ));
                    }
                }

                if (processes.Count == 0)
                {
                    return new Error(ErrorCode.NoDebugProcessFound,
                        "No debug processes found in the selected instance.");
                }

                return processes;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.DebuggerConnectionError, ex.Message, ex);
            }
        }
    }
}

