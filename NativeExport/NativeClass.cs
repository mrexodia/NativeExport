using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeExport
{
    public class NativeClass
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        private static IntPtr ToNativeString(string managedStr)
        {
            var ansi = Encoding.Default.GetBytes(managedStr);
            const uint GMEM_FIXED = 0;
            const uint GMEM_ZEROINIT = 0x40;
            var data = GlobalAlloc(GMEM_FIXED | GMEM_ZEROINIT, new UIntPtr((uint)ansi.Length + 1));
            Marshal.Copy(ansi, 0, data, ansi.Length);
            return data;
        }

        private static string FromNativeString(IntPtr nativeStr, int size)
        {
            var ansi = new byte[size];
            Marshal.Copy(nativeStr, ansi, 0, size);
            return Encoding.Default.GetString(ansi);
        }

        [DllExport("Magic", CallingConvention.Cdecl)]
        public static IntPtr Magic(IntPtr nativeStr, int size)
        {
            var managedStr = FromNativeString(nativeStr, size);
            var result = ManagedClass.Magic(managedStr);
            return ToNativeString(result);
        }
    }
}
