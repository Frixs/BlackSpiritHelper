using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// User preferences of the application content.
    /// </summary>
    public class PreferencesViewModel : DataContentBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Run the application on startup.
        /// </summary>
        public bool RunOnStartup { get; set; } = false;

        /// <summary>
        /// RunOnStartup Flag for locking.
        /// </summary>
        [XmlIgnore]
        public bool RunOnStartupFlag { get; set; } = false;

        /// <summary>
        /// User audio alert level.
        /// </summary>
        public AudioAlertLevel AudioAlertLevel { get; set; } = AudioAlertLevel.None;

        /// <summary>
        /// List of all types of audio alerts.
        /// </summary>
        [XmlIgnore]
        public AudioAlertLevel[] AudioAlertLevelList { get; private set; } = (AudioAlertLevel[])Enum.GetValues(typeof(AudioAlertLevel));

        /// <summary>
        /// 1st notification time.
        /// </summary>
        public int TimerNotificationTime1 { get; set; } = 50;

        /// <summary>
        /// 1st notification time. Property to load value from user settings on application load.
        /// </summary>
        [XmlIgnore]
        public double TimerNotificationTime1Value
        {
            get => TimerNotificationTime1;
            set
            {
                if (IoC.DataContent.TimerDesignModel.IsRunning)
                    return;

                TimerNotificationTime1 = (int)value;
            }
        }

        /// <summary>
        /// 2nd notification time.
        /// </summary>
        public int TimerNotificationTime2 { get; set; } = 15;

        /// <summary>
        /// 2nd notification time. Property to load value from user settings on application load.
        /// </summary>
        [XmlIgnore]
        public double TimerNotificationTime2Value
        {
            get => TimerNotificationTime2;
            set
            {
                if (IoC.DataContent.TimerDesignModel.IsRunning)
                    return;

                TimerNotificationTime2 = (int)value;
            }
        }

        public override bool IsRunning => throw new NotImplementedException();

        #endregion

        #region Command

        /// <summary>
        /// The command to set run the application on startup.
        /// </summary>
        [XmlIgnore]
        public ICommand RunOnStartUpCheckboxCommand { get; set; }

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
        public PreferencesViewModel()
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
            RunOnStartUpCheckboxCommand = new RelayCommand(async () => await RunOnStartUpCheckboxCommandMethodAsync());
            ResetOverlayPositionCommand = new RelayCommand(async () => await ResetOverlayPositionAsync());
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
                await Task.Delay(1);

                try
                {
                    // Get Windows register startup subkey location.
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    if (runOnStartup)
                        // Set the application executable into Windows register.
                        key.SetValue(IoC.Application.ApplicationExecutingAssembly.GetName().Name, IoC.Application.ApplicationExecutingAssembly.Location);
                    else
                        // Delete the application register key from the Windows register.
                        key.DeleteValue(IoC.Application.ApplicationExecutingAssembly.GetName().Name);
                }
                catch (Exception ex)
                {
                    if (runOnStartup)
                        IoC.Logger.Log($"Unable to set the application start on system startup.{Environment.NewLine}{ex.Message}", LogLevel.Error);
                    else
                        IoC.Logger.Log($"Unable to unset the application start on system startup.{Environment.NewLine}{ex.Message}", LogLevel.Error);
                }
            });
        }

        /// <summary>
        /// Reset overlay position to defualt.
        /// </summary>
        private async Task ResetOverlayPositionAsync()
        {
            IoC.DataContent.OverlayDesignModel.PosX = 0;
            IoC.DataContent.OverlayDesignModel.PosY = 0;

            await Task.Delay(1);
        }

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
