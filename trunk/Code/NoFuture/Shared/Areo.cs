using System;
using System.Runtime.InteropServices;

namespace NoFuture
{
    public class Areo
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int left;
            public int right;
            public int top;
            public int bottom;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();
    }
}
