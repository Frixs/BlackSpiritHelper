using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer. ViewModel for TimerListItemControl.
    /// </summary>
    public class TimerItemViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Limitation for min characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMinChar { get; private set; } = 3;

        /// <summary>
        /// Limitation for max characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMaxChar { get; private set; } = 20;

        /// <summary>
        /// Limitation for min characters in <see cref="IconTitleShortcut"/>.
        /// </summary>
        public static byte IconTitleAllowMinChar { get; private set; } = 1;

        /// <summary>
        /// Limitation for max characters in <see cref="IconTitleShortcut"/>.
        /// </summary>
        public static byte IconTitleAllowMaxChar { get; private set; } = 3;

        /// <summary>
        /// Limitation for max duration in <see cref="CountdownDurationTotal"/>.
        /// </summary>
        public static TimeSpan CountdownAllowMaxDuration { get; private set; } = TimeSpan.FromSeconds(7200);

        /// <summary>
        /// Limitation for min duration in <see cref="TimeTotal"/>.
        /// </summary>
        public static TimeSpan TimeAllowMinDuration { get; private set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Limitation for max duration in <see cref="TimeTotal"/>.
        /// </summary>
        public static TimeSpan TimeAllowMaxDuration { get; private set; } = TimeSpan.FromDays(2);

        /// <summary>
        /// Limitation for max timers on overlay <see cref="ShowInOverlay"/>.
        /// </summary>
        public static byte OverlayTimerLimitCount { get; private set; } = 6;

        #endregion

        #region Private Members

        /// <summary>
        /// DIspatcherTimer to control the timer.
        /// </summary>
        private Timer mTimer;

        /// <summary>
        /// Timer time total.
        /// </summary>
        private TimeSpan mTimeTotal;

        /// <summary>
        /// Time left to zero.
        /// </summary>
        private TimeSpan mTimeLeft;

        /// <summary>
        /// Countdown before timer starts total.
        /// </summary>
        private TimeSpan mCountdownDurationTotal;

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        private TimeSpan mCountdownLeft;

        /// <summary>
        /// Timer control.
        /// </summary>
        private TimerState mState;

        /// <summary>
        /// Array of notification event fire record.
        /// TRUE = the notification event has been fired.
        /// FALSE = the notification event has NOT been fired yet.
        /// COunt = Number of notification events for timer.
        /// </summary>
        private bool[] IsFiredNotificationEvent = new bool[3];

        #endregion

        #region Public Properties

        /// <summary>
        /// The group id the timer belongs to.
        /// </summary>
        public byte GroupID { get; set; }

        /// <summary>
        /// Timer title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Text shortcut to show instead of image icon. Only if there is no image icon.
        /// </summary>
        public string IconTitleShortcut { get; set; }

        /// <summary>
        /// Icon background if there is no image icon.
        /// Color representation in HEX.
        /// f.e. FA9BC2
        /// </summary>
        public string IconBackgroundHEX { get; set; }

        /// <summary>
        /// Formatted time output.
        /// </summary>
        public string TimeFormat { get; private set; }

        /// <summary>
        /// Timer time total.
        /// </summary>
        public TimeSpan TimeTotal
        {
            get
            {
                return mTimeTotal;
            }
            set
            {
                mTimeTotal = value;
                TimeLeft = mTimeTotal;
                UpdateTimeInUI(mTimeTotal);
            }
        }

        /// <summary>
        /// Time left to zero.
        /// </summary>
        public TimeSpan TimeLeft
        {
            get
            {
                return mTimeLeft;
            }
            private set
            {
                mTimeLeft = value;
            }
        }

        /// <summary>
        /// Countdown before timer starts total.
        /// </summary>
        public TimeSpan CountdownDurationTotal
        {
            get
            {
                return mCountdownDurationTotal;
            }
            set
            {
                mCountdownDurationTotal = value;
                CountdownLeft = mCountdownDurationTotal;
            }
        }

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        public TimeSpan CountdownLeft
        {
            get
            {
                return mCountdownLeft;
            }
            private set
            {
                mCountdownLeft = value;
            }
        }

        /// <summary>
        /// Timer control.
        /// </summary>
        public TimerState State
        {
            get
            {
                return mState;
            }
            set
            {
                mState = value;

                // Set the initial state parameters.
                if (mState == TimerState.None)
                    UpdateState(value);
            }
        }

        /// <summary>
        /// Says, the timer is currently playing (True) or it is another state (False).
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Says, the timer is currently in countdown.
        /// </summary>
        public bool IsInCountdown { get; private set; }

        /// <summary>
        /// Says, if the timer is in infinite loop.
        /// </summary>
        public bool IsLoopActive { get; set; }

        /// <summary>
        /// Says, if the timer is currently in freeze state.
        /// </summary>
        public bool IsInFreeze { get; private set; }

        /// <summary>
        /// Show this timer in overlay.
        /// </summary>
        public bool ShowInOverlay { get; set; }

        /// <summary>
        /// Says, if the timer is in warning time (less than X).
        /// </summary>
        public bool IsWarningTime { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command open timer settings.
        /// </summary>
        public ICommand OpenTimerSettingsCommand { get; set; }

        /// <summary>
        /// The command to increase time in the timer.
        /// </summary>
        public ICommand TimePlusCommand { get; set; }

        /// <summary>
        /// The command to decrease time in the timer.
        /// </summary>
        public ICommand TimeMinusCommand { get; set; }

        /// <summary>
        /// The command to reset timer.
        /// </summary>
        public ICommand ResetTimerCommand { get; set; }

        /// <summary>
        /// The command to SYNC the timer.
        /// </summary>
        public ICommand SyncCommand { get; set; }

        /// <summary>
        /// The command to play/resume the timer.
        /// </summary>
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// The command to pause the timer.
        /// </summary>
        public ICommand PauseCommand { get; set; }

        #endregion

        #region Constructor

        public TimerItemViewModel()
        {
            // Set the timer.
            SetTimer();

            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set the timer.
        /// </summary>
        private void SetTimer()
        {
            mTimer = new Timer(1000);
            mTimer.Elapsed += TimerOnElapsed;
            mTimer.AutoReset = true;

            IsRunning = false;
            IsInCountdown = false;
            IsWarningTime = false;
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan currTime;

            // Decide which time to pick according to timer state.
            if (State == TimerState.Countdown)
                // Calculate Countdown time.
                currTime = CountdownLeft = CountdownLeft.Subtract(TimeSpan.FromSeconds(1));
            else
            {
                // Calculate Timer time.
                currTime = TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(1));

                // Fire notification events.
                // TODO Add option to user to set these time brackets.
                if (currTime.TotalSeconds < 50)
                {
                    // Fire The 1st notification event.
                    if (!IsFiredNotificationEvent[0])
                    {
                        IsFiredNotificationEvent[0] = true;
                        // TODO
                    }

                    if (currTime.TotalSeconds < 15)
                    {
                        // Fire The 2nd notification event.
                        if (!IsFiredNotificationEvent[1])
                        {
                            IsFiredNotificationEvent[1] = true;
                            // TODO
                        }

                        if (currTime.TotalSeconds < 5 && currTime.TotalSeconds > 0)
                        {
                            IsWarningTime = true;
                            // TODO warning UI.

                            // The last seconds countdown event.
                            // TODO
                        }
                    }
                }

            }

            // Update UI thread.
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Update time text format in UI.
                UpdateTimeInUI(currTime);
            });

            // Countdown reached zero.
            if (IsInCountdown && CountdownLeft.TotalSeconds <= 0)
                UpdateState(TimerState.Play);

            // Timer reached zero.
            if (TimeLeft.TotalSeconds <= 0)
            {
                // Fire 3rd (on-timer-end) notification event.
                if (!IsFiredNotificationEvent[2])
                {
                    IsFiredNotificationEvent[2] = true;
                    // TODO
                }

                // Restart.
                if (IsLoopActive)
                    TimerRestartLoop();
                else
                    TimerRestart();
            }
        }

        /// <summary>
        /// Update <see cref="TimeFormat"/> in UI.
        /// </summary>
        /// <param name="ts"></param>
        private void UpdateTimeInUI(TimeSpan ts)
        {
            // Update time text format in UI.
            TimeFormat = ts.ToString("hh':'mm':'ss");
        }

        /// <summary>
        /// Update Timer state parameters.
        /// </summary>
        /// <param name="state">The state to update.</param>
        private void UpdateState(TimerState state)
        {
            switch (state)
            {
                case TimerState.Play:
                    // Play.
                    State = TimerState.Play;
                    IsRunning       = true;
                    IsInCountdown   = false;
                    IsInFreeze      = false;
                    return;

                case TimerState.Pause:
                    // Pause.
                    State = TimerState.Pause;
                    IsRunning       = false;
                    IsInFreeze      = false;
                    return;

                case TimerState.Ready:
                    // Ready.
                    State = TimerState.Ready;
                    IsRunning       = false;
                    IsInCountdown   = false;
                    IsInFreeze      = false;
                    return;

                case TimerState.Freeze:
                    // Freeze.
                    State = TimerState.Freeze;
                    IsRunning       = false;
                    IsInFreeze      = true;
                    return;

                case TimerState.Countdown:
                    // Countdown.
                    State = TimerState.Countdown;
                    IsRunning       = true;
                    IsInCountdown   = true;
                    IsInFreeze      = false;
                    return;

                default:
                    // Break debugger.
                    Debugger.Break();
                    return;
            }
        }

        /// <summary>
        /// Play the timer.
        /// </summary>
        private void TimerPlay()
        {
            // Timer is starting from Ready (after reset).
            if (State == TimerState.Ready && CountdownDurationTotal.TotalSeconds > 0)
            {
                // Set start countdown.
                UpdateState(TimerState.Countdown);

                // Update time text format in UI.
                UpdateTimeInUI(CountdownLeft);
            }
            // Timer is starting from Freeze.
            else if (State == TimerState.Freeze)
            {
                if (IsInCountdown)
                    // Awake timer to countdown if it was performed before Freeze.
                    UpdateState(TimerState.Countdown);
                else
                    // Otherwise play the timer.
                    UpdateState(TimerState.Play);
            }
            // Otherwise play the timer.
            else
            {
                UpdateState(TimerState.Play);
            }

            // Run the timer.
            mTimer.Start();
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        private void TimerPause()
        {
            UpdateState(TimerState.Pause);
            mTimer.Stop();
        }

        /// <summary>
        /// Freeze the timer.
        /// Used when the application is closed.
        /// </summary>
        public void TimerFreeze()
        {
            UpdateState(TimerState.Freeze);
            mTimer.Stop();
        }

        /// <summary>
        /// Restart the timer.
        /// </summary>
        private void TimerRestart()
        {
            IsWarningTime = false;

            TimerPause();

            UpdateState(TimerState.Ready);

            // Reset time.
            TimeLeft = TimeSpan.FromSeconds(TimeTotal.TotalSeconds);
            CountdownLeft = TimeSpan.FromSeconds(CountdownDurationTotal.TotalSeconds);

            UpdateTimeInUI(TimeLeft);
        }

        /// <summary>
        /// Restart the timer loop.
        /// </summary>
        private void TimerRestartLoop()
        {
            IsWarningTime = false;

            TimerPause();
            TimeLeft = TimeSpan.FromSeconds(TimeTotal.TotalSeconds);
            TimerPlay();
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            OpenTimerSettingsCommand = new RelayCommand(async () => await OpenTimerSettingsAsync());
            TimePlusCommand = new RelayCommand(async () => await TimePlusAsync());
            TimeMinusCommand = new RelayCommand(async () => await TimeMinusAsync());
            ResetTimerCommand = new RelayCommand(async () => await ResetTimerAsync());
            SyncCommand = new RelayCommand(async () => await SyncCommandAsync());
            PlayCommand = new RelayCommand(async () => await PlayAsync());
            PauseCommand = new RelayCommand(async () => await PauseAsync());
        }

        /// <summary>
        /// Open timer settings.
        /// </summary>
        /// <returns></returns>
        private async Task OpenTimerSettingsAsync()
        {
            // Create Settings View Model with the current timer binding.
            TimerItemSettingsFormViewModel vm = new TimerItemSettingsFormViewModel
            {
                TimerItemViewModel = this,
            };

            IoC.Application.GoToPage(ApplicationPage.TimerItemSettingsForm, vm);

            await Task.Delay(1);
        }

        /// <summary>
        /// Increase time.
        /// </summary>
        /// <returns></returns>
        private async Task TimePlusAsync()
        {
            TimeSpan tAdd = TimeSpan.FromSeconds(30);
            TimeSpan tAfterChange = TimeSpan.FromSeconds(TimeLeft.TotalSeconds).Add(tAdd);

            // Cannot increase over the total time.
            if (tAfterChange.TotalSeconds > TimeTotal.TotalSeconds)
            {
                TimeLeft = TimeSpan.FromSeconds(TimeTotal.TotalSeconds);
            }
            else
            {
                TimeLeft = TimeLeft.Add(TimeSpan.FromSeconds(tAdd.TotalSeconds));
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Decrease time.
        /// </summary>
        /// <returns></returns>
        private async Task TimeMinusAsync()
        {
            TimeSpan tSubtract = TimeSpan.FromSeconds(30);
            TimeSpan tAfterChange = TimeSpan.FromSeconds(TimeLeft.TotalSeconds - 1).Subtract(tSubtract); // -1 to prevent overlap.

            // Cannot go below the zero.
            if (tAfterChange.TotalSeconds <= 0)
            {
                TimeLeft = TimeSpan.FromSeconds(0);
            }
            else
            {
                TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(tSubtract.TotalSeconds));
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Restart the timer command reaction.
        /// </summary>
        /// <returns></returns>
        private async Task ResetTimerAsync()
        {
            TimerRestart();

            await Task.Delay(1);
        }

        private async Task SyncCommandAsync()
        {
            // TODO Sync timer.
            Console.WriteLine("TODO");
            TimerFreeze();
            await Task.Delay(1);
        }

        /// <summary>
        /// Play the timer, command reaction.
        /// </summary>
        /// <returns></returns>
        private async Task PlayAsync()
        {
            TimerPlay();

            await Task.Delay(1);
        }

        /// <summary>
        /// Pause the timer, command reaction.
        /// </summary>
        /// <returns></returns>
        private async Task PauseAsync()
        {
            TimerPause();

            await Task.Delay(1);
        }

        #endregion
    }
}
