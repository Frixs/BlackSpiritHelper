using BlackSpiritHelper.Core.Data.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// TODO comment
    /// </summary>
    public class WatchdogConnectionControlDataViewModel : AWatchdogConnectionControl
    {
        #region Private Members

        /// <summary>
        /// Timer control for checks.
        /// </summary>
        private Timer mCheckLoopTimer;

        #endregion

        #region Public Properties

        /// <summary>
        /// TODO comment
        /// </summary>
        public WatchdogInternetConnectionDataViewModel InternetConnection = new WatchdogInternetConnectionDataViewModel();

        /// <summary>
        /// TODO comment
        /// </summary>
        public ObservableCollection<WatchdogProcessConnectionDataViewModel> ProcessConnections = new ObservableCollection<WatchdogProcessConnectionDataViewModel>();

        /// <summary>
        /// Tick time for timer loop method.
        /// Delay between tests.
        /// Units: millisenconds.
        /// </summary>
        public int DelayTickTime { get; set; } = 30000;

        /// <summary>
        /// TODO comment
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning { get; protected set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogConnectionControlDataViewModel()
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
