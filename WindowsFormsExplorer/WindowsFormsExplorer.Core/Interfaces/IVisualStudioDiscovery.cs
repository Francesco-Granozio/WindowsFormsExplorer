using System.Collections.Generic;
using WindowsFormsExplorer.Core.Common;
using WindowsFormsExplorer.Core.Domain;

namespace WindowsFormsExplorer.Core.Interfaces
{
    /// <summary>
    /// Servizio per scoprire istanze di Visual Studio in esecuzione
    /// </summary>
    public interface IVisualStudioDiscovery
    {
        /// <summary>
        /// Ottiene tutte le istanze di Visual Studio in esecuzione
        /// </summary>
        Result<List<VisualStudioInstance>> GetRunningInstances();

        /// <summary>
        /// Ottiene i processi in debug per un'istanza specifica
        /// </summary>
        Result<List<DebugProcess>> GetDebugProcesses(VisualStudioInstance instance);
    }
}

