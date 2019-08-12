using BlackSpiritHelper.Core;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Deployment.Application;
using System.Security.Principal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Members

        /// <summary>
        /// Says if the application is going to restart in order to achieve administrator privileges.
        /// </summary>
        private bool mIsRestartingProcessFlag = false;

        #endregion

        #region Dispatcher Unhandled Exception

        /// <summary>
        /// Unhandled exception event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Start/Exit Methods

        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs.
            base.OnStartup(e);
            
            // Configuration.
            ApplicationSetup(CompileArguments(e.Args));

            // Setup on application deployment.
            OnDeploymentSetup();
            // Setup on application update.
            OnUpdateSetup();

            // Check for administrator privileges.
            if (IoC.DataContent.PreferencesDesignModel.ForceToRunAsAdministrator
                && !IsRunAsAdministrator()
                && !Debugger.IsAttached
                )
            {
                RunAsAdministrator();
                return;
            }

            // Log it.
            IoC.Logger.Log("Application starting up...", LogLevel.Info);

            // Show the main window.
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Perform tasks at application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Dispose.
            IoC.Get<IMouseKeyHook>().Dispose();

            if (!mIsRestartingProcessFlag)
            {
                // "Prepare data to die."
                IoC.DataContent.Unset();

                // Save data before exiting application.
                IoC.DataContent.SaveUserData();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configures our application read for use.
        /// </summary>
        /// <param name="args"></param>
        private void ApplicationSetup(Dictionary<string, string> args)
        {
            // Setup IoC.
            IoC.Setup(args);

            #region Set Application Properties
            // Bind Executing Assembly.
            IoC.Application.ApplicationExecutingAssembly = Assembly.GetExecutingAssembly();

            // Bind AssemblyInfo version.
            IoC.Application.ApplicationVersion = ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : (args.ContainsKey("Version") ? args["Version"] : FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion);

            // Bind AssemblyInfo copyright.
            IoC.Application.Copyright = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
            #endregion

            // Bind Logger.
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                new FileLogger(
                    ((AssemblyTitleAttribute)IoC.Application.ApplicationExecutingAssembly.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title.Replace(' ', '_').ToLower() + "_log.log"
                    ),
            })
            {
                LogOutputLevel = Debugger.IsAttached ? LogOutputLevel.Debug : LogOutputLevel.Informative
            });

            // Bind task manager.
            IoC.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            // Bind dispatcher manager.
            IoC.Kernel.Bind<IDispatcherFactory>().ToConstant(new DispatcherManager());

            // Bind a file manager.
            IoC.Kernel.Bind<IFileManager>().ToConstant(new FileManager());

            // Bind a UI Manager.
            IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());

            // Bind an datetime (with time zones) manager.
            IoC.Kernel.Bind<IDateTimeZone>().ToConstant(new DateTimeZoneManager());

            // Bind an audio manager.
            IoC.Kernel.Bind<IAudioFactory>().ToConstant(new BaseAudioFactory());

            // Bind an mouse key hooks.
            IoC.Kernel.Bind<IMouseKeyHook>().ToConstant(new GlobalMouseKeyHookManager());

            // Bind Application data content view models.
            IoC.Kernel.Bind<ApplicationDataContent>().ToConstant(new ApplicationDataContent());
            IoC.DataContent.Setup();
        }

        /// <summary>
        /// Check if the application is running As Administrator or not.
        /// </summary>
        /// <returns></returns>
        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Run the application As Administrator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunAsAdministrator()
        {
            // It is not possible to launch a ClickOnce app as administrator directly, so instead we launch the app as administrator in a new process.
            var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

            // The following properties run the new process as administrator.
            processInfo.UseShellExecute = true;
            processInfo.Verb = "runas";

            processInfo.Arguments = "Version=" + (ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion);

            // Start the new process.
            try
            {
                Process.Start(processInfo);
                mIsRestartingProcessFlag = true;
            }
            catch (Exception)
            {
                // The user did not allow the application to run as administrator.
                MessageBox.Show($"Sorry, this application must be run As Administrator in order to interact with the overlay while you are playing your game.{Environment.NewLine}Your computer is not allowing to start the application As Administrator.");
                return;
            }

            // Shut down the current process.
            Current.Shutdown();
        }

        /// <summary>
        ///  On application deployment.
        /// </summary>
        private void OnDeploymentSetup()
        {
            if (!ApplicationDeployment.IsNetworkDeployed || !ApplicationDeployment.CurrentDeployment.IsFirstRun)
                return;

            // Only run this code if it is ClickOnce's application and if the application runs for the first time (condition above).

            // Set the application icon on the first time deployment only.
            SetInstallerIcon();
        }

        /// <summary>
        /// This method is fired on each version update or application first deployment.
        /// </summary>
        private void OnUpdateSetup()
        {
            // TODO ;;;
            //if (Debugger.IsAttached)
            //    return;

            bool procedureFailure = true;

            // File relative to execution directory.
            string filePath = "Version.check/" + IoC.Application.ApplicationVersion.Replace('.', '_');

            // To prevent possible exception.
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // If the file exists, do nothing.
            //if (File.Exists(filePath))
            //    return;

            // if the file does not exist, we need to run on update procedure.
            #region Procedure

            Console.WriteLine("A");
            IoC.UI.ShowMessage(new MessageBoxDialogViewModel
            {
                
            });
            Console.WriteLine("B");

            //if (!DataProvider.Instance.DownloadData(SettingsConfiguration.RemoteDataDirPath))
            //    procedureFailure = true;

            #endregion

            // If the procedure successfully finished, create a new check file of the current ersion.
            if (!procedureFailure)
            {
                File.Create(filePath);
            }
        }

        /// <summary>
        /// Set the icon in Add/Remove Programs for all BlackSpiritHelper products.
        /// </summary>
        private void SetInstallerIcon()
        {
            try
            {
                // Icon path.
                string iconSourcePath = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Resources\Images\Logo\icon_red.ico");

                if (!File.Exists(iconSourcePath))
                    return;

                RegistryKey myUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                string[] mySubKeyNames = myUninstallKey.GetSubKeyNames();
                for (int i = 0; i < mySubKeyNames.Length; i++)
                {
                    RegistryKey myKey = myUninstallKey.OpenSubKey(mySubKeyNames[i], true);
                    object myValue = myKey.GetValue("DisplayName");
                    if (
                        myValue != null && myValue.ToString() == ((AssemblyTitleAttribute)IoC.Application.ApplicationExecutingAssembly.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title
                        )
                    {
                        myKey.SetValue("DisplayIcon", iconSourcePath);
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Compile arguments into Dictionary.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private Dictionary<string, string> CompileArguments(string[] args)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            if (args.Length == 0)
                return d;

            string[] sParts;
            foreach (string s in args)
            {
                sParts = s.Split('=');

                // We want only named arguments.
                if (sParts.Length <= 1)
                    continue;

                d.Add(
                    sParts[0].Trim(), 
                    sParts[1].Trim('"')
                    );
            }

            return d;
        }

        #endregion
    }
}
