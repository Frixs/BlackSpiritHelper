using BlackSpiritHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Get basic information about system windows in which the app is running
    /// </summary>
    public class WindowInfo : IWindowInfo
    {
        /// <inheritdoc/>
        public IntPtr? GetWindowPtr(string windowTitle)
        {
            var windowEnum = WindowHelper.EnumVisibleWindows();
            foreach (var i in windowEnum)
                if (i.Value.Equals(windowTitle))
                    return i.Key;
            return null;
        }

        /// <inheritdoc/>
        public IList<string> EnumVisibleWindows()
        {
            var windows = WindowHelper.EnumVisibleWindows();
            return windows.Select(o => o.Value).ToList();
        }
    }
}
