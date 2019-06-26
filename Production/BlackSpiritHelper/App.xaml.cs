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

            // Setup IoC.
            IoC.Setup();

            // Check for application available updates.
            checkForUpdates();

            // Show the main window.
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
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
