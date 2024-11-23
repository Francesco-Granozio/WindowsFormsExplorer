using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Services
{
    public class DebuggerAPI
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct VisualStudioInstance
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
            public bool IsOpen;
        }

        [DllImport("DebuggerAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRunningVisualStudioInstances(out long len, out IntPtr data);

        [DllImport("DebuggerAPI.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeVisualStudioInstances(IntPtr data);

        public static VisualStudioInstance[] GetInstances()
        {
            IntPtr data = IntPtr.Zero;

            try
            {
                int result = GetRunningVisualStudioInstances(out long len, out data);
                if (result != 0 || len <= 0 || data == IntPtr.Zero)
                {
                    return Array.Empty<VisualStudioInstance>();
                }

                VisualStudioInstance[] instances = new VisualStudioInstance[len];
                int structSize = Marshal.SizeOf<VisualStudioInstance>();

                for (long i = 0; i < len; i++)
                {
                    IntPtr elementPtr = IntPtr.Add(data, (int)(i * structSize));
                    instances[i] = Marshal.PtrToStructure<VisualStudioInstance>(elementPtr);
                }

                return instances;
            }
            finally
            {
                if (data != IntPtr.Zero)
                {
                    FreeVisualStudioInstances(data);
                }
            }
        }
    }

}
