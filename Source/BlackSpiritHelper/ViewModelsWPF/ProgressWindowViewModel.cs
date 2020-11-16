using BlackSpiritHelper.Core;
using System.Windows;

namespace BlackSpiritHelper
{
    public class ProgressWindowViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The window his view model controls.
        /// </summary>
        private Window mWindow;

        #endregion

        #region Public Properties

        /// <summary>
        /// Data view model.
        /// </summary>
        public ProgressDialogViewModel VM { get; set; } = new ProgressDialogViewModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProgressWindowViewModel(Window window)
        {
            mWindow = window;
        }

        #endregion
    }
}
