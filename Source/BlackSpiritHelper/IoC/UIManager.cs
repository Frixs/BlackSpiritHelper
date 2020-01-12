using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using BlackSpiritHelper.Core;
using Microsoft.Win32;

namespace BlackSpiritHelper
{
    /// <summary>
    /// The applications implementation of the <see cref="IUIManager"/>.
    /// </summary>
    public class UIManager : IUIManager
    {
        #region Public Properties

        /// <summary>
        /// Notification area contains and serves detailed events about notifications.
        /// You can open a new notification by calling method here <see cref="ShowNotification(NotificationBoxDialogViewModel)"/>
        /// </summary>
        public NotificationAreaDialogViewModel NotificationArea { get; } = new NotificationAreaDialogViewModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UIManager()
        {
        }

        #endregion

        #region Dialog Windows

        /// <summary>
        /// Show a notification to a user.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public Task ShowNotification(NotificationBoxDialogViewModel viewModel)
        {
            return IoC.Task.Run(() =>
            {
                NotificationArea.AddNotification(viewModel);
            });
        }

        /// <summary>
        /// Show a notification containing patch notes to a user.
        /// </summary>
        /// <returns></returns>
        public Task ShowPatchNotes()
        {
            return IoC.Task.Run(async () =>
            {
                // Get client.
                var client = IoC.Web.Http.GetClientForHost(new Uri(SettingsConfiguration.RemotePatchNotesFilePath));
                string message = string.Empty;
                bool isOk = false;

                // Get data.
                try
                {
                    // Read data.
                    message = await client.GetStringAsync(SettingsConfiguration.RemotePatchNotesFilePath);
                    IoC.Logger.Log($"Patch Notes has been read successfully!", LogLevel.Debug);
                    isOk = true;
                }
                catch (HttpRequestException e) // Internet connection issues.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Verbose);
                }
                catch (TaskCanceledException e) // Timeout.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Debug);
                }
                catch (Exception e) // Unexpected.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message}", LogLevel.Fatal);
                }

