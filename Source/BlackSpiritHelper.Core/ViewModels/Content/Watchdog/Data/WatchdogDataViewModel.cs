using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// TODO comment
    /// </summary>
    public class WatchdogDataViewModel : DataContentBaseViewModel<WatchdogDataViewModel>
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of processes in a list that can be created.
        /// </summary>
        public static byte AllowedMaxNoOfProcesses { get; private set; } = 3;

        #endregion

        #region Public Properties

        public WatchdogConnectionControlDataViewModel ConnectionControl { get; set; } = new WatchdogConnectionControlDataViewModel();

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
            ConnectionControl.SetTimerControl();
        }

        protected override void UnsetMethod()
        {
            ConnectionControl.DisposeTimerControl();
        }

        #endregion

        
    }
}
