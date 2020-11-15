using System;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper to keep screen capture handle data together
    /// </summary>
    public class ScreenCaptureHandle
    {
        /// <summary>
        /// Handle to the capture object
        /// </summary>
        public readonly IntPtr handle;

        /// <summary>
        /// Indicates if the handle is HWND (TRUE) or HMON (FALSE)
        /// </summary>
        public readonly bool isWindow;

        /// <summary>
        /// Default COnstructor
        /// </summary>
        public ScreenCaptureHandle(IntPtr handle, bool isWindow)
        {
            if (handle == IntPtr.Zero)
                throw new ArgumentException("Invalid handle!");

            this.handle = handle;
            this.isWindow = isWindow;
        }
    }
}
