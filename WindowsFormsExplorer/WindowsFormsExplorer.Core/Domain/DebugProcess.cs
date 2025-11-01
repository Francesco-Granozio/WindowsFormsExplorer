namespace WindowsFormsExplorer.Core.Domain
{
    /// <summary>
    /// Rappresenta un processo in debug
    /// </summary>
    public class DebugProcess
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public object NativeProcess { get; set; } // Oggetto Process2 nativo

        public DebugProcess()
        {
        }

        public DebugProcess(int processId, string processName, object nativeProcess)
        {
            ProcessId = processId;
            ProcessName = processName;
            NativeProcess = nativeProcess;
        }

        public override string ToString()
        {
            return $"{ProcessName} (PID: {ProcessId})";
        }
    }
}

