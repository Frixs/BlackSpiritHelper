using System;
using System.IO;
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

        /// <summary>
        /// Holds the amount of records in the archive - <see cref="CountArchiveRecords"/>
        /// </summary>
        [XmlIgnore]
        public long ArchiveRecordCount { get; private set; }

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

        private bool mArchiveCommandFlags { get; set; }

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

        /// <summary>
        /// Command to export archive
        /// </summary>
        [XmlIgnore]
        public ICommand ExportArchiveCommand { get; set; }

        /// <summary>
        /// Command to reset archive
        /// </summary>
        [XmlIgnore]
        public ICommand ResetArchiveCommand { get; set; }

        /// <summary>
        /// Command to archivate
        /// </summary>
        [XmlIgnore]
        public ICommand ArchivateCommand { get; set; }

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
            ExportArchiveCommand = new RelayCommand(async () => await ExportArchiveCommandMethodAsync());
            ResetArchiveCommand = new RelayCommand(async () => await ResetArchiveCommandMethodAsync());
            ArchivateCommand = new RelayParameterizedCommand(async (parameter) => await ArchivateCommandMethodAsync(parameter));
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

            // Count the archive records
            CountArchiveRecords();
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

        private async Task ExportArchiveCommandMethodAsync()
        {
            await RunCommandAsync(() => mArchiveCommandFlags, async () =>
            {
                await IoC.UI.ShowFolderBrowserDialog((selectedPath) =>
                {
                    FileInfo f = new FileInfo(SettingsConfiguration.ApmCalculatorArchiveFilePath);
                    f.CopyTo(Path.Combine(selectedPath, IoC.Application.ProductName.ToLower().Replace(' ', '_') + "_apm_archive.csv"), true);
                });
                // Log it
                IoC.Logger.Log("APM Calculator archive has been exported!", LogLevel.Info);
            });
        }

        private async Task ResetArchiveCommandMethodAsync()
        {
            await RunCommandAsync(() => mArchiveCommandFlags, async () =>
            {
                // Confirm dialog to restart the app.
                await IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                {
                    Title = "RESET THE ARCHIVE",
                    Message = $"Are you sure you want to proceed?",
                    Result = NotificationBoxResult.YesNo,
                    YesAction = () =>
                    {
                        // If the file does exist...
                        if (File.Exists(SettingsConfiguration.ApmCalculatorArchiveFilePath))
                        {
                            // Delete the file
                            File.Delete(SettingsConfiguration.ApmCalculatorArchiveFilePath);
                            // Recount
                            CountArchiveRecords();
                            // Log it
                            IoC.Logger.Log("APM Calculator archive has been reset!", LogLevel.Info);

                            // Reset archived flags
                            CurrentSession.IsArchived = false;
                            if (LastSession != null)
                                LastSession.IsArchived = false;
                        }
                    },
                });
            });
        }

        private async Task ArchivateCommandMethodAsync(object parameter)
        {
            if (parameter == null || parameter.GetType() != typeof(ApmCalculatorSessionDataViewModel))
                return;

            await RunCommandAsync(() => mArchiveCommandFlags, async () =>
            {
                ApmCalculatorSessionDataViewModel session = (ApmCalculatorSessionDataViewModel)parameter;

                // To prevent possible exception, check if the data dir exists
                if (!Directory.Exists(SettingsConfiguration.DataDirPath))
                    Directory.CreateDirectory(SettingsConfiguration.DataDirPath);

                // If the file does not exist...
                if (!File.Exists(SettingsConfiguration.ApmCalculatorArchiveFilePath))
                {
                    // Create a new file with CSV header
                    await IoC.File.WriteTextToFileAsync($@"START AT;ELAPSED TIME;AVERAGE APM;HIGHEST APM;TOTAL ACTIONS;KEYBOARD;MOUSE CLICK;MOUSE DOUBLE CLICK;MOUSE WHEEL;MOUSE DRAG{Environment.NewLine}",
                        SettingsConfiguration.ApmCalculatorArchiveFilePath,
                        false
                        );
                }

                // Write down the session data
                await IoC.File.WriteTextToFileAsync($@"{session.StartAt:yyyy-MM-dd HH:mm};{session.ElapsedTime};{session.AverageApm};{session.HighestApm};{session.TotalActions};{session.TrackKeyboard};{session.TrackMouseClick};{session.TrackMouseDoubleClick};{session.TrackMouseWheel};{session.TrackMouseDrag}{Environment.NewLine}",
                    SettingsConfiguration.ApmCalculatorArchiveFilePath,
                    true
                    );

                // Flag the session as archived
                session.IsArchived = true;

                // Recount
                CountArchiveRecords();

                // Log it
                IoC.Logger.Log($"Archived APM session (Total Actions: {session.TotalActions}).", LogLevel.Info);
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

        #region Private Methods

        /// <summary>
        /// Start APM calculation session
        /// </summary>
        private void StartSession()
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
        private void StopSession()
        {
            mTimerControl.Stop();
            OnPropertyChanged(nameof(IsRunning));
            IoC.Get<IMouseKeyHook>().UnsubscribeApmCalculatorEvents();

            IoC.Audio.Play(AudioSampleType.StopNotification1, AudioPriorityBracket.Sample);
        }

        /// <summary>
        /// Restart APM calculation session
        /// </summary>
        private void RestartSession()
        {
            LastSession = CurrentSession;
            CurrentSession = new ApmCalculatorSessionDataViewModel();

            IoC.Audio.Play(AudioSampleType.PingNotification1, AudioPriorityBracket.Sample);
        }

        /// <summary>
        /// Count archive records and save it to the count holder-property
        /// </summary>
        private void CountArchiveRecords()
        {
            if (File.Exists(SettingsConfiguration.ApmCalculatorArchiveFilePath))
                ArchiveRecordCount = IoC.File.LineCount(SettingsConfiguration.ApmCalculatorArchiveFilePath) - 1; // -1 header
            else
                ArchiveRecordCount = 0;
        }

        #endregion
    }
}
