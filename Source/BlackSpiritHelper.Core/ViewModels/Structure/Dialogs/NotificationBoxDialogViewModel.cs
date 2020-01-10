using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Represents notification context of notification UI.
    /// List of all notifications and calling notifications can be found in <see cref="IoC.UI"/>
    /// </summary>
    public class NotificationBoxDialogViewModel
    {
        #region Public Properties

        /// <summary>
        /// TODO
        /// </summary>
        public string Title { get; set; } = "NOTIFICATION";

        /// <summary>
        /// TODO
        /// </summary>
        public string Message { get; set; } = "Message placeholder!";

        /// <summary>
        /// TODO
        /// </summary>
        public NotificationBoxResult Result { get; set; } = NotificationBoxResult.Ok;

        #endregion

        #region Commands

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand YesCommand { get; set; }

        /// <summary>
        /// Command to run OK event.
        /// </summary>
        public ICommand NoCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NotificationBoxDialogViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            OkCommand = new RelayCommand(async () => await OkCommandMethodAsync());
            YesCommand = new RelayCommand(async () => await OkCommandMethodAsync());
            NoCommand = new RelayCommand(async () => await OkCommandMethodAsync());
        }

        private Task OkCommandMethodAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
