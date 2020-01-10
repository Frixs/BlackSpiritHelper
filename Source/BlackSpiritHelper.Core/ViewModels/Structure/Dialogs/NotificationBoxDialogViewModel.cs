using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Represents notification context of notification UI.
    /// List of all notifications and calling notifications can be found in <see cref="IoC.UI"/>
    /// </summary>
    public class NotificationBoxDialogViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Notification Title
        /// </summary>
        public string Title { get; set; } = "NOTIFICATION";

        /// <summary>
        /// Message context
        /// </summary>
        public string Message { get; set; } = "Message placeholder!";

        /// <summary>
        /// Type of the button layout
        /// </summary>
        public NotificationBoxResult Result { get; set; } = NotificationBoxResult.Ok;

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.Ok"/> successful result from notification box.
        /// </summary>
        public Action OkAction { get; set; } = delegate { };

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.YesNo"/> positive - Yes - result from notification box.
        /// </summary>
        public Action YesAction { get; set; } = delegate { };

        /// <summary>
        /// Run this action on <see cref="NotificationBoxResult.YesNo"/> negative - No - result from notification box.
        /// </summary>
        public Action NoAction { get; set; } = delegate { };

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
            YesCommand = new RelayCommand(async () => await YesCommandMethodAsync());
            NoCommand = new RelayCommand(async () => await NoCommandMethodAsync());
        }

        /// <summary>
        /// NO command routine
        /// </summary>
        /// <returns></returns>
        private async Task NoCommandMethodAsync()
        {
            NoAction();

            Remove();

            await Task.Delay(1);
        }

        /// <summary>
        /// YES command routine
        /// </summary>
        /// <returns></returns>
        private async Task YesCommandMethodAsync()
        {
            YesAction();

            Remove();

            await Task.Delay(1);
        }

        /// <summary>
        /// OK command routine
        /// </summary>
        /// <returns></returns>
        private async Task OkCommandMethodAsync()
        {
            OkAction();

            Remove();

            await Task.Delay(1);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remove this instance of notification box from the list of notifications.
        /// </summary>
        private void Remove()
        {
            IoC.UI.NotificationArea.RemoveNotification(this);
        }

        #endregion
    }
}
