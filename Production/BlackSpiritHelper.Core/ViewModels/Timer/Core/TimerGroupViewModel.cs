using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

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

        /// <summary>
        /// Max number of timers that can be created in the group.
        /// </summary>
        public static byte AllowedMaxNoOfTimers { get; private set; } = 10;

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
        /// Says if you can create a new item. Limit check.
        /// </summary>
        [XmlIgnore]
        public bool CanCreateNewTimer => TimerList.Count < AllowedMaxNoOfTimers;

        /// <summary>
        /// List of timers in the group.
        /// </summary>
        public ObservableCollection<TimerItemViewModel> TimerList { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command play group.
        /// </summary>
        [XmlIgnore]
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// The command pause group.
        /// </summary>
        [XmlIgnore]
        public ICommand PauseCommand { get; set; }

        /// <summary>
        /// The command to add new timer to the group.
        /// </summary>
        [XmlIgnore]
        public ICommand AddTimerCommand { get; set; }

        /// <summary>
        /// The command to open group settings.
        /// </summary>
        [XmlIgnore]
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
            // TODO Play group.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task PauseAsync()
        {
            Title = "Hey";
            // TODO Pause Group.
            Console.WriteLine("TODO");
            await Task.Delay(1);
        }

        private async Task AddTimerAsync()
        {
            // Create default timer.
            AddTimer(new TimerItemViewModel
            {
                GroupID = ID,
                Title = "Untitled Timer",
                IconTitleShortcut = "X",
                IconBackgroundHEX = "FFFFFF",
                TimeDuration = new TimeSpan(0, 1, 0),
                CountdownDuration = TimeSpan.FromSeconds(0),
                State = TimerState.Ready,
                IsLoopActive = false,
                ShowInOverlay = false,
            });

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
        /// Add timer item to specific group.
        /// </summary>
        /// <param name="vm">The item.</param>
        public bool AddTimer(TimerItemViewModel vm)
        {
            IoC.Logger.Log($"Trying to add Timer '{vm.Title}' to group '{Title}'...", LogLevel.Debug);

            if (vm == null)
                return false;

            // Check limits.
            if (!CanCreateNewTimer)
                return false;

            TimerList.Add(vm);

            IoC.Logger.Log($"Timer '{vm.Title}' added to group '{Title}'!", LogLevel.Info);
            return true;
        }

        /// <summary>
        /// Delete the group permanently.
        /// </summary>
        /// <param name="vm">The item.</param>
        public bool DestroyTimer(TimerItemViewModel vm)
        {
            IoC.Logger.Log($"Trying to destroy Timer '{vm.Title}'...", LogLevel.Debug);

            if (vm == null)
                return false;

            var title = vm.Title;
            // Remove the group from the list.
            if (!TimerList.Remove(vm))
                return false;

            // Dispose timer calculations.
            vm.DisposeTimer();

            // Destroy reference to timer instance.
            vm = null;

            // Release GC.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            IoC.Logger.Log($"Timer '{title}' destroyed!", LogLevel.Info);
            return true;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Check group parameters.
        /// TRUE, if all parameters are OK and the group can be created.
        /// </summary>
        /// <param name="title">The group title.</param>
        /// <returns></returns>
        public static bool ValidateGroupInputs(string title)
        {
            title = title.Trim();

            // Check conditions.
            if (title.Length < TimerGroupViewModel.TitleAllowMinChar || title.Length > TimerGroupViewModel.TitleAllowMaxChar)
                return false;

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumericString(title, true, true))
                return false;

            return true;
        }

        #endregion
    }
}
