using BlackSpiritHelper.Core.Data.Interfaces;
using System.Collections.ObjectModel;
using System.Timers;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Connection Watcher handles connection to the internet and connection of processes to TCP/UDP protocols.
    /// </summary>
    public class WatchdogConnectionWatcherDataViewModel : AWatchdogConnectionWatcherBase
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of process connections in a list that can be created.
        /// </summary>
        public static byte AllowedMaxNoOfProcessConnections { get; private set; } = 3;

        #endregion

        #region Private Members

        /// <summary>
        /// Timer control for checks.
        /// </summary>
        private Timer mCheckLoopTimer;

        #endregion

        #region Public Properties

        /// <summary>
        /// InternetConnection wrapper.
        /// Handles internet connection.
        /// </summary>
        public WatchdogInternetConnectionDataViewModel InternetConnection { get; set; } = new WatchdogInternetConnectionDataViewModel();

        /// <summary>
        /// Array of multiple Process Connections wrappers.
        /// Each handle independent process connection to TPC/UDP.
        /// </summary>
        public ObservableCollection<WatchdogProcessConnectionDataViewModel> ProcessConnections { get; set; } = new ObservableCollection<WatchdogProcessConnectionDataViewModel>();

        /// <summary>
        /// Tick time for timer loop method.
        /// Delay between tests.
        /// Units: millisenconds.
        /// </summary>
        public int DelayTickTime { get; set; } = 30000;

        /// <summary>
        /// Says if the watcher section is running or not.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning { get; protected set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogConnectionWatcherDataViewModel()
        {
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set the timers.
        /// </summary>
        public void SetTimerControl()
        {
            // Set check loop timer.
            mCheckLoopTimer = new Timer(DelayTickTime);
            mCheckLoopTimer.Elapsed += CheckLoopTimerOnElapsed;
            mCheckLoopTimer.AutoReset = true;
        }

        /// <summary>
        /// Dispose timer calculations.
        /// Use this only while destroying the instance of the timer.
        /// </summary>
        public void DisposeTimerControl()
        {
            // Check loop timer.
            mCheckLoopTimer.Stop();
            mCheckLoopTimer.Elapsed -= CheckLoopTimerOnElapsed;
            mCheckLoopTimer.Dispose();
            mCheckLoopTimer = null;
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckLoopTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
        }

        #endregion
    }
}
