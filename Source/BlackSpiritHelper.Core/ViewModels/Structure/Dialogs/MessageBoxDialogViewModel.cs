using System;
using System.Windows;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Details for a message box dialog.
    /// </summary>
    public class MessageBoxDialogViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The title of the message box.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Buttons type.
        /// </summary>
        public MessageBoxButton Button { get; set; }

        /// <summary>
        /// Icon of the message box.
        /// </summary>
        public MessageBoxImage Icon { get; set; }

        /// <summary>
        /// Run this action on <see cref="MessageBoxResult.Yes"/> successful result from messagebox.
        /// </summary>
        public Action YesAction { get; set; } = delegate { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessageBoxDialogViewModel()
        {
        }

        #endregion
    }
}
