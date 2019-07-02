using BlackSpiritHelper.Core;
using System;
using System.Reflection;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
            IoC.Logger.Log("Application starting up...", LogLevel.Debug);

            // Check for application available updates.
            checkForUpdates();

            // Show the main window.
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
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
                // TODO: log path
                new FileLogger("Log/Log.txt"),
            }));

            // Bind task manager.
            IoC.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            // Bind file manager.
            IoC.Kernel.Bind<IFileManager>().ToConstant(new FileManager());
        }

        /// <summary>
        /// Checks if a new version of the SW is available.
        /// </summary>
        private void checkForUpdates()
        {
            string title = (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            Version currVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // TODO: Version check.
            if (true)
            {
                return;
            }
            
            // Dialog window.
            string messageBoxText = "New version of " + title + " is available!\r\nDo you want to download it now?";
            string caption = title + " - New version available!";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
