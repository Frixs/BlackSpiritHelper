using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    /// <summary>
    /// General Windows library to help to manage windows
    /// </summary>
    public static class WindowHelper
    {
        public static void SetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = User32.GetWindowLong(hwnd, (int)User32.GWL.GWL_EXSTYLE);
            User32.SetWindowLong(hwnd, (int)User32.GWL.GWL_EXSTYLE, extendedStyle | User32.WS_EX_TRANSPARENT);
        }

        public static void UnsetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = User32.GetWindowLong(hwnd, (int)User32.GWL.GWL_EXSTYLE);
            User32.SetWindowLong(hwnd, (int)User32.GWL.GWL_EXSTYLE, 0);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            public const int WS_EX_TRANSPARENT = 0x00000020;

            public enum GWL
            {
                GWL_WNDPROC = (-4),
                GWL_HINSTANCE = (-6),
                GWL_HWNDPARENT = (-8),
                GWL_STYLE = (-16),
                GWL_EXSTYLE = (-20),
                GWL_USERDATA = (-21),
                GWL_ID = (-12)
            }

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            [DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hwnd, int index);
        }
    }
}
