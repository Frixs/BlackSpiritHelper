using System;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// Abstract class as interface for all watchers of Watchdog.
    /// </summary>
    public abstract class AWatchdogWatcherBase : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Minimal delay which can be set.
        /// Units: Milliseconds
        /// It is set minimal value due to timer which cannot handle zero delay.
        /// It can be set like: > 0
        /// But let take there some space for tests. 1sec is fine.
        /// </summary>
        public static int AllowedMinCheckInterval { get; private set; } = 1000;

        #endregion

        #region Private Members

        /// <summary>
        /// Time for timer loop method.
        /// This private value should be set ONLY through <see cref="IntervalTime"/>.
        /// Set by <see cref="IntervalTimeTicks"/>.
        /// Delay between tests.
        /// </summary>
        private TimeSpan mIntervalTime = TimeSpan.FromMilliseconds(30000);

        #endregion

        #region Protected Members

        /// <summary>
        /// Timer control for checks.
        /// </summary>
        protected Timer mCheckLoopTimer = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Tick time for timer loop method.
        /// Ticks of <see cref="IntervalTime"/>.
        /// Delay between tests.
        /// Units: Millisenconds.
        /// </summary>
        public long IntervalTimeTicks
        {
            get => IntervalTime.Ticks;
            set => IntervalTime = TimeSpan.FromTicks(value);
        }

        /// <summary>
        /// Time for timer loop method.
        /// It is set minimal value due to timer which cannot handle zero delay.
        /// Set by <see cref="IntervalTimeTicks"/>.
        /// Delay between tests.
        /// </summary>
        [XmlIgnore]
        public TimeSpan IntervalTime
        {
            get => mIntervalTime;
            set => mIntervalTime = value.Ticks < AllowedMinCheckInterval ? TimeSpan.FromMilliseconds(AllowedMinCheckInterval) : value;
        }

        /// <summary>
        /// Progress note gives feedback what is happening during check.
        /// </summary>
        [XmlIgnore]
        public string ProgressNote { get; private set; } = "";

        #endregion

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

        #endregion

        #region Protected Methods

        /// <summary>
        /// Update progress note with proper formatting.
        /// </summary>
        /// <param name="note">New note to update</param>
        protected void UpdateProgressNote(string note)
        {
            if (string.IsNullOrEmpty(note))
                ProgressNote = string.Empty;
            else
                ProgressNote = $"({note})";
        }

        /// <summary>
        /// Play the check timer loop.
        /// </summary>
        /// <param name="interval">Interval of the timer loop (> 0)</param>
        /// <returns></returns>
        protected async Task RunWatcherAsync(TimeSpan interval)
        {
            // Update interal first.
            UpdateTimerControlInterval(interval);

            // Update state.
            IsRunning = true;
            IoC.DataContent.WatchdogData.OnPropertyChanged(nameof(IsRunning));

            // Update note.
            UpdateProgressNote("Starting...");

            // Wait the minimal time before first check.
            await Task.Delay(AllowedMinCheckInterval);

            // First check right after start.
            CheckLoopTimerOnElapsed(null, null);

            // Start the timer.
            mCheckLoopTimer.Start();
        }

        /// <summary>
        /// Stop the check timer loop.
        /// </summary>
        /// <returns></returns>
        protected async Task StopWatcherAsync()
        {
            // Update state.
            IsRunning = false;
            IoC.DataContent.WatchdogData.OnPropertyChanged(nameof(IsRunning));

            // Stop the timer.
            mCheckLoopTimer.Stop();

            // Update note.
            UpdateProgressNote(string.Empty);

            await Task.Delay(1);
        }

        #endregion

        #region Abstract Methods (general)

        /// <summary>
        /// Process to check watcher and evaluate what happens based on evaluation.
        /// </summary>
        public abstract void CheckProcess();

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set the timers.
        /// </summary>
        public void SetTimerControl()
        {
            // Set check loop timer.
            mCheckLoopTimer = new Timer(1000);
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
        /// Update timer control interval.
        /// </summary>
        /// <param name="interval"></param>
        protected void UpdateTimerControlInterval(TimeSpan interval)
        {
            mCheckLoopTimer.Interval = interval.TotalMilliseconds;
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void CheckLoopTimerOnElapsed(object sender, ElapsedEventArgs e);

        #endregion
    }
}
