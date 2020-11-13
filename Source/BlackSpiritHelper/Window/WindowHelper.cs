using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    public static class WindowHelper
    {
        public static void SetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = User32.GetWindowLong(hwnd, User32.GWL_EXSTYLE);
            User32.SetWindowLong(hwnd, User32.GWL_EXSTYLE, extendedStyle | User32.WS_EX_TRANSPARENT);
        }

        public static void UnsetWindowExTransparent(this System.Windows.Window wnd)
        {
            IntPtr hwnd = new WindowInteropHelper(wnd).Handle;
            int extendedStyle = User32.GetWindowLong(hwnd, User32.GWL_EXSTYLE);
            User32.SetWindowLong(hwnd, User32.GWL_EXSTYLE, 0);
        }

        public static IDictionary<IntPtr, string> EnumVisibleWindows()
        {
            Dictionary<IntPtr, string> result = new Dictionary<IntPtr, string>();
            
            User32.EnumWindows(new User32.WindowEnumCallback((IntPtr hwnd, int lparam) =>
            {
                // Get the window's title
                StringBuilder sb_title = new StringBuilder(1024);
                int length = User32.GetWindowText(hwnd, sb_title, sb_title.Capacity);
                string title = sb_title.ToString();

                // If the window is visible and has a title, save it
                if (User32.IsWindowVisible(hwnd) && string.IsNullOrEmpty(title) == false)
                    result.Add(hwnd, title);

                // Return true to indicate that we should continue enumerating windows
                return true;
            }), 0);

            return result;
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            public const int GWL_EXSTYLE = (-20);
            public const int WS_EX_TRANSPARENT = 0x00000020;

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            [DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hwnd, int index);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);
            public delegate bool WindowEnumCallback(IntPtr hwnd, int lparam);

            [DllImport("user32.dll")]
            public static extern int GetWindowText(IntPtr h, StringBuilder s, int nMaxCount);

            [DllImport("user32.dll")]
            public static extern bool IsWindowVisible(IntPtr h);
        }
    }
}
