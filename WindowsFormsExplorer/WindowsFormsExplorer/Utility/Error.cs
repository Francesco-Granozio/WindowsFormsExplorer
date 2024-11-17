using System;
using System.Linq;

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

    public static class ErrorExtensions
    {
        public static bool IsIn(this Error error, params ErrorCode[] errorCodes)
        {
            return errorCodes.Contains(error.Code);
        }

        public static bool Is(this Error error, ErrorCode errorCode)
        {
            return error.Code == errorCode;
        }
    }
}