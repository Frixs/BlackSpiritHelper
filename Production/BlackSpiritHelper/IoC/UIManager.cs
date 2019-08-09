using System;
using System.Threading.Tasks;
using System.Windows;
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
    }
}
