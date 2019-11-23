using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// Abstract class as interface for all watchers of Watchdog.
    /// </summary>
    public abstract class AWatchdogConnectionWatcherBase : BaseViewModel
    {
        /// <summary>
        /// Says if the current watcher is running or not.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        /// <summary>
        /// Each watcher has own user settings for failure actions.
        /// </summary>
        public WatchdogFailureActionDataViewModel FailureAction { get; set; } = new WatchdogFailureActionDataViewModel();
    }
}
