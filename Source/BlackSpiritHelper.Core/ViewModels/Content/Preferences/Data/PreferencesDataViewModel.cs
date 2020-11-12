using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// User preferences of the application content.
    /// </summary>
    public class PreferencesDataViewModel : ADataContentBaseViewModel<PreferencesDataViewModel>
    {
        #region Public Properties

        /// <summary>
        /// Run the application on startup.
        /// </summary>
        public bool RunOnStartup { get; set; } = false;

        /// <summary>
        /// Run the application As Administrator.
        /// </summary>
        [XmlIgnore]
        public bool ForceToRunAsAdministrator { get; set; } //; Set in Constructor region

        /// <summary>
        /// Start the application in tray.
        /// </summary>
        public bool StartInTray { get; set; } = false;

        /// <summary>
        /// Volume of application audio output.
        /// </summary>
        public double Volume { get; set; } = 0.5;

        /// <summary>
        /// RunOnStartup Flag for locking.
        /// </summary>
        [XmlIgnore]
        public bool RunOnStartupFlag { get; private set; } = false;

        /// <summary>
        /// Connection wrapper that handles connection to 3rd party user's software.
        /// </summary>
        public PreferencesConnectionDataViewModel Connection { get; set; } = new PreferencesConnectionDataViewModel();

        /// <summary>
        /// User audio alert level.
        /// </summary>
        public AudioAlertType AudioAlertType { get; set; } = AudioAlertType.Standard;

        /// <summary>
        /// List of all types of audio alerts.
        /// </summary>
        [XmlIgnore]
        public AudioAlertType[] AudioAlertTypeList { get; private set; } = (AudioAlertType[])Enum.GetValues(typeof(AudioAlertType));

        [XmlIgnore]
        public override bool IsRunning
        {
            get => false;
            protected set => throw new NotImplementedException();
        }

        /// <summary>
        /// Installation directory of the app to show the user
        /// </summary>
        [XmlIgnore]
        public string InstallationDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// Data directory of the app to show the user
        /// </summary>
        [XmlIgnore]
        public string DataDirectory => SettingsConfiguration.UserConfigDirPath;

        #endregion

        #region Command

        /// <summary>
        /// The command to set run the application on startup.
        /// </summary>
        [XmlIgnore]
        public ICommand RunOnStartUpCheckboxCommand { get; set; }

        /// <summary>
        /// The command to set run the application as administrator.
        /// </summary>
        [XmlIgnore]
        public ICommand ForceToRunAsAdministratorCommand { get; set; }

        /// <summary>
        /// The command to reset overlay position.
        /// </summary>
        [XmlIgnore]
        public ICommand ResetOverlayPositionCommand { get; set; }

        /// <summary>
        /// The command to export log file.
        /// </summary>
        [XmlIgnore]
        public ICommand ExportLogFileCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PreferencesDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        protected override void InitRoutine(params object[] parameters)
        {
            // Init after application start.
            ForceToRunAsAdministrator = IoC.Application.Cookies.ForceToRunAsAdministrator;

            // Init connection section.
            Connection.Init();
        }

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        protected override void SetDefaultsRoutine()
        {
        }

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        protected override void DisposeRoutine()
        {
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            RunOnStartUpCheckboxCommand = new RelayCommand(async () => await RunOnStartUpCheckboxCommandMethodAsync());
            ForceToRunAsAdministratorCommand = new RelayCommand(async () => await ForceToRunAsAdministratorCommandMethodAsync());
            ResetOverlayPositionCommand = new RelayCommand(async () => await ResetOverlayPositionCommandMethodAsync());
            ExportLogFileCommand = new RelayCommand(async () => await ExportLogAsync());
        }

        /// <summary>
        /// Set/Unset - Run the application on system startup.
        /// </summary>
        private async Task RunOnStartUpCheckboxCommandMethodAsync()
        {
            bool runOnStartup = RunOnStartup;

            await RunCommandAsync(() => RunOnStartupFlag, async () =>
            {
                try
                {
                    // Get Windows register startup subkey location.
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                    if (runOnStartup)
                    {
                        // Set the application executable into Windows register.
                        key.SetValue(
                            IoC.Application.ExecutingAssembly.GetName().Name,
                            Environment.GetFolderPath(Environment.SpecialFolder.Programs) + $"\\{IoC.Application.PublisherName}\\{IoC.Application.ProductName}.appref-ms"
                            );
                    }
                    else
                    {
                        // Delete the application register key from the Windows register.
                        key.DeleteValue(IoC.Application.ExecutingAssembly.GetName().Name);
                    }
                }
                catch (Exception ex)
                {
                    if (runOnStartup)
                        IoC.Logger.Log($"Unable to set the application start on system startup: ({ex.GetType().ToString()}) {ex.Message}", LogLevel.Error);
                    else
                        IoC.Logger.Log($"Unable to unset the application start on system startup: ({ex.GetType().ToString()}) {ex.Message}", LogLevel.Error);
                }

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Set to run the application As Administrator.
        /// </summary>
        /// <returns></returns>
        private async Task ForceToRunAsAdministratorCommandMethodAsync()
        {
            if (!ForceToRunAsAdministrator)
            {
                await Task.Delay(1);
                return;
            }

            await IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
            {
                Title = "RESTART REQUIRED",
                Message = $"In order to take the effect, you have to restart the application.",
                Result = NotificationBoxResult.Ok,
            });
        }

        /// <summary>
        /// Reset overlay position to defualt.
        /// </summary>
        private async Task ResetOverlayPositionCommandMethodAsync()
        {
            IoC.DataContent.OverlayData.BaseOverlay.PosX = 0;
            IoC.DataContent.OverlayData.BaseOverlay.PosY = 0;

            await Task.Delay(1);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Export log file for user.
        /// </summary>
        /// <returns></returns>
        private async Task ExportLogAsync()
        {
            await IoC.UI.ShowFolderBrowserDialog((selectedPath) =>
            {
                foreach (FileInfo f in IoC.Logger.GetLogFiles())
                {
                    f.CopyTo(Path.Combine(selectedPath, f.Name));
                }
            });
        }

        #endregion
    }
}
