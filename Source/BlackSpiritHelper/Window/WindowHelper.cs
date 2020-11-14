using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
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

        public static bool IsWindowValidForCapture(IntPtr hwnd)
        {
            if (hwnd.ToInt32() == 0)
                return false;

            if (hwnd == User32.GetShellWindow())
                return false;

            if (!User32.IsWindowVisible(hwnd))
                return false;

            if (User32.GetAncestor(hwnd, User32.GetAncestorFlags.GetRoot) != hwnd)
                return false;

            var style = (User32.WindowStyles)(uint)User32.GetWindowLongPtr(hwnd, (int)User32.GWL.GWL_STYLE).ToInt32();
            if (style.HasFlag(User32.WindowStyles.WS_DISABLED))
                return false;

            bool cloaked = false;
            var hrTemp = DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.Cloaked, out cloaked, Marshal.SizeOf<bool>());
            if (hrTemp == 0 && cloaked)
                return false;

            return true;
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            // https://docs.microsoft.com/en-us/windows/win32/gdi/capturing-an-image
            // https://www.cyotek.com/blog/capturing-screenshots-using-csharp-and-p-invoke
            // get the hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            RECT windowRect;
            User32.GetWindowRect(handle, out windowRect);
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
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY | GDI32.CAPTUREBLT);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            try
            {
                // get a .NET image object for it
                return Image.FromHbitmap(hBitmap);
            }
            finally
            {
                // clean up
                GDI32.DeleteDC(hdcDest);
                User32.ReleaseDC(handle, hdcSrc);
                // free up the Bitmap object
                GDI32.DeleteObject(hBitmap);
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public override string ToString()
            {
                return string.Format("Left = {0}, Top = {1}, Right = {2}, Bottom ={3}", left, top, right, bottom);
            }
            public int Width => Math.Abs(right - left);
            public int Height => Math.Abs(bottom - top);
        }

        private enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            public const int CAPTUREBLT = 0x40000000;

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
            public const int WS_EX_TRANSPARENT = 0x00000020;

            public enum GetAncestorFlags
            {
                // Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
                GetParent = 1,
                // Retrieves the root window by walking the chain of parent windows.
                GetRoot = 2,
                // Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
                GetRootOwner = 3
            }

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

            [Flags]
            public enum WindowStyles : uint
            {
                WS_BORDER = 0x800000,
                WS_CAPTION = 0xc00000,
                WS_CHILD = 0x40000000,
                WS_CLIPCHILDREN = 0x2000000,
                WS_CLIPSIBLINGS = 0x4000000,
                WS_DISABLED = 0x8000000,
                WS_DLGFRAME = 0x400000,
                WS_GROUP = 0x20000,
                WS_HSCROLL = 0x100000,
                WS_MAXIMIZE = 0x1000000,
                WS_MAXIMIZEBOX = 0x10000,
                WS_MINIMIZE = 0x20000000,
                WS_MINIMIZEBOX = 0x20000,
                WS_OVERLAPPED = 0x0,
                WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
                WS_POPUP = 0x80000000u,
                WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
                WS_SIZEFRAME = 0x40000,
                WS_SYSMENU = 0x80000,
                WS_TABSTOP = 0x10000,
                WS_VISIBLE = 0x10000000,
                WS_VSCROLL = 0x200000
            }

            // This static method is required because Win32 does not support
            // GetWindowLongPtr directly.
            // http://pinvoke.net/default.aspx/user32/GetWindowLong.html
            public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
            {
                if (IntPtr.Size == 8)
                    return GetWindowLongPtr64(hWnd, nIndex);
                else
                    return GetWindowLongPtr32(hWnd, nIndex);
            }

            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetShellWindow();

            [DllImport("user32.dll", ExactSpelling = true)]
            public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
            public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

            [DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hwnd, int index);

            [DllImport("user32.dll")]
            public static extern bool IsWindowVisible(IntPtr h);
        }
    }
}
