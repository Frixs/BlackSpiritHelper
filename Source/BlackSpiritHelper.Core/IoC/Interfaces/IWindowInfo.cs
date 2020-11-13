using System;
using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Handles functions to share system components as a bitmap
    /// </summary>
    public interface IWindowInfo
    {
        /// <summary>
        /// Get the pointer to the window by giving window title
        /// </summary>
        /// <param name="windowTitle">The window title</param>
        /// <returns>Window Pointer</returns>
        IntPtr? GetWindowPtr(string windowTitle);

        /// <summary>
        /// Enum all visible window titles
        /// </summary>
        /// <returns>List of visible window titles</returns>
        IList<string> EnumVisibleWindows();
    }
}
