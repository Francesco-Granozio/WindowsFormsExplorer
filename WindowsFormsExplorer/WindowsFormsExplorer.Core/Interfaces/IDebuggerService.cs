using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsFormsExplorer.Core.Common;
using WindowsFormsExplorer.Core.Domain;

namespace WindowsFormsExplorer.Core.Interfaces
{
    /// <summary>
    /// Servizio per interagire con il debugger e ottenere informazioni sui controlli
    /// </summary>
    public interface IDebuggerService
    {
        /// <summary>
        /// Verifica se è possibile interrogare il debugger
        /// </summary>
        Result<int> CanQuery();

        /// <summary>
        /// Ottiene le informazioni sulle form aperte
        /// </summary>
        Task<Result<List<ControlInfo>>> GetOpenFormsAsync();

        /// <summary>
        /// Esplora ricorsivamente i controlli di una form
        /// </summary>
        Task<Result<ControlInfo>> ExploreControlsAsync(string baseExpression, int depth = 0, int maxDepth = 100);

        /// <summary>
        /// Ottiene il valore di un'espressione nel debugger
        /// </summary>
        Result<string> EvaluateExpression(string expression);

        /// <summary>
        /// Valuta multiple espressioni in batch (ottimizzazione)
        /// </summary>
        Task<Result<Dictionary<string, string>>> EvaluateExpressionsAsync(IEnumerable<string> expressions);

        /// <summary>
        /// Rilascia le risorse del debugger
        /// </summary>
        void Dispose();
    }
}

