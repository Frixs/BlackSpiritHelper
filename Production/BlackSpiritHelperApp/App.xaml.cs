using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string LogFileLocation = "Log/";
        public const string LogFileName = "Log.txt";
        public const string LogFilePath = LogFileLocation + LogFileName;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            checkForUpdates();
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
