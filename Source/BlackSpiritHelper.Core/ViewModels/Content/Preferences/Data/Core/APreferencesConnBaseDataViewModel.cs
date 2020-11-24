using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class APreferencesConnBaseDataViewModel : BaseViewModel, IPreferencesConnectionMethods
    {
        #region Protected Members

        /// <summary>
        /// Timeout for sending message.
        /// Unit: Milliseconds
        /// </summary>
        protected int mSendingTimeout = 10000;

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifier representing particular connection.
        /// </summary>
        [XmlIgnore]
        public abstract PreferencesConnectionType Identifier { get; protected set; }

        #endregion

        #region Command Flags

        private bool mActivateCommandFlag { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to activate connection.
        /// </summary>
        [XmlIgnore]
        public ICommand ActivateCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public APreferencesConnBaseDataViewModel()
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
            ActivateCommand = new RelayCommand(async () => await ActivateCommandMethodAsync());
        }

        /// <summary>
        /// Activate connection method async wrapper.
        /// </summary>
        /// <returns></returns>
        private async Task ActivateCommandMethodAsync()
        {
            await RunCommandAsync(() => mActivateCommandFlag, async () =>
            {
                ActivateMethod();
                await Task.Delay(1);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send message to the user's connection.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        /// Status code:
        ///     - 0 = OK
        ///     - 1 = Unexpected error occurred - no internet connection
        ///     - 2 = Not set active connection - No Check in this method !!!!! - This is subclass of this manager <see cref="PreferencesConnectionDataViewModel"/>
        /// </returns>
        public abstract int SendTextMessage(string message);

        /// <summary>
        /// Async version of <see cref="SendTextMessage(string)"/>.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        /// Status code:
        ///     - 0 = OK
        ///     - 1 = Unexpected error occurred - no internet connection
        ///     - 2 = Not set active connection - No Check in this method !!!!! - This is subclass of this manager <see cref="PreferencesConnectionDataViewModel"/>
        /// </returns>
        public abstract Task<int> SendTextMessageAsync(string message);

        /// <summary>
        /// Validate inputs of connection method.
        /// </summary>
        /// <returns></returns>
        public abstract bool ValidateInputs();

        /// <summary>
        /// Activate method as the one ehich user uses.
        /// </summary>
        /// <returns></returns>
        public void ActivateMethod()
        {
            if (!ValidateInputs())
            {
                // Some error occured during validation.
                IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                {
                    Title = "INVALID VALUES",
                    Message = $"Some of the entered values are invalid.",
                    Result = NotificationBoxResult.Ok,
                });

                return;
            }

            // Activate.
            IoC.DataContent.PreferencesData.Connection.ActivateMethod(Identifier);
        }

        #endregion
    }
}
