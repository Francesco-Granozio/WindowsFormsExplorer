using System;
using System.Runtime.InteropServices;

namespace WindowsFormsExplorer.Infrastructure.COM
{
    /// <summary>
    /// Gestisce i messaggi COM per evitare errori RPC_E_CALL_REJECTED
    /// quando si interagisce con EnvDTE
    /// </summary>
    public class MessageFilter : IOleMessageFilter
    {
        public static void Register()
        {
            IOleMessageFilter newFilter = new MessageFilter();
            CoRegisterMessageFilter(newFilter, out IOleMessageFilter oldFilter);
        }

        public static void Revoke()
        {
            CoRegisterMessageFilter(null, out IOleMessageFilter oldFilter);
        }

        int IOleMessageFilter.HandleInComingCall(int dwCallType,
          IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
        {
            return 0; // SERVERCALL_ISHANDLED
        }

        int IOleMessageFilter.RetryRejectedCall(IntPtr hTaskCallee,
          int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == 2) // SERVERCALL_RETRYLATER
            {
                return 99; // Retry immediately
            }
            return -1; // Cancel call
        }

        int IOleMessageFilter.MessagePending(IntPtr hTaskCallee,
          int dwTickCount, int dwPendingType)
        {
            return 2; // PENDINGMSG_WAITDEFPROCESS
        }

        [DllImport("Ole32.dll")]
        private static extern int CoRegisterMessageFilter(
            IOleMessageFilter newFilter,
            out IOleMessageFilter oldFilter);
    }

    [ComImport(), Guid("00000016-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOleMessageFilter
    {
        [PreserveSig]
        int HandleInComingCall(int dwCallType, IntPtr hTaskCaller,
            int dwTickCount, IntPtr lpInterfaceInfo);

        [PreserveSig]
        int RetryRejectedCall(IntPtr hTaskCallee,
            int dwTickCount, int dwRejectType);

        [PreserveSig]
        int MessagePending(IntPtr hTaskCallee,
            int dwTickCount, int dwPendingType);
    }
}