                // We have data OK
                if (isOk)
                {
                    // Remove first line of the message
                    message = message.RemoveFirstLines(1);

                    // Create notification view model
                    var vm = new NotificationBoxDialogViewModel()
                    {
                        Title = "PATCH NOTES",
                        MessageFormatting = true,
                        Message = message,
                        Result = NotificationBoxResult.Ok,
                    };

                    // Create notification
                    NotificationArea.AddNotification(vm);
                }
            });
        }

        /// <summary>
        /// Show a notification containing news to a user.
        /// </summary>
        /// <param name="onlyWhenNew">True: shows patch notes only when the first line of news file which represents latest news, is newer.</param>
        /// <returns></returns>
        public Task ShowNews(bool onlyWhenNew)
        {
            return IoC.Task.Run(async () =>
            {
                // Get client.
                var client = IoC.Web.Http.GetClientForHost(new Uri(SettingsConfiguration.RemoteNewsFilePath));
                string message = string.Empty;
                bool isOk = false;

                // Get data.
                try
                {
                    // Read data.
                    message = await client.GetStringAsync(SettingsConfiguration.RemoteNewsFilePath);
                    IoC.Logger.Log($"News has been read successfully!", LogLevel.Debug);
                    isOk = true;
                }
                catch (HttpRequestException e) // Internet connection issues.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Verbose);
                }
                catch (TaskCanceledException e) // Timeout.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Debug);
                }
                catch (Exception e) // Unexpected.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message}", LogLevel.Fatal);
                }

                // We have data OK
                if (isOk)
                {
                    // Get first line of the message = date of latest news
                    DateTime date;
                    DateTime.TryParseExact(
                        message.GetFirstLines(1).Replace(@"<!--", "").Replace(@"-->", "").Replace(" ", ""),
                        "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    
                    // Compare user's date of last news show with the latest news date.
                    // To show the notification, the remote date has to be greater than the one saved in the cookies and the saved one cannot be equal to today's date.
                    if (!onlyWhenNew 
                        || (DateTime.Compare(IoC.Application.Cookies.NewsLatestReviewDate, date) < 0
                            && DateTime.Compare(IoC.Application.Cookies.NewsLatestReviewDate, DateTime.Today) != 0)
                            )
                    {
                        // If user download the app and the news are not up-to-date. Don!t show them and update default user's review time to today's date.
                        if (DateTime.Compare(date, DateTime.Today) < 0)
                        {
                            IoC.Application.Cookies.NewsLatestReviewDate = DateTime.Today;
                        }
                        // Otherwise, show notification.
                        else
                        {
                            // Create notification view model
                            var vm = new NotificationBoxDialogViewModel()
                            {
                                Title = "NEWS",
                                MessageFormatting = true,
                                Message = message,
                                Result = NotificationBoxResult.Ok,
                                OkAction = () =>
                                {
                                    IoC.Application.Cookies.NewsLatestReviewDate = DateTime.Today;
                                },
                            };

                            // Create notification
                            NotificationArea.AddNotification(vm);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Displays a single message box to the user.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public Task ShowMessage(MessageBoxDialogViewModel viewModel)
        {
            return IoC.Task.Run(() =>
            {
                MessageBoxResult res = MessageBox.Show(viewModel.Message, viewModel.Caption, viewModel.Button, viewModel.Icon);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        viewModel.YesAction();
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// Open folder browser dialog.
        /// </summary>
        /// <param name="action">Action on success/select browser item.</param>
        /// <returns></returns>
        public Task ShowFolderBrowserDialog(Action<string> action)
        {
            return IoC.Task.Run(() =>
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    // UI thread required.
                    IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
                    {
                        System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            action(fbd.SelectedPath);
                        }
                    }));

                }
            });
        }

        /// <summary>
        /// Open file browser dialog.
        /// </summary>
        /// <param name="action">Action on success/select browser item.</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task ShowFileBrowserDialog(Action<string> action, string filter)
        {
            return IoC.Task.Run(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = filter;
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == true)
                {
                    action(openFileDialog.FileName);
                }
            });
        }

        #endregion

        #region Overlay Window

        /// <summary>
        /// Open overlay window.
        /// </summary>
        public void OpenOverlay()
        {
            if (OverlayWindow.Window != null)
                return;

            IoC.DataContent.OverlayData.IsOpened = true;

            OverlayWindow.Window = new OverlayWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            OverlayWindow.Window.Show();
        }

        /// <summary>
        /// Close overlay window.
        /// </summary>
        public void CloseOverlay()
        {
            if (OverlayWindow.Window == null)
                return;

            IoC.DataContent.OverlayData.IsOpened = false;

            OverlayWindow.Window.Close();
            OverlayWindow.Window = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region Progress Window

        /// <summary>
        /// Open progress window.
        /// </summary>
        /// <param name="viewModel"></param>
        public void OpenProgressWindow(ProgressDialogViewModel viewModel = null)
        {
            if (ProgressWindow.Window != null)
                return;

            ProgressWindow.Window = new ProgressWindow();

            if (viewModel != null)
                ((ProgressWindowViewModel)ProgressWindow.Window.DataContext).VM = viewModel;

            ProgressWindow.Window.Show();
        }

        /// <summary>
        /// Close progress window.
        /// </summary>
        public void CloseProgressWindow()
        {
            if (ProgressWindow.Window == null)
                return;

            ProgressWindow.Window.Close();
            ProgressWindow.Window = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region Application MainWindow

        /// <summary>
        /// Get Application's MainWindow size.
        /// </summary>
        public Vector2Double GetMainWindowSize()
        {
            return new Vector2Double(Application.Current.MainWindow.Width, Application.Current.MainWindow.Height);
        }

        /// <summary>
        /// Set Application's MainWindow size.
        /// </summary>
        /// <param name="size"></param>
        public void SetMainWindowSize(Vector2Double size)
        {
            Application.Current.MainWindow.Width = size.X;
            Application.Current.MainWindow.Height = size.Y;
        }

        /// <summary>
        /// Show MainWindow.
        /// </summary>
        public void ShowMainWindow()
        {
            // Activate window if it is visible in background.
            if (Application.Current.MainWindow.IsVisible)
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                    Application.Current.MainWindow.WindowState = WindowState.Normal;

                Application.Current.MainWindow.Activate();
            }
            // Open window if it is closed in tray.
            else
            {
                Application.Current.MainWindow.Show();
                WindowViewModel.DisposeTrayIcon();
            }
        }

        /// <summary>
        /// Close MainWindow to Windows tray.
        /// </summary>
        public void CloseMainWindowToTray()
        {
            // Create notification tray icon.
            System.Windows.Forms.NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
            trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon("Resources/Images/Logo/icon_white.ico");
            trayIcon.DoubleClick += (s, args) => ShowMainWindow();
            trayIcon.Visible = true;
            // Create context menu for the notification icon.
            trayIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add("Open Application").Click += (s, e) => ShowMainWindow();
            trayIcon.ContextMenuStrip.Items.Add("Donate").Click += (s, e) => System.Diagnostics.Process.Start(IoC.Application.DonationURL);
            trayIcon.ContextMenuStrip.Items.Add("-");
            trayIcon.ContextMenuStrip.Items.Add("Quit").Click += (s, e) => IoC.Application.Exit();

            // Try to dispose previous ion if exists.
            WindowViewModel.DisposeTrayIcon();
            // Assign tray icon.
            WindowViewModel.TrayIcon = trayIcon;

            // Hide MainWindow.
            Application.Current.MainWindow.Hide(); // A hidden window can be shown again, a closed one not.
        }

        #endregion
    }
}
