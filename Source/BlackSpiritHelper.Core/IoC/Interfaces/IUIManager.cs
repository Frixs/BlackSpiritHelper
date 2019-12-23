using System;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The UI manager that handles any UI interaction in the application.
    /// </summary>
    public interface IUIManager
    {
        #region Dialog Windows

        /// <summary>
        /// DIsplays a single message box to the user.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        Task ShowMessage(MessageBoxDialogViewModel viewModel);

        /// <summary>
        /// Open file browser dialog.
        /// </summary>
        /// <param name="action">Action on success/select browser item.</param>
        /// <returns></returns>
        Task ShowFolderBrowserDialog(Action<string> action);

        /// <summary>
        /// Open file browser dialog.
        /// </summary>
        /// <param name="action">Action on success/select browser item.</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task ShowFileBrowserDialog(Action<string> action, string filter);

        #endregion

        #region Overlay Window

        /// <summary>
        /// Open overlay window.
        /// </summary>
        void OpenOverlay();

        /// <summary>
        /// Close overlay window.
        /// </summary>
        void CloseOverlay();

        #endregion
        
        #region Progress Window

        /// <summary>
        /// Open progress window.
        /// </summary>
        /// <param name="viewModel"></param>
        void OpenProgressWindow(ProgressDialogViewModel viewModel = null);

        /// <summary>
        /// Close progress window.
        /// </summary>
        void CloseProgressWindow();

        #endregion

        #region Application MainWindow

        /// <summary>
        /// Get Application's MainWindow size.
        /// </summary>
        Vector2Double GetMainWindowSize();

        /// <summary>
        /// Set Application's MainWindow size.
        /// </summary>
        /// <param name="size"></param>
        void SetMainWindowSize(Vector2Double size);

        /// <summary>
        /// Show MainWindow.
        /// </summary>
        void ShowMainWindow();

        /// <summary>
        /// Close MainWindow to Windows tray.
        /// </summary>
        void CloseMainWindowToTray();

        #endregion
    }
}
