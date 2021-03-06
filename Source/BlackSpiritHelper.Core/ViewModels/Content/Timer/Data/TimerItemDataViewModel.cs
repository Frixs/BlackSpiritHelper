﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer. ViewModel for TimerListItemControl.
    /// Data Content.
    /// </summary>
    public class TimerItemDataViewModel : ASetupableBaseViewModel
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
        public static byte OverlayTimerLimitCount { get; private set; } = 8;

        #endregion

        #region Private Members

        /// <summary>
        /// Timer control.
        /// </summary>
        private Timer mTimer;

        /// <summary>
        /// Timer control - Warning time.
        /// </summary>
        private Timer mWarningTimer;

        /// <summary>
        /// Timer time total.
        /// </summary>
        private TimeSpan mTimeDuration;

        /// <summary>
        /// Countdown before timer starts total.
        /// </summary>
        private TimeSpan mCountdownDuration;

        /// <summary>
        /// Array of notification event fire record.
        /// TRUE = the notification event has been fired.
        /// FALSE = the notification event has NOT been fired yet.
        /// Count = Number of notification events for timer.
        /// </summary>
        private bool[] mIsFiredNotificationEvent = new bool[3];

        #endregion

        #region Public Properties

        /// <summary>
        /// The group id the timer belongs to.
        /// </summary>
        public sbyte GroupID { get; set; } = -1;

        /// <summary>
        /// Timer title.
        /// </summary>
        public string Title { get; set; } = "NoName";

        /// <summary>
        /// Text shortcut to show instead of image icon. Only if there is no image icon.
        /// </summary>
        public string IconTitleShortcut { get; set; } = "XXX";

        /// <summary>
        /// Icon background if there is no image icon.
        /// Color representation in HEX.
        /// f.e. FA9BC2
        /// </summary>
        public string IconBackgroundHEX { get; set; } = "000000";

        /// <summary>
        /// Formatted time output.
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeCurrent { get; private set; }

        /// <summary>
        /// Timer total time.
        /// Set will set <see cref="TimeLeft"/> at the same value.
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeDuration
        {
            get => mTimeDuration;
            set
            {
                mTimeDuration = value;
                UpdateTimeInUI(value);
                TimeLeft = value;
            }
        }

        /// <summary>
        /// Helper for <see cref="TimeDuration"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long TimeDurationTicks
        {
            get => mTimeDuration.Ticks;
            set => mTimeDuration = new TimeSpan(value);
        }

        /// <summary>
        /// Time left to zero.
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeLeft { get; private set; }

        /// <summary>
        /// Helper for <see cref="TimeLeft"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long TimeLeftTicks
        {
            get => TimeLeft.Ticks;
            set => TimeLeft = new TimeSpan(value);
        }

        /// <summary>
        /// Countdown before timer starts total.
        /// </summary>
        [XmlIgnore]
        public TimeSpan CountdownDuration
        {
            get => mCountdownDuration;
            set
            {
                mCountdownDuration = value;
                CountdownLeft = value;
            }
        }

        /// <summary>
        /// Helper for <see cref="CountdownDuration"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long CountdownDurationTicks
        {
            get => mCountdownDuration.Ticks;
            set => mCountdownDuration = new TimeSpan(value);
        }

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        [XmlIgnore]
        public TimeSpan CountdownLeft { get; private set; }

        /// <summary>
        /// Helper for <see cref="CountdownLeft"/>.
        /// It is used to set back the value on application load.
        /// </summary>
        public long CountdownLeftTicks
        {
            get => CountdownLeft.Ticks;
            set => CountdownLeft = new TimeSpan(value);
        }

        /// <summary>
        /// Timer control.
        /// </summary>
        public TimerState State { get; set; } = TimerState.None;

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
        /// ---
        /// NOTE: Do NOT set this manually. It helps on application start to recover timer to its previous state.
        /// </summary>
        public bool IsInCountdown { get; set; }

        /// <summary>
        /// Says, if the timer is in warning time (less than X).
        /// </summary>
        [XmlIgnore]
        public bool WarningFlag { get; private set; }

        #endregion

        #region Command Flags

        private bool mModifyCommandFlag { get; set; }

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
        /// The command to increase time in the timer (10x).
        /// </summary>
        [XmlIgnore]
        public ICommand TimePlusBulkCommand { get; set; }

        /// <summary>
        /// The command to decrease time in the timer.
        /// </summary>
        [XmlIgnore]
        public ICommand TimeMinusCommand { get; set; }

        /// <summary>
        /// The command to decrease time in the timer (10x).
        /// </summary>
        [XmlIgnore]
        public ICommand TimeMinusBulkCommand { get; set; }

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

        /// <summary>
        /// Default constructor.
        /// <see cref="Init"/> method should be called everytime after creation.
        /// </summary>
        public TimerItemDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        protected override void InitRoutine(params object[] parameters)
        {
            // Set the timer.
            SetTimerControl();

            // Update time as placeholder, only in ready state.
            if (State == TimerState.Ready)
                UpdateTimeInUI(TimeDuration);
            // In any other state, update time of countdown if the countdown is live.
            else if (CountdownLeft.TotalSeconds > 0)
                UpdateTimeInUI(CountdownLeft);
            // Update time if there is no live countdown.
            else
                UpdateTimeInUI(TimeLeft);

            // Update state.
            UpdateState(State);

            // Set notification triggers on load.
            TimerSetNotificationEventTriggers(TimeLeft);
        }

        protected override void DisposeRoutine()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set timer control.
        /// </summary>
        private void SetTimerControl()
        {
            // Set normal timer.
            mTimer = new Timer(1000);
            mTimer.Elapsed += TimerOnElapsed;
            mTimer.AutoReset = true;

            // Set warning timer.
            mWarningTimer = new Timer(500);
            mWarningTimer.Elapsed += TimerOnWarningTime;
            mWarningTimer.AutoReset = true;
        }

        /// <summary>
        /// Dispose timer control.
        /// Use this only while destroying the instance of the timer.
        /// </summary>
        public void DisposeTimerControl()
        {
            // Normal timer.
            mTimer.Stop();
            mTimer.Elapsed -= TimerOnElapsed;
            mTimer.Dispose();
            mTimer = null;

            // Warning timer.
            mWarningTimer.Stop();
            mWarningTimer.Elapsed -= TimerOnWarningTime;
            mWarningTimer.Dispose();
            mWarningTimer = null;
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
                // Handle notification events.
                TimerHandleNotificationEvents(currTime);
            }

            // Update time text format in UI.
            UpdateTimeInUI(currTime);

            // Countdown reached zero.
            if (IsInCountdown && CountdownLeft.TotalSeconds <= 0)
            {
                UpdateState(TimerState.Play);
            }

            // Timer reached zero.
            if (TimeLeft.TotalSeconds <= 0)
            {
                // Restart.
                if (IsLoopActive)
                    TimerRestartLoop();
                else
                    TimerRestart();
            }
        }

        /// <summary>
        /// On warning time, event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnWarningTime(object sender, ElapsedEventArgs e)
        {
            WarningFlag = !WarningFlag;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            OpenTimerSettingsCommand = new RelayCommand(async () => await OpenTimerSettingsCommandMethodAsync());
            TimePlusCommand = new RelayCommand(async () => await TimePlusCommandMethodAsync());
            TimePlusBulkCommand = new RelayCommand(async () => await TimePlusBulkCommandMethodAsync());
            TimeMinusCommand = new RelayCommand(async () => await TimeMinusCommandMethodAsync());
            TimeMinusBulkCommand = new RelayCommand(async () => await TimeMinusBulkCommandMethodAsync());
            ResetTimerCommand = new RelayCommand(async () => await ResetTimerCommandMethodAsync());
            SyncCommand = new RelayCommand(async () => await SyncCommandMethodAsync());
            PlayCommand = new RelayCommand(async () => await PlayCommandMethodAsync());
            PauseCommand = new RelayCommand(async () => await PauseCommandMethodAsync());
        }

        /// <summary>
        /// Open timer settings.
        /// </summary>
        /// <returns></returns>
        private async Task OpenTimerSettingsCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                if (State != TimerState.Ready)
                {
                    // Cannot open timer settings while timer is running.
                    await IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                    {
                        Title = "OOOPS...",
                        Message = $"You cannot open the timer settings while the timer is in process.{Environment.NewLine}Please, reset the timer into default state first.",
                        Result = NotificationBoxResult.Ok,
                    });

                    return;
                }

                // Create Settings View Model with the current timer binding.
                TimerItemSettingsFormPageViewModel vm = new TimerItemSettingsFormPageViewModel
                {
                    FormVM = this,
                };

                IoC.Application.GoToPage(ApplicationPage.TimerItemSettingsForm, vm);

                await Task.Delay(1);
            });
        }

        private async Task TimePlusCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await TimePlusAsync(false);
            });
        }

        private async Task TimePlusBulkCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await TimePlusAsync(true);
            });
        }

        private async Task TimeMinusCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await TimeMinusAsync(false);
            });
        }

        private async Task TimeMinusBulkCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await TimeMinusAsync(true);
            });
        }

        /// <summary>
        /// Restart the timer.
        /// </summary>
        /// <returns></returns>
        private async Task ResetTimerCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                TimerRestart();
                await Task.Delay(1);
            });
        }

        private async Task SyncCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await SyncCommandAsync();
            });
        }

        private async Task PlayCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                await PlayAsync();
            });
        }

        /// <summary>
        /// Pause the timer, command reaction.
        /// </summary>
        /// <returns></returns>
        private async Task PauseCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                TimerPause();
                await Task.Delay(1);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play the timer.
        /// </summary>
        public void TimerPlay()
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
            // Otherwise play the timer as usual.
            else
            {
                if (IsInCountdown)
                    // Awake timer to countdown if it was performed.
                    UpdateState(TimerState.Countdown);
                else
                    // Otherwise play the timer.
                    UpdateState(TimerState.Play);
            }

            // Update IsRunning property of the group and timer component itself.
            IoC.DataContent.TimerData.GetGroupByID(GroupID).OnPropertyChanged(nameof(IsRunning));
            IoC.DataContent.TimerData.OnPropertyChanged(nameof(IsRunning));

            // Run the timer.
            mTimer.Start();
        }

        /// <summary>
        /// Play the timer, command reaction.
        /// </summary>
        /// <returns></returns>
        public async Task PlayAsync()
        {
            TimerPlay();

            await Task.Delay(1);
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void TimerPause()
        {
            UpdateState(TimerState.Pause);

            // Update IsRunning property of the group and timer component itself.
            IoC.DataContent.TimerData.GetGroupByID(GroupID).OnPropertyChanged(nameof(IsRunning));
            IoC.DataContent.TimerData.OnPropertyChanged(nameof(IsRunning));

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
        public void TimerRestart()
        {
            TimerPause();

            // Reset warning flag to default.
            TimerTryToDeactivateWarningUI();

            // Update state.
            UpdateState(TimerState.Ready);

            // Reset time.
            TimeLeft = new TimeSpan(TimeDuration.Ticks);
            CountdownLeft = new TimeSpan(CountdownDuration.Ticks);

            // Update UI.
            UpdateTimeInUI(TimeLeft);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update <see cref="TimeCurrent"/> in UI.
        /// </summary>
        /// <param name="ts"></param>
        private void UpdateTimeInUI(TimeSpan ts)
        {
            // Update UI thread.
            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // Update time text format in UI.
                TimeCurrent = ts;
            }));
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
                    IsRunning = true;
                    IsInCountdown = false;
                    IsInFreeze = false;
                    return;

                case TimerState.Pause:
                    // Pause.
                    State = TimerState.Pause;
                    IsRunning = false;
                    IsInFreeze = false;
                    return;

                case TimerState.Ready:
                    // Ready.
                    State = TimerState.Ready;
                    IsRunning = false;
                    IsInCountdown = false;
                    IsInFreeze = false;
                    // Reset notification triggers.
                    TimerResetNotificationEventTriggers();
                    return;

                case TimerState.Freeze:
                    // Freeze.
                    State = TimerState.Freeze;
                    IsRunning = false;
                    IsInFreeze = true;
                    return;

                case TimerState.Countdown:
                    // Countdown.
                    State = TimerState.Countdown;
                    IsRunning = true;
                    IsInCountdown = true;
                    IsInFreeze = false;
                    return;

                default:
                    // Log it.
                    IoC.Logger.Log("A selected timer state value is out of box!", LogLevel.Error);
                    // Break debugger.
                    Debugger.Break();
                    return;
            }
        }

        /// <summary>
        /// Restart the timer loop.
        /// </summary>
        private void TimerRestartLoop()
        {
            TimerPause();

            // Reset warning flag to default.
            TimerTryToDeactivateWarningUI();

            // Reset notification triggers.
            TimerResetNotificationEventTriggers();

            // Reset time.
            TimeLeft = new TimeSpan(TimeDuration.Ticks);

            TimerPlay();
        }

        /// <summary>
        /// Handle timer's notification events.
        /// </summary>
        /// <param name="time"></param>
        private void TimerHandleNotificationEvents(TimeSpan time)
        {
            // ------------------------------
            // 1st Bracket.
            // ------------------------------
            if (time.TotalSeconds > IoC.DataContent.TimerData.TimerNotificationTime1)
            {
                // Time has changed, try to deactivate if the warning UI is running.
                TimerTryToDeactivateWarningUI();
                return;
            }

            // Fire notification event.
            if (!mIsFiredNotificationEvent[0])
            {
                mIsFiredNotificationEvent[0] = true;
                IoC.Audio.Play(AudioSampleType.Alert1, AudioPriorityBracket.Sample);
            }

            // ------------------------------
            // 2nd Bracket.
            // ------------------------------
            if (time.TotalSeconds > IoC.DataContent.TimerData.TimerNotificationTime2)
            {
                // Time has changed, try to deactivate if the warning UI is running.
                TimerTryToDeactivateWarningUI();
                return;
            }

            // Fire notification event.
            if (!mIsFiredNotificationEvent[1])
            {
                mIsFiredNotificationEvent[1] = true;
                IoC.Audio.Play(AudioSampleType.Alert2, AudioPriorityBracket.Sample);
            }

            // Activate WARNING UI event.
            TimerTryToActivateWarningUI();

            // Counting the last seconds.
            if (time.TotalSeconds <= 5 && time.TotalSeconds > 0)
            {
                // The last seconds countdown event.
                IoC.Audio.Play(AudioSampleType.AlertCountdown, AudioPriorityBracket.Sample);
            }

            // ------------------------------
            // 3rd Bracket.
            // ------------------------------
            if (time.TotalSeconds > 0)
            {
                return;
            }

            // Fire notification event.
            if (!mIsFiredNotificationEvent[2])
            {
                mIsFiredNotificationEvent[2] = true;
                IoC.Audio.Play(AudioSampleType.Alert3, AudioPriorityBracket.Sample);
            }

            // Deactivate WARNING UI event.
            TimerTryToDeactivateWarningUI();

        }

        /// <summary>
        /// Set notification event triggers according to time left.
        /// </summary>
        /// <param name="time">Time according to which to set the triggers.</param>
        private void TimerSetNotificationEventTriggers(TimeSpan time)
        {
            // User time brackets.
            int[] brackets = new int[3] {
                IoC.DataContent.TimerData.TimerNotificationTime1,
                IoC.DataContent.TimerData.TimerNotificationTime2,
                0
            };

            for (int i = 0; i < mIsFiredNotificationEvent.Length; i++)
                if (time.TotalSeconds < brackets[i])
                    mIsFiredNotificationEvent[i] = true;
                else
                    mIsFiredNotificationEvent[i] = false;
        }

        /// <summary>
        /// Reset notification event triggers.
        /// </summary>
        private void TimerResetNotificationEventTriggers()
        {
            for (int i = 0; i < mIsFiredNotificationEvent.Length; ++i)
                mIsFiredNotificationEvent[i] = false;
        }

        /// <summary>
        /// Activate WARNING UI event.
        /// If the event is already running, it cannot be run multiple times.
        /// </summary>
        private void TimerTryToActivateWarningUI()
        {
            if (mWarningTimer.Enabled)
                return;

            // Force warning at the beginning immediately.
            WarningFlag = true;

            // Start the event handling.
            mWarningTimer.Start();
        }

        /// <summary>
        /// Deactivate WARNING UI event.
        /// If the event is not running, it cannot be stopped.
        /// </summary>
        private void TimerTryToDeactivateWarningUI()
        {
            if (!mWarningTimer.Enabled)
                return;

            // Force warning off immediately.
            WarningFlag = false;

            // Stop the event handling.
            mWarningTimer.Stop();
        }

        /// <summary>
        /// Increase time.
        /// </summary>
        /// <returns></returns>
        private async Task TimePlusAsync(bool bulkAdd)
        {
            if (!IsRunning)
                return;

            TimeSpan tAdd = TimeSpan.FromSeconds(30 * (bulkAdd ? 10 : 1));
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

                // Update notification event triggers.
                // We are increasing time, the triggers can trigger again.
                TimerSetNotificationEventTriggers(TimeLeft);
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Decrease time.
        /// </summary>
        /// <returns></returns>
        private async Task TimeMinusAsync(bool bulkSubtract)
        {
            if (!IsRunning)
                return;

            TimeSpan tSubtract = TimeSpan.FromSeconds(30 * (bulkSubtract ? 10 : 1));
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

                // Update notification event triggers.
                // We are decreasing time, we do not want to go into the situation where the triggers can trigger simultaneously.
                TimerSetNotificationEventTriggers(TimeLeft);
            }

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

                // Update notification event triggers.
                // We are modifying time, the triggers can trigger again.
                TimerSetNotificationEventTriggers(TimeLeft);
            }

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
        public static bool ValidateInputs(TimerItemDataViewModel vm, string title, string iconTitleShortcut, string iconBackgroundHEX, TimeSpan timeDuration, TimeSpan countdownDuration, bool showInOverlay, TimerGroupDataViewModel associatedGroupViewModel, sbyte currentGroupID)
        {
            #region Title

            if (!new TimerTitleRule().Validate(title, null).IsValid)
                return false;

            #endregion

            #region IconTitleShortcut

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

            if (!iconBackgroundHEX.CheckColorHEX())
                return false;

            #endregion

            #region ShowInOverlay

            var rule6 = new TimerShowInOverlayRule();
            rule6.CurrentShowOverlayValue = vm.ShowInOverlay;
            if (!rule6.Validate(showInOverlay, null).IsValid)
                return false;
            rule6 = null;

            #endregion

            #region AssociatedGroupViewModel

            var rule7 = new TimerAssociatedGroupViewModelRule();
            rule7.CurrentGroupID = currentGroupID;
            if (!rule7.Validate(associatedGroupViewModel, null).IsValid)
                return false;
            rule7 = null;

            #endregion

            return true;
        }

        #endregion
    }
}
