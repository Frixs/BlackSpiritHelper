using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents the APM calculator base
    /// </summary>
    public class ApmCalculatorDataViewModel : ADataContentBaseViewModel<ApmCalculatorDataViewModel>
    {
        #region Private Members (General)

        /// <summary>
        /// Timer control instance reference
        /// </summary>
        private Timer mTimerControl;

        /// <summary>
        /// Timer control interval (ms)
        /// </summary>
        private const int mTimerControlInterval = 1000;

        /// <summary>
        /// Max allowed duration for session
        /// </summary>
        private readonly TimeSpan mSessionMaxAllowedDuration = TimeSpan.FromHours(3);

        #endregion

        #region Private Members (Mouse Events)

        /// <summary>
        /// Reference : <see cref="TrackMouseClick"/>
        /// </summary>
        private bool mTrackMouseClick = true;

        /// <summary>
        /// Reference : <see cref="TrackMouseDoubleClick"/>
        /// </summary>
        private bool mTrackMouseDoubleClick = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says if the section should be in the overlay.
        /// </summary>
        public bool ShowInOverlay { get; set; } = false;

        /// <summary>
        /// Indication of tracking keyboard
        /// </summary>
        public bool TrackKeyboard { get; set; } = true;

        /// <summary>
        /// Indication of tracking mouse click
        /// </summary>
        public bool TrackMouseClick 
        {
            get => mTrackMouseClick;
            set
            {
                if (value)
                    TrackMouseDoubleClick = false;
                mTrackMouseClick = value;
            }
        }

        /// <summary>
        /// Indication of tracking mouse double click
        /// </summary>
        public bool TrackMouseDoubleClick 
        {
            get => mTrackMouseDoubleClick;
            set
            {
                if (value)
                    TrackMouseClick = false;
                mTrackMouseDoubleClick = value;
            }
        }

        /// <summary>
        /// Indication of tracking mouse wheel
        /// </summary>
        public bool TrackMouseWheel { get; set; } = false;

        /// <summary>
        /// Indication of tracking mouse drag
        /// </summary>
        public bool TrackMouseDrag { get; set; } = false;

        /// <summary>
        /// Current session data
        /// </summary>
        [XmlIgnore]
        public ApmCalculatorSessionDataViewModel CurrentSession { get; set; }

        /// <summary>
        /// Last session data
        /// </summary>
        [XmlIgnore]
        public ApmCalculatorSessionDataViewModel LastSession { get; set; }

        /// <inheritdoc/>
        [XmlIgnore]
        public override bool IsRunning 
        {
            get => mTimerControl.Enabled;
            protected set => throw new NotImplementedException();
        }

        #endregion

        #region Command Flags

        private bool mPlayStopCommandFlags { get; set; }

        private bool mOpenResultsCommandFlags { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to play the section.
        /// </summary>
        [XmlIgnore]
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// Command to stop the section.
        /// </summary>
        [XmlIgnore]
        public ICommand StopCommand { get; set; }

        /// <summary>
        /// Command to restart the section.
        /// </summary>
        [XmlIgnore]
        public ICommand RestartCommand { get; set; }

        /// <summary>
        /// Command to open results
        /// </summary>
        [XmlIgnore]
        public ICommand OpenResultsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApmCalculatorDataViewModel()
        {
            PlayCommand = new RelayCommand(async () => await PlayCommandMethodAsync());
            StopCommand = new RelayCommand(async () => await StopCommandMethodAsync());
            RestartCommand = new RelayCommand(async () => await RestartCommandMethodAsync());
            OpenResultsCommand = new RelayCommand(async () => await OpenResultsCommandMethodAsync());
        }

        /// <inheritdoc/>
        protected override void DisposeRoutine()
        {
            // Dispose the timer control
            DisposeTimerControl();
        }

        /// <inheritdoc/>
        protected override void InitRoutine(params object[] parameters)
        {
            // Set the timer control
            InitializeTimerControl();

            // Init default current session
            CurrentSession = new ApmCalculatorSessionDataViewModel();
        }

        /// <inheritdoc/>
        protected override void SetDefaultsRoutine()
        {
        }

        #endregion

        #region Command Methods

        private async Task PlayCommandMethodAsync()
        {
            await RunCommandAsync(() => mPlayStopCommandFlags, async () =>
            {
                StartSession();
                await Task.Delay(1);
            });
        }

        private async Task StopCommandMethodAsync()
        {
            await RunCommandAsync(() => mPlayStopCommandFlags, async () =>
            {
                StopSession();
                await Task.Delay(1);
            });
        }

        private async Task RestartCommandMethodAsync()
        {
            await RunCommandAsync(() => mPlayStopCommandFlags, async () =>
            {
                RestartSession();
                await Task.Delay(1);
            });
        }

        private async Task OpenResultsCommandMethodAsync()
        {
            await RunCommandAsync(() => mOpenResultsCommandFlags, async () =>
            {
                // Make sure the main app window is shown
                IoC.UI.ShowMainWindow();
                // Move to the APM page.
                IoC.Application.GoToPage(ApplicationPage.ApmCalculator);

                await Task.Delay(1);
            });
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set timer control.
        /// Use this only while initializing the strcuture
        /// </summary>
        private void InitializeTimerControl()
        {
            mTimerControl = new Timer(mTimerControlInterval);
            mTimerControl.Elapsed += TimerOnElapsed;
            mTimerControl.AutoReset = true;
        }

        /// <summary>
        /// Dispose timer control.
        /// Use this only while destroying the instance of the timer.
        /// </summary>
        private void DisposeTimerControl()
        {
            mTimerControl.Stop();
            mTimerControl.Elapsed -= TimerOnElapsed;
            mTimerControl.Dispose();
            mTimerControl = null;
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            CurrentSession.ElapsedTime += TimeSpan.FromMilliseconds(mTimerControlInterval);

            CurrentSession.ProcessAtRealTime();

            if (CurrentSession.ElapsedTime > mSessionMaxAllowedDuration)
                StopSession();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start APM calculation session
        /// </summary>
        public void StartSession()
        {
            CurrentSession = new ApmCalculatorSessionDataViewModel()
            {
                TrackKeyboard = TrackKeyboard,
                TrackMouseClick = TrackMouseClick,
                TrackMouseDoubleClick = TrackMouseDoubleClick,
                TrackMouseWheel = TrackMouseWheel,
                TrackMouseDrag = TrackMouseDrag
            };

            mTimerControl.Start();
            IoC.Get<IMouseKeyHook>().SubscribeApmCalculatorEvents(CurrentSession);

            OnPropertyChanged(nameof(IsRunning));

            IoC.Audio.Play(AudioSampleType.StartNotification1, AudioPriorityBracket.Sample);
        }

        /// <summary>
        /// Stop APM calculation session
        /// </summary>
        public void StopSession()
        {
            mTimerControl.Stop();
            IoC.Get<IMouseKeyHook>().UnsubscribeApmCalculatorEvents();

            OnPropertyChanged(nameof(IsRunning));

            IoC.Audio.Play(AudioSampleType.StopNotification1, AudioPriorityBracket.Sample);
        }

        /// <summary>
        /// Restart APM calculation session
        /// </summary>
        public void RestartSession()
        {
            LastSession = CurrentSession;
            CurrentSession = new ApmCalculatorSessionDataViewModel();
        }

        #endregion
    }
}
