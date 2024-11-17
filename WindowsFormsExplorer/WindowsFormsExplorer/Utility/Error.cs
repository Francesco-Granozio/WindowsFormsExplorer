using System;

namespace WindowsFormsExplorer.Utility
{
    public enum ErrorCode
    {
        None,
        Exception,
        NoVSIstanceFound,
        NoInstanceSelected,
        NoDebugProcessFound,
        InvaliidPID,
        ProcessMustBeInDebugMode,
        DebuggerConnectionError
    }


    public class Error
    {
        public Error(ErrorCode code, string message, Exception exception)
        {
            Code = code;
            Message = message;
            Exception = exception;
        }

        public Error(ErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public ErrorCode Code { get; }
        
        public string Message { get; }
        
        public Exception Exception { get; }
        
        public bool HasException => Exception != null;

        public static Error None => new Error(ErrorCode.None, string.Empty);


    }

}