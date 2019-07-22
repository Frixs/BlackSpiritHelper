using BlackSpiritHelper.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Dispatcher Unhandled Exception

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log error.
            if (IoC.Logger != null)
            {
                IoC.Logger.Log($"An unhandled exception occurred: {e.Exception.Message}", LogLevel.Fatal);
            }
            MessageBox.Show($"An unhandled exception just occurred: {e.Exception.Message}. {Environment.NewLine}Please, contact the developers to fix the issue.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs.
            base.OnStartup(e);

            // Configuration.
            ApplicationSetup();

            // Log it.
            IoC.Logger.Log("Application starting up...", LogLevel.Info);
            
            // Show the main window.
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            // Check for application available updates.
            CheckForUpdates();
        }

        /// <summary>
        /// Configures our application read for use.
        /// </summary>
        private void ApplicationSetup()
        {
            // Setup IoC.
            IoC.Setup();

            // Bind Logger.
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                new FileLogger(IoC.Application.ApplicationName.Replace(' ', '_').ToLower() + "_log.txt"),
            })
            {
                LogOutputLevel = Debugger.IsAttached ? LogOutputLevel.Debug : LogOutputLevel.Informative
            });

            // Bind task manager.
            IoC.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            // Bind a file manager.
            IoC.Kernel.Bind<IFileManager>().ToConstant(new FileManager());

            // Bind a UI Manager.
            IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());

            // Bind an audio manager.
            IoC.Kernel.Bind<IAudioFactory>().ToConstant(new BaseAudioFactory());

            // Bind Application data content view models.
            IoC.Kernel.Bind<ApplicationDataContent>().ToConstant(new ApplicationDataContent());
            IoC.DataContent.Setup();

            // Bind AssemblyInfo version.
            IoC.Application.ApplicationVersion = "1.0.1.0"; //FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;

            // Bind AssemblyInfo copyright.
            IoC.Application.Copyright = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;

            // Bind Executing Assembly.
            IoC.Application.ApplicationExecutingAssembly = Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Perform tasks at application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save data before exiting application.
            IoC.DataContent.SaveUserData();
        }

        /// <summary>
        /// Checks if a new version of the SW is available.
        /// </summary>
        private void CheckForUpdates()
        {
            string currVersion = IoC.Application.ApplicationVersion;

            try
            {
                var webRequest = WebRequest.Create(@"https://raw.githubusercontent.com/Frixs/BlackSpiritHelper/master/Release/latest_version.txt");
                using (var response = webRequest.GetResponse())
                using (var content = response.GetResponseStream())
                using (var reader = new StreamReader(content))
                {
                    var strContent = reader.ReadLine();

                    // New version available.
                    if (IsVersionNewer(strContent))
                    {
                        IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Caption = "New version!",
                            Message = $"New version is available! {Environment.NewLine}Would you like to download new version now?",
                            Button = MessageBoxButton.YesNo,
                            Icon = MessageBoxImage.Information,
                            YesAction = delegate { Process.Start("https://github.com/Frixs/BlackSpiritHelper#installation"); }
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                // Internet error.
            }
        }

        /// <summary>
        /// Check if there is newer version of the application.
        /// </summary>
        /// <param name="newVersion"></param>
        /// <returns></returns>
        private bool IsVersionNewer(string newVersion)
        {
            string[] newVersionNumbers = newVersion.Split('.');
            string[] currVersionNumbers = IoC.Application.ApplicationVersion.Split('.');
            
            int newVersionNumber, currVersionNumber;

            for (var i = 0; i < newVersionNumbers.Length; i++)
            {
                if (int.TryParse(newVersionNumbers[i], out newVersionNumber) && int.TryParse(currVersionNumbers[i], out currVersionNumber))
                {
                    if (newVersionNumber > currVersionNumber)
                        return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
