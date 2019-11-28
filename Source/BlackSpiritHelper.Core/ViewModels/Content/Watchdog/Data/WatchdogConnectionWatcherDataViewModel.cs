﻿using BlackSpiritHelper.Core.Data.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Connection Watcher handles connection to the internet and connection of processes to TCP/UDP protocols.
    /// </summary>
    public class WatchdogConnectionWatcherDataViewModel : AWatchdogWatcherBase
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of process connections in a list that can be created.
        /// </summary>
        public static byte AllowedMaxNoOfProcessConnections { get; private set; } = 3;

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

        #region Commands

        /// <summary>
        /// Command to play the watcher.
        /// </summary>
        [XmlIgnore]
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// Command to stop the watcher.
        /// </summary>
        [XmlIgnore]
        public ICommand StopCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogConnectionWatcherDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            PlayCommand = new RelayCommand(async () => await RunWatcherAsync(IntervalTime));
            StopCommand = new RelayCommand(async () => await StopWatcherAsync());
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void CheckLoopTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            CheckProcess();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Process to check watcher and evaluate what happens based on evaluation.
        /// </summary>
        public override void CheckProcess()
        {
            // TODO
            InternetConnection.Check();
            ProcessConnection.Check();
        }

        #endregion
    }
}
