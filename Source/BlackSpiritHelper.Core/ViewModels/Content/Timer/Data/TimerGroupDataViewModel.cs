﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer Group. ViewModel for TimerGroupMenuItemControl.
    /// List of all groups is in the view model <see cref="TimerDataViewModel"/>.
    /// It contains list of timers: <see cref="TimerItemDataViewModel"/>.
    /// Data Content.
    /// </summary>
    public class TimerGroupDataViewModel : BaseViewModel
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
        public sbyte ID { get; set; } = -1;

        /// <summary>
        /// Group Title.
        /// </summary>
        public string Title { get; set; } = "NoName";

        /// <summary>
        /// Ignore the group in the overlay
        /// </summary>
        public bool IgnoreInOverlay { get; set; }

        /// <summary>
        /// Ignore the group in the overlay - update timer data group list property changed setter
        /// </summary>
        [XmlIgnore]
        public bool IgnoreInOverlayUpdate
        {
            get => IgnoreInOverlay;
            set
            {
                IgnoreInOverlay = value;
                IoC.DataContent.TimerData.OnPropertyChanged(nameof(IoC.DataContent.TimerData.GroupList));
            }
        }

        /// <summary>
        /// List of timers in the group.
        /// </summary>
        public ObservableCollection<TimerItemDataViewModel> TimerList { get; set; } = new ObservableCollection<TimerItemDataViewModel>();

        /// <summary>
        /// Button control - says if any of the child timers are active.
        /// </summary>
        [XmlIgnore]
        public bool IsRunning => TimerList.FirstOrDefault(o => o.IsRunning == true) == null ? false : true;

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        [XmlIgnore]
        public bool CanCreateNewTimer => TimerList.Count < AllowedMaxNoOfTimers;

        #endregion

        #region Command Flags

        private bool mPlayPauseCommandFlag { get; set; }
        private bool mModifyCommandFlag { get; set; }

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
        /// The command to reset all timers in the group.
        /// </summary>
        [XmlIgnore]
        public ICommand ResetTimersCommand { get; set; }

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

        public TimerGroupDataViewModel()
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
            PlayCommand = new RelayCommand(async () => await PlayCommandMethodAsync());
            PauseCommand = new RelayCommand(async () => await PauseCommandMethodAsync());
            ResetTimersCommand = new RelayCommand(async () => await ResetTimersCommandMethodAsync());
            AddTimerCommand = new RelayCommand(async () => await AddTimerCommandMethodAsync());
            OpenGroupSettingsCommand = new RelayCommand(async () => await OpenGroupSettingsCommandMethodAsync());
        }

        private async Task PlayCommandMethodAsync()
        {
            await RunCommandAsync(() => mPlayPauseCommandFlag, async () =>
            {
                await PlayAsync();
            });
        }

        private async Task PauseCommandMethodAsync()
        {
            await RunCommandAsync(() => mPlayPauseCommandFlag, async () =>
            {
                await PauseAsync();
            });
        }

        /// <summary>
        /// Restart all timers in the group.
        /// </summary>
        /// <returns></returns>
        private async Task ResetTimersCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                for (int i = 0; i < TimerList.Count; i++)
                    TimerList[i].TimerRestart();
                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Add new timer with default parameters.
        /// </summary>
        /// <returns></returns>
        private async Task AddTimerCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                // Create default timer.
                AddTimer(new TimerItemDataViewModel
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
            });
        }

        /// <summary>
        /// Open group settings.
        /// </summary>
        /// <returns></returns>
        private async Task OpenGroupSettingsCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                // Create Settings View Model with the current group binding.
                TimerGroupSettingsFormPageViewModel vm = new TimerGroupSettingsFormPageViewModel
                {
                    FormVM = this,
                };

                IoC.Application.GoToPage(ApplicationPage.TimerGroupSettingsForm, vm);

                await Task.Delay(1);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add timer item to specific group.
        /// </summary>
        /// <param name="vm">The item.</param>
        public bool AddTimer(TimerItemDataViewModel vm)
        {
            IoC.Logger.Log($"Adding timer '{vm.Title}' to group '{Title}'...", LogLevel.Debug);

            if (vm == null)
                return false;

            // Check limits.
            if (!CanCreateNewTimer)
                return false;

            // Run setup if it has not run yet.
            vm.Init();

            // Add timer to the list.
            TimerList.Add(vm);

            // Update properties.
            OnPropertyChanged(nameof(CanCreateNewTimer));

            IoC.Logger.Log($"Added timer '{vm.Title}' to group '{Title}'!", LogLevel.Info);
            return true;
        }

        /// <summary>
        /// Delete the timer permanently.
        /// </summary>
        /// <param name="vm">The item.</param>
        public bool DestroyTimer(TimerItemDataViewModel vm)
        {
            IoC.Logger.Log($"Destroying timer '{vm.Title}'...", LogLevel.Debug);

            if (vm == null)
                return false;

            // CHeck if the timer is running or not...
            if (vm.IsRunning)
                return false;

            var title = vm.Title;
            // Remove the timer from the list.
            if (!TimerList.Remove(vm))
                return false;

            // Dispose timer calculations.
            vm.DisposeTimerControl();

            // Destroy reference to timer instance.
            vm = null;

            // Release GC.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Update properties.
            OnPropertyChanged(nameof(CanCreateNewTimer));

            IoC.Logger.Log($"Destroyed timer '{title}'!", LogLevel.Info);
            return true;
        }

        /// <summary>
        /// Sort <see cref="TimerList"/> alphabetically.
        /// </summary>
        public void SortTimerList()
        {
            TimerList = new ObservableCollection<TimerItemDataViewModel>(
                TimerList.OrderBy(o => o.Title)
                );
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Play the timers in the group.
        /// </summary>
        /// <returns></returns>
        private async Task PlayAsync()
        {
            // We don't have any timer to run.
            if (TimerList.Count <= 0)
                return;

            bool isAnyInFreeze = false;

            // Chech if there is any freezed timer.
            foreach (TimerItemDataViewModel t in TimerList)
                if (t.IsInFreeze)
                {
                    isAnyInFreeze = true;
                    break;
                }

            // If there are freezed timers, run them, not all.
            // -
            // Lets run all freezed timers.
            if (isAnyInFreeze)
            {
                foreach (TimerItemDataViewModel t in TimerList)
                    if (t.IsInFreeze)
                        t.TimerPlay();

            }
            // Lets run all timers.
            else
            {
                foreach (TimerItemDataViewModel t in TimerList)
                    t.TimerPlay();
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Pause the timers in the group.
        /// </summary>
        /// <returns></returns>
        private async Task PauseAsync()
        {
            // We don't have any timer to pause.
            if (TimerList.Count <= 0)
                return;

            // Pause all timers in the group.
            foreach (TimerItemDataViewModel t in TimerList)
                t.TimerPause();

            await Task.Delay(1);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Check group parameters.
        /// TRUE, if all parameters are OK and the group can be created.
        /// </summary>
        /// <param name="title">The group title.</param>
        /// <returns></returns>
        public static bool ValidateInputs(string title)
        {
            #region Title

            title = title.Trim();

            if (!new TimerGroupTitleRule().Validate(title, null).IsValid)
                return false;

            #endregion

            return true;
        }

        #endregion
    }
}
