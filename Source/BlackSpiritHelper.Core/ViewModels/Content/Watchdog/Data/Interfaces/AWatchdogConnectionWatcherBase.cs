using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// Abstract class as interface for all watchers of Watchdog.
    /// </summary>
    public abstract class AWatchdogConnectionWatcherBase : BaseViewModel
    {
        /// <summary>
        /// Run the watche when the application starts.
        /// </summary>
        public abstract bool RunOnApplicationStart { get; set; }

        /// <summary>
        /// Says if the current watcher is running or not.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        /// <summary>
        /// Each watcher has own user settings for failure actions.
        /// </summary>
        public abstract WatchdogFailureActionDataViewModel FailureAction { get; set; }
    }
}
