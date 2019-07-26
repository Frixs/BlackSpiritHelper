using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    public static class WindowHelper
    {
        const int GWL_EXSTYLE = (-20);
        const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        public static void SetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        public static void UnsetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, 0);
        }
    }
}
