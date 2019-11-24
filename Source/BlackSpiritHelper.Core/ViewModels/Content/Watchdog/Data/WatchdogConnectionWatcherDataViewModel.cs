using BlackSpiritHelper.Core.Data.Interfaces;
using System;
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

        /// <summary>
        /// Minimal delay which can be set.
        /// Units: Milliseconds
        /// It is set minimal value due to timer which cannot handle zero delay.
        /// It can be set like: > 0
        /// But let take there some space for tests. 1sec is fine.
        /// </summary>
        public static long AllowedMinCheckDelay { get; private set; } = 1000;

        #endregion

        #region Private Members

        /// <summary>
        /// Time for timer loop method.
        /// This private value should be set ONLY through <see cref="DelayTime"/>.
        /// Set by <see cref="DelayTimeTicks"/>.
        /// Delay between tests.
        /// </summary>
        private TimeSpan mDelayTime = TimeSpan.FromMilliseconds(30000);

        /// <summary>
        /// Timer control for checks.
        /// </summary>
        private Timer mCheckLoopTimer;

        #endregion

        #region Public Properties

        /// <summary>
        /// Internet Connection wrapper.
        /// Handles internet connection.
        /// </summary>
        public WatchdogInternetConnectionDataViewModel InternetConnection { get; set; } = new WatchdogInternetConnectionDataViewModel();

        /// <summary>
        /// Process Connections wrapper.
        /// Handle independent process connections to TPC/UDP.
        /// </summary>
        public WatchdogProcessConnectionDataViewModel ProcessConnection { get; set; } = new WatchdogProcessConnectionDataViewModel();

        /// <summary>
        /// Tick time for timer loop method.
        /// Ticks of <see cref="DelayTime"/>.
        /// Delay between tests.
        /// Units: Millisenconds.
        /// </summary>
        public long DelayTimeTicks 
        {
            get => DelayTime.Ticks;
            set => DelayTime = TimeSpan.FromTicks(value);
        }

        /// <summary>
        /// Time for timer loop method.
        /// It is set minimal value due to timer which cannot handle zero delay.
        /// Set by <see cref="DelayTimeTicks"/>.
        /// Delay between tests.
        /// </summary>
        [XmlIgnore]
        public TimeSpan DelayTime 
        {
            get => mDelayTime;
            set => mDelayTime = value.Ticks < AllowedMinCheckDelay ? TimeSpan.FromMilliseconds(AllowedMinCheckDelay) : value;
        }

        /// <summary>
        /// Run the watche when the application starts.
        /// </summary>
        public override bool RunOnApplicationStart { get; set; } = false;

        /// <summary>
        /// Says if the watcher section is running or not.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning { get; protected set; } = false;

        /// <summary>
        /// Each watcher has own user settings for failure actions.
        /// </summary>
        public override WatchdogFailureActionDataViewModel FailureAction { get; set; } = new WatchdogFailureActionDataViewModel();

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
            mCheckLoopTimer = new Timer(DelayTimeTicks);
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
