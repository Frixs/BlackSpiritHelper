using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer group.
    /// </summary>
    public class TimerGroupViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// ID of the group.
        /// </summary>
        public byte ID { get; set; }

        /// <summary>
        /// Group Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Button control - says if any of the child timers are active.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Max number of timers that can be created in the group.
        /// </summary>
        public byte MaxNoOfTimers { get; private set; } = 10;

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        public bool CanCreateNewTimer { get; private set; }

        /// <summary>
        /// List of timers in the group.
        /// </summary>
        public ObservableCollection<TimerItemViewModel> TimerList { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command play or pause group.
        /// </summary>
        public ICommand PlayPauseCommand { get; set; }

        #endregion

        #region Constructor

        public TimerGroupViewModel()
        {
            TimerList = new ObservableCollection<TimerItemViewModel>();

            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            PlayPauseCommand = new RelayCommand(async () => await PlayPauseAsync());
        }

        /// <summary>
        /// Command helper, open page async.
        /// </summary>
        /// <returns></returns>
        private async Task PlayPauseAsync()
        {
            Title = "Hey";
            System.Console.WriteLine("TRIGGER");

            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add timer item to specific group..
        /// </summary>
        /// <param name="item">The item.</param>
        public bool AddTimer(TimerItemViewModel item)
        {
            IoC.Logger.Log($"Trying to add Timer '{item.Title}' to group '{Title}'...", LogLevel.Debug);

            if (item == null)
                return false;

            // Check limits.
            if (TimerList.Count + 1 > MaxNoOfTimers)
            {
                CanCreateNewTimer = false;
                return false;
            }

            TimerList.Add(item);

            IoC.Logger.Log($"Timer '{item.Title}' added to group '{Title}'!", LogLevel.Info);
            return true;
        }

        #endregion
    }
}
