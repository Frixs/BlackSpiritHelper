using System;
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
        /// User audio alert level.
        /// </summary>
        public AudioAlert AudioAlertLevel { get; set; } = AudioAlert.None;

        /// <summary>
        /// List of all types of audio alerts.
        /// </summary>
        [XmlIgnore]
        public AudioAlert[] AudioAlertList { get; private set; } = (AudioAlert[])Enum.GetValues(typeof(AudioAlert));

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
        /// The command to open author webpage.
        /// </summary>
        [XmlIgnore]
        public ICommand AuthorWebpageLinkCommand { get; set; }

        /// <summary>
        /// The command to open donation link.
        /// </summary>
        [XmlIgnore]
        public ICommand AuthorDonateLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PreferencesViewModel()
        {
            System.Console.WriteLine("HEY");
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
            ResetOverlayPositionCommand = new RelayCommand(() => ResetOverlayPositionMethod());
            AuthorWebpageLinkCommand = new RelayCommand(() => AuthorWebpageLinkMethod());
            AuthorDonateLinkCommand = new RelayCommand(() => AuthorDonateLinkMethod());
        }

        /// <summary>
        /// Set/Unset - Run the application on system startup.
        /// </summary>
        private async Task RunOnStartUpCheckboxCommandMethodAsync()
        {
            bool runOnStartup = RunOnStartup;
            bool flag = false;

            // TODO error on action.
            await RunCommandAsync(() => flag, async () => {
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

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Reset overlay position to defualt.
        /// </summary>
        private void ResetOverlayPositionMethod()
        {
            // TODO reset overlay position.
            System.Console.WriteLine("ResetOverlayPositionTrigger");
        }

        /// <summary>
        /// Open author webpage.
        /// </summary>
        private void AuthorWebpageLinkMethod()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start("https://github.com/Frixs");
        }

        /// <summary>
        /// Open donation weblink.
        /// </summary>
        private void AuthorDonateLinkMethod()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start(IoC.Application.DonationURL);
        }

        #endregion
    }
}
