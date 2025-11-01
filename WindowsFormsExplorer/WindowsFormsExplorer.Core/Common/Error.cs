using System;
using System.Linq;

namespace WindowsFormsExplorer.Core.Common
{
    public enum ErrorCode
    {
        None,
        Exception,
        NoVSInstanceFound,
        NoInstanceSelected,
        NoDebugProcessFound,
        InvalidPID,
        ProcessMustBeInDebugMode,
        DebuggerConnectionError,
        NoThreadsAvailableInProcess,
        DebuggerMustBePaused,
        UnableToGetFormCount,
        InvalidFormCountValue,
        ControlQueryFailed,
        CacheExpired
    }

    public class Error
    {
        public Error(ErrorCode code, string message, Exception exception = null)
        {
            Code = code;
            Message = message;
            Exception = exception;
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

