using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Services
{
    public class Debugger
    {
        private readonly EnvDTE.Process m_DebuggedProcess;
        private EnvDTE80.DTE2 m_DTE;


        public EnvDTE80.DTE2 DTE => m_DTE;





        public Debugger(EnvDTE.Process debuggedProcess, EnvDTE80.DTE2 dte)
        {

            this.m_DebuggedProcess = debuggedProcess;
            this.m_DTE = dte;

        }


        public bool HasThreads => m_DebuggedProcess != null && m_DebuggedProcess.Collection.Count > 0;


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
