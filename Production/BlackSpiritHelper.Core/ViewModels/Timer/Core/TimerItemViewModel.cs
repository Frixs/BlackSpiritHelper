using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;

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
        /// Limitation for max duration in <see cref="CountdownDuration"/>.
        /// </summary>
        public static TimeSpan CountdownAllowMaxDuration { get; private set; } = TimeSpan.FromSeconds(7200);

        /// <summary>
        /// Limitation for min duration in <see cref="TimeDuration"/>.
        /// </summary>
        public static TimeSpan TimeAllowMinDuration { get; private set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Limitation for max duration in <see cref="TimeDuration"/>.
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
        private TimerState mState = TimerState.None;

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
        [XmlIgnore]
        public string TimeFormat { get; private set; }

        /// <summary>
        /// Timer total time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeDuration
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
        /// Helper for <see cref="TimeDuration"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long TimeTotalTicks
        {
            get
            {
                return TimeDuration.Ticks;
            }
            set
            {
                TimeDuration = new TimeSpan(value);
            }
        }

        /// <summary>
        /// Time left to zero.
        /// </summary>
        [XmlIgnore]
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
        /// Helper for <see cref="TimeLeft"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long TimeLeftTicks
        {
            get
            {
                return TimeLeft.Ticks;
            }
            set
            {
                TimeLeft = new TimeSpan(value);

                // Update time format on load.
                UpdateTimeInUI(TimeLeft);
            }
        }

        /// <summary>
        /// Countdown before timer starts total.
        /// </summary>
        [XmlIgnore]
        public TimeSpan CountdownDuration
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
        /// Helper for <see cref="CountdownDuration"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long CountdownDurationTotalTicks
        {
            get
            {
                return CountdownDuration.Ticks;
            }
            set
            {
                CountdownDuration = new TimeSpan(value);
            }
        }

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        [XmlIgnore]
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
        /// Helper for <see cref="CountdownLeft"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long CountdownLeftTicks
        {
            get
            {
                return CountdownLeft.Ticks;
            }
            set
            {
                CountdownLeft = new TimeSpan(value);

                // Update time format on load.
                if (CountdownLeft.TotalSeconds > 0)
                    UpdateTimeInUI(CountdownLeft);
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
                TimerState previousState = mState;

                mState = value;

                // Set the initial state parameters at the time of timer creation.
                if (previousState == TimerState.None)
                    UpdateState(value);
            }
        }

        /// <summary>
        /// Says, if the timer is in infinite loop.
        /// </summary>
        public bool IsLoopActive { get; set; }

        /// <summary>
        /// Show this timer in overlay.
        /// </summary>
        public bool ShowInOverlay { get; set; }

        /// <summary>
        /// Says, the timer is currently playing (True) or it is another state (False).
        /// </summary>
        [XmlIgnore]
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Says, if the timer is currently in freeze state.
        /// </summary>
        [XmlIgnore]
        public bool IsInFreeze { get; private set; }

        /// <summary>
        /// Says, the timer is currently in countdown.
        /// NOTE: Do NOT set this manually. It helps on application start to recover timer to its previous state.
        /// </summary>
        public bool IsInCountdown { get; set; }

        /// <summary>
        /// Says, if the timer is in warning time (less than X).
        /// </summary>
        [XmlIgnore]
        public bool IsWarningTime { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command open timer settings.
        /// </summary>
        [XmlIgnore]
        public ICommand OpenTimerSettingsCommand { get; set; }

        /// <summary>
        /// The command to increase time in the timer.
        /// </summary>
        [XmlIgnore]
        public ICommand TimePlusCommand { get; set; }

        /// <summary>
        /// The command to decrease time in the timer.
        /// </summary>
        [XmlIgnore]
        public ICommand TimeMinusCommand { get; set; }

        /// <summary>
        /// The command to reset timer.
        /// </summary>
        [XmlIgnore]
        public ICommand ResetTimerCommand { get; set; }

        /// <summary>
        /// The command to SYNC the timer.
        /// </summary>
        [XmlIgnore]
        public ICommand SyncCommand { get; set; }

        /// <summary>
        /// The command to play/resume the timer.
        /// </summary>
        [XmlIgnore]
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// The command to pause the timer.
        /// </summary>
        [XmlIgnore]
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

            // Update time format on load.
            if (TimeLeft.Ticks < TimeDuration.Ticks)
                UpdateTimeInUI(TimeLeft);
            else if (CountdownLeft.Ticks < CountdownDuration.Ticks)
                UpdateTimeInUI(CountdownLeft);
        }

        /// <summary>
        /// Dispose timer calculations.
        /// Use this only while destroying the instance of the timer.
        /// </summary>
        public void DisposeTimer()
        {
            mTimer.Stop();
            mTimer.Elapsed -= TimerOnElapsed;
            mTimer.Dispose();
            mTimer = null;
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
            // Countdown.
            if (State == TimerState.Countdown)
                // Calculate Countdown time.
                currTime = CountdownLeft = CountdownLeft.Subtract(TimeSpan.FromSeconds(1));

            // Normal timer.
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

                            IsWarningTime = true;
                            // TODO warning UI.
                        }

                        if (currTime.TotalSeconds < 5 && currTime.TotalSeconds > 0)
                        {
                            // The last seconds countdown event.
                            // TODO
                        }
                    }
                } // End - notification events.

            } // End - calculating time.

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
            if (State == TimerState.Ready && CountdownDuration.TotalSeconds > 0)
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
            TimeLeft = new TimeSpan(TimeDuration.Ticks);
            CountdownLeft = new TimeSpan(CountdownDuration.Ticks);

            UpdateTimeInUI(TimeLeft);
        }

        /// <summary>
        /// Restart the timer loop.
        /// </summary>
        private void TimerRestartLoop()
        {
            IsWarningTime = false;

            TimerPause();
            TimeLeft = new TimeSpan(TimeDuration.Ticks);
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
            if (State != TimerState.Ready)
            {
                // Cannot open timer settings while timer is running.
                await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Ooops...",
                    Message = $"You cannot open timer settings while timer is in process. {Environment.NewLine}Please reset the timer into default state, first.",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Information,
                });

                return;
            }

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
            if (!IsRunning)
                return;

            TimeSpan tAdd = TimeSpan.FromSeconds(30);
            TimeSpan tAfterChange;

            // Countdown.
            if (IsInCountdown)
            {
                tAfterChange = TimeSpan.FromSeconds(CountdownLeft.TotalSeconds).Add(tAdd);
                // Cannot increase over the total time.
                if (tAfterChange.TotalSeconds > CountdownDuration.TotalSeconds)
                    CountdownLeft = TimeSpan.FromSeconds(CountdownDuration.TotalSeconds);
                else
                    CountdownLeft = CountdownLeft.Add(TimeSpan.FromSeconds(tAdd.TotalSeconds));

                // Update time UI.
                UpdateTimeInUI(CountdownLeft);
            }
            // Normal timer.
            else
            {
                tAfterChange = TimeSpan.FromSeconds(TimeLeft.TotalSeconds).Add(tAdd);
                // Cannot increase over the total time.
                if (tAfterChange.TotalSeconds > TimeDuration.TotalSeconds)
                    TimeLeft = TimeSpan.FromSeconds(TimeDuration.TotalSeconds);
                else
                    TimeLeft = TimeLeft.Add(TimeSpan.FromSeconds(tAdd.TotalSeconds));

                // Update time UI.
                UpdateTimeInUI(TimeLeft);
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Decrease time.
        /// </summary>
        /// <returns></returns>
        private async Task TimeMinusAsync()
        {
            if (!IsRunning)
                return;

            TimeSpan tSubtract = TimeSpan.FromSeconds(30);
            TimeSpan tAfterChange;

            // Countdown.
            if (IsInCountdown)
            {
                tAfterChange = TimeSpan.FromSeconds(CountdownLeft.TotalSeconds).Subtract(tSubtract);
                // Cannot go below the zero.
                if (tAfterChange.TotalSeconds <= 0)
                    CountdownLeft = TimeSpan.FromSeconds(1); // Set it 1 sec before zero to prevent overflow over the zero.
                else
                    CountdownLeft = CountdownLeft.Subtract(TimeSpan.FromSeconds(tSubtract.TotalSeconds));

                // Update time UI.
                UpdateTimeInUI(CountdownLeft);
            }
            // Normal timer.
            else
            {
                tAfterChange = TimeSpan.FromSeconds(TimeLeft.TotalSeconds).Subtract(tSubtract);
                // Cannot go below the zero.
                if (tAfterChange.TotalSeconds <= 0)
                    TimeLeft = TimeSpan.FromSeconds(1); // Set it 1 sec before zero to prevent overflow over the zero.
                else
                    TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(tSubtract.TotalSeconds));

                // Update time UI.
                UpdateTimeInUI(TimeLeft);
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

        /// <summary>
        /// Synchronization command.
        /// Move your timer to min or to max.
        /// </summary>
        /// <returns></returns>
        private async Task SyncCommandAsync()
        {
            if (!IsRunning)
                return;

            // Countdown.
            if (State == TimerState.Countdown)
            {
                CountdownLeft = TimeSpan.FromSeconds(1);

                // Update time in UI.
                UpdateTimeInUI(CountdownLeft);
            }
            // Normal timer.
            else
            {
                if (TimeLeft.Ticks > TimeDuration.Ticks / 2)
                    TimeLeft = TimeSpan.FromSeconds(TimeDuration.TotalSeconds);
                else
                    TimeLeft = TimeSpan.FromSeconds(1);

                // Update time in UI.
                UpdateTimeInUI(TimeLeft);
            }

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

        #region Validation Methods

        /// <summary>
        /// Check timer parameters.
        /// TRUE, if all parameters are OK and the timer can be created.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="iconTitleShortcut"></param>
        /// <param name="iconBackgroundHEX"></param>
        /// <param name="timeDuration"></param>
        /// <param name="countdownDuration"></param>
        /// <param name="showInOverlay"></param>
        /// <param name="showInOverlay"></param>
        /// <returns></returns>
        public static bool ValidateTimerInputs(string title, string iconTitleShortcut, string iconBackgroundHEX, TimeSpan timeDuration, TimeSpan countdownDuration, bool showInOverlay, TimerGroupViewModel associatedGroupViewModel)
        {
            #region Title

            title = title.Trim();
            if (!new TimerTitleRule().Validate(title, null).IsValid)
                return false;

            #endregion

            #region IconTitleShortcut

            iconTitleShortcut = iconTitleShortcut.Trim();
            if (!new TimerIconTitleShortcutRule().Validate(iconTitleShortcut, null).IsValid)
                return false;

            #endregion

            #region TimeDuration

            if (!new TimerTimeDurationRule().Validate(timeDuration, null).IsValid)
                return false;

            #endregion

            #region CountdownDuration

            if (!new TimerCountdownDurationRule().Validate(countdownDuration.TotalSeconds, null).IsValid)
                return false;

            #endregion

            #region IconBackgroundHEX

            if (!Regex.IsMatch(iconBackgroundHEX, @"^[A-Fa-f0-9]{6}$"))
                return false;

            #endregion

            #region ShowInOverlay

            if (!new TimerShowInOverlayRule().Validate(showInOverlay, null).IsValid)
                return false;

            #endregion

            #region AssociatedGroupViewModel

            if (!new TimerAssociatedGroupViewModelRule().Validate(associatedGroupViewModel, null).IsValid)
                return false;

            #endregion

            return true;
        }

        #endregion
    }
}
