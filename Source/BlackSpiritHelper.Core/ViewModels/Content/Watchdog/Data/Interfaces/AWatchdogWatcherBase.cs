using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// Abstract class as interface for all watchers of Watchdog.
    /// </summary>
    public abstract class AWatchdogWatcherBase : BaseViewModel
    {
        #region Abstract Properties

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

        /// <summary>
        /// Progress note gives feedback what is happening during check.
        /// </summary>
        [XmlIgnore]
        public abstract string ProgressNote { get; protected set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Process to check watcher and evaluate what happens based on evaluation.
        /// </summary>
        public abstract void CheckProcess();

        #endregion
    }
}
