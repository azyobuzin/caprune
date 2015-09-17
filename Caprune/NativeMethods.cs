using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Caprune
{
    static class NativeMethods
    {
        [DllImport("user32")]
        public static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
