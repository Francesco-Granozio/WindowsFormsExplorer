using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsExplorer.Utility;

namespace WindowsFormsExplorer.Services
{
    public class ProcessInspector
    {
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable pprot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);


        public Result<List<EnvDTE80.DTE2>> GetVisualStudioInstances()
        {
            try
            {
                // Ottenere tutte le istanze di Visual Studio attive
                var visualStudioInstancesResult = GetRunningVisualStudioInstances();

                if (visualStudioInstancesResult.IsFailure)
                {
                    return visualStudioInstancesResult.Error;
                }

                List<EnvDTE80.DTE2> visualStudioInstances = visualStudioInstancesResult.Value;

                if (visualStudioInstances.Count == 0)
                {
                    return new Error(ErrorCode.NoVSIstanceFound, "No Visual Studio instance found.");
                }

                return visualStudioInstances;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.DebuggerConnectionError, ex.Message, ex);
            }
        }


        /// <summary>
        /// Questo metodo restituisce una lista di tutte le istanze di Visual Studio attualmente in esecuzione
        /// utilizzando la Running Object Table (ROT) e la COM API per interagire con i processi attivi.
        /// L'idea è di trovare tutte le istanze di Visual Studio in esecuzione e restituirle come una lista.
        /// </summary>
        private Result<List<EnvDTE80.DTE2>> GetRunningVisualStudioInstances()
        {
            try
            {
                List<EnvDTE80.DTE2> instances = new List<EnvDTE80.DTE2>();

                // Ottiene la Running Object Table (ROT), che è un oggetto COM che permette di ottenere informazioni
                // sui processi in esecuzione registrati.
                GetRunningObjectTable(0, out IRunningObjectTable rot);
                rot.EnumRunning(out IEnumMoniker enumMoniker);  // Ottiene un enumeratore per gli oggetti in esecuzione
                enumMoniker.Reset();  // Reset dell'enumeratore per partire dall'inizio

                // Crea un array di moniker per raccogliere informazioni sugli oggetti
                // Adifferenza di un semplice puntatore o riferimento a un oggetto in memoria,
                // un moniker non si limita a fare riferimento alla memoria di un processo in corso,
                // ma può essere utilizzato per localizzare l'oggetto anche se si trova in un altro contesto,
                // come un altro processo, una rete o un file.
                IMoniker[] monikers = new IMoniker[1];
                IntPtr fetched = IntPtr.Zero;

                // Enumerazione degli oggetti nella ROT
                while (enumMoniker.Next(1, monikers, fetched) == 0)
                {
                    // Crea un contesto di binding per ogni moniker
                    CreateBindCtx(0, out IBindCtx bindCtx);
                    monikers[0].GetDisplayName(bindCtx, null, out string displayName);  // Ottiene il nome visualizzabile del moniker

                    // Verifica se il nome del moniker corrisponde a un'istanza di Visual Studio
                    if (!displayName.StartsWith("!VisualStudio.DTE"))
                    {
                        continue;
                    }

                    // Recupera l'oggetto associato al moniker
                    rot.GetObject(monikers[0], out object obj);
                    if (obj is EnvDTE80.DTE2 dte)  // Se l'oggetto è un'istanza di Visual Studio (DTE2), lo aggiunge alla lista
                    {
                        instances.Add(dte);
                    }
                }

                return instances;
            }
            catch (Exception ex)
            {
                return new Error(ErrorCode.DebuggerConnectionError, ex.Message, ex);
            }

        }
    }
}
