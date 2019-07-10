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
            return IoC.Task.Run(() => MessageBox.Show(viewModel.Message, viewModel.Caption, viewModel.Button, viewModel.Icon));
        }
    }
}
