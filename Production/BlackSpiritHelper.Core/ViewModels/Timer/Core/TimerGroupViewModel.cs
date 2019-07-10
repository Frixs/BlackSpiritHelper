using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer Group. ViewModel for TimerGroupMenuItemControl.
    /// List of all groups is in the view model <see cref="TimerGroupListViewModel"/>.
    /// </summary>
    public class TimerGroupViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Limitation for min characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMinChar { get; private set; } = 3;

        /// <summary>
        /// Limitation for max characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMaxChar { get; private set; } = 20;

        #endregion

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
        public bool CanCreateNewTimer { get; set; }

        /// <summary>
        /// List of timers in the group.
        /// </summary>
        public ObservableCollection<TimerItemViewModel> TimerList { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command play group.
        /// </summary>
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// The command pause group.
        /// </summary>
        public ICommand PauseCommand { get; set; }

        /// <summary>
        /// The command to add new timer to the group.
        /// </summary>
        public ICommand AddTimerCommand { get; set; }

        /// <summary>
        /// The command to open group settings.
        /// </summary>
        public ICommand OpenGroupSettingsCommand { get; set; }

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
            PlayCommand = new RelayCommand(async () => await PlayAsync());
            PauseCommand = new RelayCommand(async () => await PauseAsync());
            AddTimerCommand = new RelayCommand(async () => await AddTimerAsync());
            OpenGroupSettingsCommand = new RelayCommand(async () => await OpenGroupSettingsAsync());
        }

        private async Task PlayAsync()
        {
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task PauseAsync()
        {
            Title = "Hey";
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task AddTimerAsync()
        {
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task OpenGroupSettingsAsync()
        {
            // Create Settings View Model with the current group binding.
            TimerGroupSettingsFormViewModel vm = new TimerGroupSettingsFormViewModel
            {
                TimerGroupViewModel = this,
            };

            IoC.Application.GoToPage(ApplicationPage.TimerGroupSettingsForm, vm);

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
