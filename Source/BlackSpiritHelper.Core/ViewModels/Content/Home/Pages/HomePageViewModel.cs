using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The View Model for the Home page
    /// </summary>
    public class HomePageViewModel : BaseViewModel
    {
        #region Command Flags

        private bool mOpenWebLinkCommandFlag { get; set; }
        private bool mOpenImportExportCommandFlag { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// The command to open web page
        /// </summary>
        public ICommand GetStartedCommand { get; set; }

        /// <summary>
        /// The command to open web page
        /// </summary>
        public ICommand FaqCommand { get; set; }

        /// <summary>
        /// The command to import settings.
        /// </summary>
        public ICommand ImportSettingsCommand { get; set; }

        /// <summary>
        /// The command to export settings.
        /// </summary>
        public ICommand ExportSettingsCommand { get; set; }

        /// <summary>
        /// The command to open author webpage.
        /// </summary>
        public ICommand AuthorWebpageLinkCommand { get; set; }

        /// <summary>
        /// The command to open donation link.
        /// </summary>
        public ICommand AuthorDonateLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HomePageViewModel()
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
            GetStartedCommand = new RelayCommand(async () => await GetStartedCommandMethodAsync());
            FaqCommand = new RelayCommand(async () => await FaqCommandMethodAsync());
            ImportSettingsCommand = new RelayCommand(async () => await ImportSettingsAsync());
            ExportSettingsCommand = new RelayCommand(async () => await ExportSettingsAsync());
            AuthorWebpageLinkCommand = new RelayCommand(async () => await AuthorWebpageLinkMethodAsync());
            AuthorDonateLinkCommand = new RelayCommand(async () => await AuthorDonateLinkMethodAsync());
        }

        /// <summary>
        /// Open webpage with the Get started info.
        /// </summary>
        private async Task GetStartedCommandMethodAsync()
        {
            await RunCommandAsync(() => mOpenWebLinkCommandFlag, async () =>
            {
                // Open the webpage.
                System.Diagnostics.Process.Start("https://github.com/Frixs/BlackSpiritHelper/wiki/GettingStarted");
                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Open webpage with the FAQ info.
        /// </summary>
        private async Task FaqCommandMethodAsync()
        {
            await RunCommandAsync(() => mOpenWebLinkCommandFlag, async () =>
            {
                // Open the webpage.
                System.Diagnostics.Process.Start("https://github.com/Frixs/BlackSpiritHelper/wiki/FAQ");
                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Open author webpage.
        /// </summary>
        private async Task AuthorWebpageLinkMethodAsync()
        {
            await RunCommandAsync(() => mOpenWebLinkCommandFlag, async () =>
            {
                // Open the webpage.
                System.Diagnostics.Process.Start("https://github.com/Frixs");
                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Open donation weblink.
        /// </summary>
        private async Task AuthorDonateLinkMethodAsync()
        {
            await RunCommandAsync(() => mOpenWebLinkCommandFlag, async () =>
            {
                // Open the webpage.
                System.Diagnostics.Process.Start(IoC.Application.DonationURL);
                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Import settings file for user.
        /// </summary>
        /// <returns></returns>
        private async Task ImportSettingsAsync()
        {
            await RunCommandAsync(() => mOpenImportExportCommandFlag, async () =>
            {
                await IoC.UI.ShowFileBrowserDialog((selectedPath) =>
                {
                    // Get file info
                    FileInfo fiNew = new FileInfo(selectedPath);
                    FileInfo fiCurrent = new FileInfo(SettingsConfiguration.UserConfigPath);

                    // Check InUse status
                    short timeoutCurrent = 10; // tries.
                    while (IoC.File.IsInUse(fiCurrent))
                    {
                        Thread.Sleep(1000); // Wait for some amount of time before another try.
                        if (timeoutCurrent > 0)
                            timeoutCurrent -= 1;
                        else
                        {
                            IoC.Logger.Log($"Cannot access log file '{fiCurrent.Name}'!", LogLevel.Error);
                            return;
                        }
                    }

                    // Delete previous settings file first
                    fiCurrent.Delete();

                    // Check InUse status
                    short timeoutNew = 10; // tries.
                    while (IoC.File.IsInUse(fiNew))
                    {
                        Thread.Sleep(1000); // Wait for some amount of time before another try.
                        if (timeoutNew > 0)
                            timeoutNew -= 1;
                        else
                        {
                            IoC.Logger.Log($"Cannot access log file '{fiNew.Name}'!", LogLevel.Error);
                            return;
                        }
                    }

                    // Copy a new settings file to the correct settings file location
                    fiNew.CopyTo(SettingsConfiguration.UserConfigPath);

                    // Confirm dialog to restart the app.
                    IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                    {
                        Title = "RESTART REQUIRED",
                        Message = $"To apply new user settings, you have to restart the application.{Environment.NewLine}Do you want to restart the application now?",
                        Result = NotificationBoxResult.YesNo,
                        YesAction = () =>
                        {
                            IoC.Application.AppAssembly.Restart();
                        },
                    });

                }, "Config files (*.config)|*.config|All files (*.*)|*.*");
            });
        }

        /// <summary>
        /// Export settings file for user.
        /// </summary>
        /// <returns></returns>
        private async Task ExportSettingsAsync()
        {
            await RunCommandAsync(() => mOpenImportExportCommandFlag, async () =>
            {
                await IoC.UI.ShowFolderBrowserDialog((selectedPath) =>
                {
                    FileInfo f = new FileInfo(SettingsConfiguration.UserConfigPath);
                    f.CopyTo(Path.Combine(selectedPath, IoC.Application.ProductName.ToLower().Replace(' ', '_') + ".config"), true);
                });
            });
        }

        #endregion
    }
}
