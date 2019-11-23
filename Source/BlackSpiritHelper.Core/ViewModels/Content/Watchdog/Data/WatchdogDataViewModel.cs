using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Watchdog section data ROOT.
    /// </summary>
    public class WatchdogDataViewModel : DataContentBaseViewModel<WatchdogDataViewModel>
    {
        #region Public Properties

        /// <summary>
        /// Handles connection checks - whole subsection of Watchdog.
        /// </summary>
        public WatchdogConnectionWatcherDataViewModel ConnectionWatcher { get; set; } = new WatchdogConnectionWatcherDataViewModel();

        /// <summary>
        /// Says, if watchdog section is running.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogDataViewModel()
        {
        }

        protected override void SetDefaultsMethod()
        {
        }

        protected override void SetupMethod()
        {
            // Set the timers.
            ConnectionWatcher.SetTimerControl();
        }

        protected override void UnsetMethod()
        {
            ConnectionWatcher.DisposeTimerControl();
        }

        #endregion

        
    }
}
