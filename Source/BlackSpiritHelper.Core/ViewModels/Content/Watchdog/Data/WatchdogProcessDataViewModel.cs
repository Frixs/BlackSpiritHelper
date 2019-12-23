using System.Diagnostics;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Windows process structure model.
    /// </summary>
    public class WatchdogProcessDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Process name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Says if the process should be killed on failure.
        /// </summary>
        public bool KillOnFailure { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogProcessDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Find process by the current instance fields.
        /// </summary>
        /// <returns>Null for no results or error, otherwise no null object</returns>
        public Process FindProcess()
        {
            Process ret = null;

            // Get processes coinciding with the name.
            Process[] foundProcesses = Process.GetProcessesByName(Name);
            // We know, we want the only 1 process.
            ret = foundProcesses.Length > 0 ? foundProcesses[0] : ret;

            return ret;
        }

        #endregion
    }
}
