using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using BlackSpiritHelper.Core;

namespace BlackSpiritHelper
{
    /// <summary>
    /// The applications implementation of the <see cref="IUIManager"/>.
    /// </summary>
    public class UIManager : IUIManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UIManager()
        {
        }

        #endregion

        /// <summary>
        /// DIsplays a single message box to the user.
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
        /// Open file browser dialog.
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
        /// Open overlay window.
        /// </summary>
        public void OpenOverlay()
        {
            if (OverlayWindow.Window != null)
                return;

            IoC.DataContent.OverlayDesignModel.IsOpened = true;

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

            IoC.DataContent.OverlayDesignModel.IsOpened = false;

            OverlayWindow.Window.Close();
            OverlayWindow.Window = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

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
    }
}
