using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Handles functions to share system components as a bitmap
    /// </summary>
    public interface IWindowInfo
    {
        /// <summary>
        /// Enum all processes with windows
        /// </summary>
        /// <returns>List of processes</returns>
        IEnumerable<Process> EnumProcessesWithWindows();
    }
}
