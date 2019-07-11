using System;
using System.Threading.Tasks;
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
        /// Limitation for max duration in <see cref="CountdownDuration"/>.
        /// </summary>
        public static TimeSpan CountdownAllowMaxDuration { get; private set; } = TimeSpan.FromSeconds(7200);

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
        private DispatcherTimer mTimer;

        /// <summary>
        /// Timer time total.
        /// </summary>
        private TimeSpan mTimeTotal;

        /// <summary>
        /// Time left to zero.
        /// </summary>
        private TimeSpan mTimeLeft;

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
                mTimeLeft = mTimeTotal;
                TimeFormat = mTimeTotal.ToString("hh':'mm':'ss");
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
                TimeFormat = mTimeTotal.ToString("hh':'mm':'ss");
            }
        }

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        public TimeSpan CountdownDuration { get; set; }

        /// <summary>
        /// Button control.
        /// </summary>
        public TimerState State { get; set; }

        /// <summary>
        /// Says, the timer is currently playing (True) or it is another state (False).
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Says, if the timer is in infinite loop.
        /// </summary>
        public bool IsLoopActive { get; set; }

        /// <summary>
        /// Show this timer in overlay.
        /// </summary>
        public bool ShowInOverlay { get; set; }

        /// <summary>
        /// Says, if the timer is in warning time (less than X).
        /// </summary>
        public bool IsWarningTime { get; set; }

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
            mTimer = new DispatcherTimer();

            // Create commands.
            CreateCommands();
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

        private async Task TimePlusAsync()
        {
            // TODO.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task TimeMinusAsync()
        {
            // TODO.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task ResetTimerAsync()
        {
            // TODO Reset timer.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task SyncCommandAsync()
        {
            // TODO Sync timer.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task PlayAsync()
        {
            // TODO Play timer.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task PauseAsync()
        {
            // TODO Pause timer.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        #endregion
    }
}
