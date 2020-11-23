using BlackSpiritHelper.Core.Data.Interfaces;
using System;
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
        public override WatchdogFailureRoutineDataViewModel FailureRoutine { get; set; } = new WatchdogFailureRoutineDataViewModel();

        #endregion

        #region Command Flags

        private bool mModifyCommandFlag { get; set; }

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

        protected override void InitRoutine(params object[] parameters)
        {
            SetTimerControl();
        }

        protected override void DisposeRoutine()
        {
            DisposeTimerControl();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            PlayCommand = new RelayCommand(async () => await PlayCommandMethodAsync());
            StopCommand = new RelayCommand(async () => await StopCommandMethodAsync());
        }

        private async Task PlayCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await RunWatcherAsync(IntervalTime);
            });
        }

        private async Task StopCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await StopWatcherAsync();
            });
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        protected override void CheckLoopTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var datetime = DateTimeOffset.UtcNow.ToString("MM-dd HH:mm UTC");

            UpdateProgressNote("Checking...");
            CheckProcess();
            UpdateProgressNote($"Last check: {datetime}");

            // Fire next cycle of the timer.
            if (IsRunning) // Check if the timer has NOT been stopped during processing current cycle.
                mCheckLoopTimer.Start();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Process to check watcher and evaluate what happens based on evaluation.
        /// </summary>
        public override void CheckProcess()
        {
            bool isOk = true;

            // Check.
            isOk = isOk ? (InternetConnection.IsSelected ? InternetConnection.Check() : true) : false;
            isOk = isOk ? (ProcessConnection.IsSelected ? ProcessConnection.Check() : true) : false;

            // Failure hook.
            if (!isOk && !mIsFailureRoutineFired)
            {
                mIsFailureRoutineFired = true; // Let know the procedure to do not fire this the same event again until the failure disappear.
                FailureRoutine.Do();
            }
            // First occurance of recovery from failure.
            else if (isOk && mIsFailureRoutineFired)
            {
                mIsFailureRoutineFired = false; // Reset value back to false to be able to record failure event again when occurs.
            }
        }

        #endregion
    }
}
