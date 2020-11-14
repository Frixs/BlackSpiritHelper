using BlackSpiritHelper.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Get basic information about system windows in which the app is running
    /// </summary>
    public class WindowInfo : IWindowInfo
    {
        /// <inheritdoc/>
        public IEnumerable<Process> EnumProcessesWithWindows()
        {
            var processesWithWindows = from p in Process.GetProcesses()
                                       where !string.IsNullOrWhiteSpace(p.MainWindowTitle) && WindowHelper.IsWindowValidForCapture(p.MainWindowHandle)
                                       select p;
            return processesWithWindows;
        }
    }
}
