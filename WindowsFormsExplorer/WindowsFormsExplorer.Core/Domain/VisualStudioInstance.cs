namespace WindowsFormsExplorer.Core.Domain
{
    /// <summary>
    /// Rappresenta un'istanza di Visual Studio in esecuzione
    /// </summary>
    public class VisualStudioInstance
    {
        public int Index { get; set; }
        public string SolutionName { get; set; }
        public string SolutionFullPath { get; set; }
        public bool HasOpenSolution { get; set; }
        public object NativeInstance { get; set; } // Oggetto DTE2 nativo

        public VisualStudioInstance()
        {
        }

        public VisualStudioInstance(int index, string solutionName, string solutionFullPath, bool hasOpenSolution, object nativeInstance)
        {
            Index = index;
            SolutionName = solutionName;
            SolutionFullPath = solutionFullPath;
            HasOpenSolution = hasOpenSolution;
            NativeInstance = nativeInstance;
        }

        public override string ToString()
        {
            return HasOpenSolution ? SolutionName : "No solution open";
        }
    }
}

