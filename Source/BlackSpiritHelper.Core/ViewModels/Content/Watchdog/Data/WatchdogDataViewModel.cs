using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Watchdog section data ROOT.
    /// </summary>
    public class WatchdogDataViewModel : DataContentBaseViewModel<WatchdogDataViewModel>
    {
        #region Private Fields

        /// <summary>
        /// Number of max log messages to be displayed.
        /// </summary>
        private int mMaxLogMessages = 100;

        #endregion

        #region Public Properties

        /// <summary>
        /// Handles connection checks - whole subsection of Watchdog.
        /// </summary>
        public WatchdogConnectionWatcherDataViewModel ConnectionWatcher { get; set; } = new WatchdogConnectionWatcherDataViewModel();

        /// <summary>
        /// List of log messages.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<string> LogList { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// Says, if watchdog section is running.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning
        {
            get => ConnectionWatcher.IsRunning ? true : false;
            protected set => throw new NotImplementedException();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command to clear log.
        /// </summary>
        [XmlIgnore]
        public ICommand ClearLogCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogDataViewModel()
        {
            // Create commands
            CreateCommands();
        }

        protected override void SetDefaultsMethod()
        {
        }

        protected override void SetupMethod()
        {
            // Set the timers.
            ConnectionWatcher.SetTimerControl();
        }

        protected override void UnsetMethod()
        {
            ConnectionWatcher.DisposeTimerControl();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            ClearLogCommand = new RelayCommand(async () => await ClearLogAsync());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new message to the log.
        /// The method also keeps the list in proper size.
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            var datetime = DateTimeOffset.UtcNow.ToString("MM-dd HH:mm UTC");

            // UI thread required - due to ObservableCollection.
            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // Keep the list with proper size.
                if (LogList.Count + 1 > mMaxLogMessages)
                    LogList.RemoveAt(0);

                // Add new log message.
                LogList.Add($"[{datetime}] {message}");
            }));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clear all the list of logs.
        /// </summary>
        /// <returns></returns>
        private async Task ClearLogAsync()
        {
            // UI thread required - due to ObservableCollection.
            await IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                LogList.Clear();
            }));
        }

        #endregion
    }
}
