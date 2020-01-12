using BlackSpiritHelper.Core;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Deployment.Application;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// ---
    /// TODO:LATER: Replace Border for Separator TAG
    /// TODO:LATER:APP: more...
    ///     - Application user option to download/check updates.
    ///     - Auto manage length of log file. Cut the file if it is too large.
    ///     - DataViewModel extends DataModel - Move business logic into Model with its fields.
    ///       Separate ViewModel's command logic from Model's business logic.
    ///     - IDisposable can be useful in some situation for destroying Timer instances in sections.
    ///       External links: https://stackoverflow.com/questions/188688/what-does-the-tilde-before-a-function-name-mean-in-c
    ///                       https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
    /// </summary>
    public partial class App : Application
    {
        #region Private Members

        /// <summary>
        /// Says if the application is going to restart in order to achieve administrator privileges.
        /// </summary>
        private bool mIsRestartingProcessFlag = false;

        /// <summary>
        /// Early error list is here to catch all error simple messages that we need to track while <see cref="IoC.Logger"/> is not available yet.
        /// </summary>
        private List<string> mEarlyErrorList = null;

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
                IoC.Logger.Log($"An unhandled exception occurred: ({e.GetType().ToString()}) {e.Exception.Message}", LogLevel.Fatal);
            }
            MessageBox.Show($"An unhandled exception just occurred: {e.Exception.Message}.{Environment.NewLine}Please, contact the developers to fix the issue.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Warning);

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

            // Initialize early error list.
            InitEarlyErrorList();

            // Configuration setup.
            ApplicationPropertySetup(CompileArguments(e.Args));

            // Setup on application deployment.
            OnDeploymentSetup();

            // Check for administrator privileges.
            if (IoC.Application.Cookies.ForceToRunAsAdministrator
                && !IoC.Application.IsRunningAsAdministratorCheck
                && !Debugger.IsAttached
                )
            {
                if (RunAsAdministrator())
                    return;
            }

            // Configuration module setup.
            ApplicationModuleSetup();

            // Process early error list. We already have access to IoC.
            ProcessEarlyErrorList();

            // Set application main window.
            Current.MainWindow = new MainWindow();

            // Set MainWindow size to the size of last opening.
            IoC.UI.SetMainWindowSize(IoC.Application.Cookies.MainWindowSize);

            // Run application with pre-start process.
            IoC.Task.Run(async () =>
            {
                // Setup on application update.
                bool bNewUpdate = await OnUpdateSetupAsync();

                // Log it.
                IoC.Logger.Log("Application starting up" + (IoC.Application.IsRunningAsAdministratorCheck ? " (As Administrator)" : "") + "...", LogLevel.Info);

                // Show the main window.
                await IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
                {
                    // Open MainWindow.
                    IoC.UI.ShowMainWindow();

                    // Welcome message
                    if (!IoC.Application.Cookies.AlreadyFirstlyLaunched)
                        IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                        {
                            Title = "WELCOME!",
                            MessageFormatting = true,
                            Message = "## Feedback",
                            Result = NotificationBoxResult.Ok,
                            OkAction = () =>
                            {
                                IoC.Application.Cookies.AlreadyFirstlyLaunched = true;
                            },
                        });

                    // News.
                    IoC.UI.ShowNews(true);

                    // Patch Notes.
                    if (bNewUpdate)
                        IoC.UI.ShowPatchNotes();

                    // Start in tray?
                    if (IoC.DataContent.PreferencesData.StartInTray)
                    {
                        IoC.UI.CloseMainWindowToTray();
                    }

                    // Start overlay?
                    if (IoC.DataContent.OverlayData.OpenOnStart)
                    {
                        IoC.UI.OpenOverlay();
                    }
                }));
            });
        }

        /// <summary>
        /// Perform tasks at application exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Do nothing if the application is going to restart immediately after start.
            if (mIsRestartingProcessFlag)
                return;

            // All application windows are already closed here.
            #region Dispose Here

            // Dispose tray icon.
            WindowViewModel.DisposeTrayIcon();

            // Clean log files.
            IoC.Logger.CleanLogFiles();

            // Dispose IoC modules
            IoC.Web.Dispose();
            IoC.Get<IMouseKeyHook>().Dispose();

            // "Prepare data to die."
            IoC.DataContent.Unset();

            #endregion
        }

        #endregion

        #region Private Methods: As Administrator

        /// <summary>
        /// Run the application As Administrator.
        /// </summary>
        /// <returns>If true, applicationwil restart to administrator mode. The return is just for making sure and possibility to stop the code process.</returns>
        private bool RunAsAdministrator()
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
            catch (Exception ex)
            {
                // The user did not allow the application to run as administrator.
                MessageBox.Show($"Sorry, {Assembly.GetExecutingAssembly().GetName().Name} must be run As Administrator in order to interact with the overlay while you are playing your game.{Environment.NewLine}Your computer is not allowing to start the application As Administrator.");
                AddEarlyError("Process start error: " + ex.Message);
                return false;
            }

            // Shut down the current process.
            Current.Shutdown();

            return true;
        }

        #endregion

        #region Private Methods: Setup Application

        /// <summary>
        /// Configure the basics for our application.
        /// </summary>
        /// <param name="args"></param>
        private void ApplicationPropertySetup(Dictionary<string, string> args)
        {
            // Setup IoC.
            IoC.Setup(args);

            #region Set Application Properties

            // App assembly.
            IoC.Application.AppAssembly = new AppAssembly();

            // Bind Executing Assembly.
            IoC.Application.ExecutingAssembly = Assembly.GetExecutingAssembly();

            // Bind AssemblyInfo version.
            IoC.Application.ApplicationVersion = ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : (args.ContainsKey("Version") ? args["Version"] : FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion);

            // Bind AssemblyInfo copyright.
            IoC.Application.Copyright = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;

            #endregion
        }

        /// <summary>
        /// Configures module departments of our application.
        /// <see cref="ApplicationPropertySetup(Dictionary{string, string})"/> must be called first.
        /// </summary>
        private void ApplicationModuleSetup()
        {
            // Bind Logger.
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new[]
            {
                new FileLogger(
                    ((AssemblyTitleAttribute)IoC.Application.ExecutingAssembly.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title.Replace(' ', '_').ToLower() + "_log.log"
                    ),
            })
            {
                LogOutputLevel = Debugger.IsAttached ? LogOutputLevel.Debug : LogOutputLevel.Verbose
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

            // Bind an web manager.
            IoC.Kernel.Bind<IWebManager>().ToConstant(new WebManager());

            // Bind an mouse key hooks.
            IoC.Kernel.Bind<IMouseKeyHook>().ToConstant(new GlobalMouseKeyHookManager());

            // Bind Application data content view models.
            IoC.Kernel.Bind<ApplicationDataContent>().ToConstant(new ApplicationDataContent());
            IoC.DataContent.Setup();
        }

        #endregion

        #region Private Methods: Setup Deployment

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
        /// <returns>Was the update fired?</returns>
        private async Task<bool> OnUpdateSetupAsync()
        {
            if (Debugger.IsAttached)
                return false;

            int userDelayMs = 500;
            bool procedureFailure = false;

            // File relative to execution directory.
            string filePath = "Version.check/" + IoC.Application.ApplicationVersion.Replace('.', '_');

            // To prevent possible exception.
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // If the file exists, do nothing.
            if (File.Exists(filePath))
                return false;

            // if the file does not exist, we need to run on update procedure.
            IoC.Logger.Log("Starting update procedure...", LogLevel.Info);

            #region Procedure

            // Create default process dialog model.
            ProgressDialogViewModel progressData = new ProgressDialogViewModel
            {
                Title = "CHECKING FOR UPDATES",
                Subtitle = "",
                WorkOn = "",
            };

            // Open progress window.
            await IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                IoC.UI.OpenProgressWindow(progressData);
            }));

            // Delay to inform user about upcoming procedure.
            await Task.Delay(userDelayMs);

            // Start procedure with updating progress title.
            progressData.Title = "UPDATING...";

            // Update application data.
            progressData.Subtitle = "Application data";
            if (!DataProvider.Instance.DownloadData(SettingsConfiguration.RemoteDataDirPath, progressData))
                procedureFailure = true;

            #endregion

            // On procedure finish.
            progressData.Subtitle = "";
            progressData.WorkOn = procedureFailure ? "Unable to update!" : "Done!";
            await Task.Delay(userDelayMs);
            progressData.Title = "STARTING UP";
            await Task.Delay(userDelayMs * 3);

            // Close progress window.
            await IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                IoC.UI.CloseProgressWindow();
            }));

            // If the procedure successfully finished.
            if (!procedureFailure)
            {
                // Create a new check file of the current version.
                File.Create(filePath).Dispose();

                IoC.Logger.Log("Update successfully finished!", LogLevel.Info);

                // Restart application.
                IoC.Application.AppAssembly.Restart();
            }
            // Otherwise do if the updating failed.
            else
            {
                IoC.Logger.Log("Unable to update!", LogLevel.Warning);
            }

            return true;
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
                        myValue != null && myValue.ToString() == ((AssemblyTitleAttribute)IoC.Application.ExecutingAssembly.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title
                        )
                    {
                        myKey.SetValue("DisplayIcon", iconSourcePath);
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion

        #region Private Helpers

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

        /// <summary>
        /// Initialize <see cref="mEarlyErrorList"/>.
        /// </summary>
        private void InitEarlyErrorList()
        {
            mEarlyErrorList = new List<string>();
        }

        /// <summary>
        /// Add new early error message to <see cref="mEarlyErrorList"/>.
        /// </summary>
        /// <param name="errorMessage"></param>
        private void AddEarlyError(string errorMessage)
        {
            if (mEarlyErrorList == null)
                return;

            mEarlyErrorList.Add(new StackTrace().GetFrame(1).GetMethod().Name + "(): " + errorMessage);
        }

        /// <summary>
        /// Process <see cref="mEarlyErrorList"/>.
        /// </summary>
        private void ProcessEarlyErrorList()
        {
            for (int i = 0; i < mEarlyErrorList.Count; i++)
            {
                IoC.Logger.Log(mEarlyErrorList[i], LogLevel.Error);
            }
            mEarlyErrorList = null;
        }

        #endregion
    }
}
