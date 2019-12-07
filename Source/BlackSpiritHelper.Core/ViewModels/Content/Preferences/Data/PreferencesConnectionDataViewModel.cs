using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class PreferencesConnectionDataViewModel : ASetupableBaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Timer for pending messages.
        /// </summary>
        private Timer mPendingTimer = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says if the application has set active connection or not.
        /// </summary>
        [XmlIgnore]
        public bool IsActive => ActiveMethod == null ? false : true;

        /// <summary>
        /// Active (activated) method's identifier - <see cref="APreferencesConnBaseDataViewModel.Identifier"/>.
        /// Servers loading/saving user data, primarily.
        /// Is synced with value changing of <see cref="ActiveMethod"/>.
        /// </summary>
        public PreferencesConnectionType ActiveMethodIdentifier { get; set; } = PreferencesConnectionType.None;

        /// <summary>
        /// Active (activated) method that user uses.
        /// </summary>
        [XmlIgnore]
        public APreferencesConnBaseDataViewModel ActiveMethod { get; set; } = null;

        /// <summary>
        /// List of all available connections for user.
        /// Use <seealso cref="Init"/> for initialization.
        /// </summary>
        [XmlIgnore]
        public List<APreferencesConnBaseDataViewModel> MethodList { get; set; } //; Init - Constructor

        #endregion

        #region Public Properties (Methods)

        /// <summary>
        /// Connection method: Discord
        /// Use <seealso cref="Init"/> for initialization.
        /// </summary>
        public PreferencesConnDiscordDataViewModel MethodDiscord { get; set; } = new PreferencesConnDiscordDataViewModel();

        #endregion

        #region Commands

        /// <summary>
        /// Command to activate connection.
        /// </summary>
        [XmlIgnore]
        public ICommand DeactivateCommand { get; set; }

        /// <summary>
        /// Command to go to guide URL.
        /// </summary>
        [XmlIgnore]
        public ICommand GuideLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PreferencesConnectionDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        protected override void InitRoutine(params object[] parameters)
        {
            // Build list of all methods.
            MethodList = new List<APreferencesConnBaseDataViewModel>()
            {
                MethodDiscord,
            };

            // Activate user preferred method.
            ActivateMethod(ActiveMethodIdentifier);

            // Handle pending messages.
            StartTimerControl();
        }

        protected override void DisposeRoutine()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            DeactivateCommand = new RelayCommand(async () => await DeactivateCommandMethodAsync());
            GuideLinkCommand = new RelayCommand(async () => await GuideLinkCommandMethodAsync());
        }

        /// <summary>
        /// Deactivate connection method async wrapper.
        /// </summary>
        /// <returns></returns>
        private async Task DeactivateCommandMethodAsync()
        {
            DeactivateMethod();
            await Task.Delay(1);
        }

        /// <summary>
        /// Open guide link.
        /// </summary>
        /// <returns></returns>
        private async Task GuideLinkCommandMethodAsync()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start("https://github.com/Frixs/BlackSpiritHelper/wiki/PreferencesConnection");

            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activate new connection method as the one which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void ActivateMethod(PreferencesConnectionType identifier)
        {
            ActiveMethod = MethodList.FirstOrDefault(o => o.Identifier.Equals(identifier));
            ActiveMethodIdentifier = ActiveMethod == null ? PreferencesConnectionType.None : ActiveMethod.Identifier;
            OnPropertyChanged(nameof(IsActive));
        }

        /// <summary>
        /// Deactivate connection method which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void DeactivateMethod()
        {
            ActiveMethod = null;
            ActiveMethodIdentifier = PreferencesConnectionType.None;
            OnPropertyChanged(nameof(IsActive));
        }

        /// <summary>
        /// Add new pending message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddNewPendingMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            IoC.Application.Cookies.PendingMessageList.Add(message);

            StartTimerControl();
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set the timers.
        /// </summary>
        private void StartTimerControl()
        {
            if (!IoC.Application.Cookies.PendingMessageList.Any() || mPendingTimer != null)
                return;

            // Set pending timer.
            mPendingTimer = new Timer(TimeSpan.FromMinutes(10).TotalMilliseconds);
            mPendingTimer.Elapsed += PendingTimerOnElapsed;
            mPendingTimer.AutoReset = false; // Make it fire only once -> Manual firing.
            mPendingTimer.Start();

            IoC.Logger.Log("Starting timer for pending messages.", LogLevel.Debug);
        }

        /// <summary>
        /// Dispose timer calculations.
        /// Use this only while destroying the instance of the timer.
        /// </summary>
        private void StopTimerControl()
        {
            if (mPendingTimer == null)
                return;

            // Pending timer.
            mPendingTimer.Stop();
            mPendingTimer.Elapsed -= PendingTimerOnElapsed;
            mPendingTimer.Dispose();
            mPendingTimer = null;

            IoC.Logger.Log("Stopping timer for pending messages.", LogLevel.Debug);
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PendingTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var list = IoC.Application.Cookies.PendingMessageList;
            List<string> itemsToRemove = new List<string>();
            
            // Go through pending messages.
            for (int i = 0; i < list.Count; i++)
            {
                if (IsActive)
                {
                    if (ActiveMethod.SendTextMessage(list[i]) == 0)
                        itemsToRemove.Add(list[i]);
                }
                else // No point to check messages. We dont have set active connection.
                {
                    list.Clear();
                }
            }

            // Removation.
            list = list.Except(itemsToRemove).ToList();

            // Check if the list has any pending messages. If it does, start next cycle.
            if (list.Any())
                mPendingTimer.Start();
            else
                StopTimerControl();
        }

        #endregion
    }
}
