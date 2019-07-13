using System;
using System.Linq;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerItemSettingsFormViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The Timer associated to this settings.
        /// </summary>
        public TimerItemViewModel mTimerItemViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The Timer associated to this settings.
        /// </summary>
        public TimerItemViewModel TimerItemViewModel
        {
            get
            {
                return mTimerItemViewModel;
            }
            set
            {
                mTimerItemViewModel = value;

                // Bind properties to the inputs.
                BindProperties();
            }
        }

        /// <summary>
        /// Title binding.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// IconTitleShortcut binding.
        /// </summary>
        public string IconTitleShortcut { get; set; }

        /// <summary>
        /// IconBackgroundHEX binding.
        /// </summary>
        public string IconBackgroundHEX { get; set; }

        /// <summary>
        /// TimeTotal binding.
        /// </summary>
        public TimeSpan TimeDuration { get; set; }

        /// <summary>
        /// CountdownDuration binding.
        /// </summary>
        public double CountdownDuration { get; set; }

        /// <summary>
        /// IsLoopActive binding.
        /// </summary>
        public bool IsLoopActive { get; set; }

        /// <summary>
        /// GroupID binding.
        /// </summary>
        public byte GroupID { get; set; }

        /// <summary>
        /// Group binding.
        /// </summary>
        public TimerGroupViewModel AssociatedGroupViewModel { get; set; }

        /// <summary>
        /// ShowOnOverlay binding.
        /// </summary>
        public bool ShowInOverlay { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to save changes in the group settings.
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// The command to delete group.
        /// </summary>
        public ICommand DeleteTimerCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerItemSettingsFormViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        /// <summary>
        /// Bind properties to the inputs.
        /// </summary>
        private void BindProperties()
        {
            if (TimerItemViewModel == null)
                return;

            Title                           = TimerItemViewModel.Title;
            IconTitleShortcut               = TimerItemViewModel.IconTitleShortcut;
            IconBackgroundHEX               = "#" + TimerItemViewModel.IconBackgroundHEX;
            TimeDuration                    = TimerItemViewModel.TimeDuration;
            CountdownDuration               = TimerItemViewModel.CountdownDuration.TotalSeconds;
            IsLoopActive                    = TimerItemViewModel.IsLoopActive;
            ShowInOverlay                   = TimerItemViewModel.ShowInOverlay;
            GroupID                         = TimerItemViewModel.GroupID;
            AssociatedGroupViewModel        = null;
        }

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayCommand(() => SaveChanges());
            DeleteTimerCommand = new RelayCommand(() => DeleteTimer());
        }

        private void SaveChanges()
        {
            //TODO
            Console.WriteLine(Title);

            if (TimerItemViewModel.State != TimerState.Ready)
                return;

            if (!TimerItemViewModel.ValidateTimerInputs(Title, IconTitleShortcut, IconBackgroundHEX, TimeDuration, TimeSpan.FromSeconds(CountdownDuration), ShowInOverlay))
            {
                // Some error occured during saving changes of the timer.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Invalid Parameters!",
                    Message = $"Some of entered parameters are invalid... {Environment.NewLine}{Environment.NewLine}" +
                              $"- Title can contain only letters and numbers, {TimerItemViewModel.TitleAllowMinChar} characters at minimum and {TimerItemViewModel.TitleAllowMaxChar} characters at maximum. {Environment.NewLine}{Environment.NewLine}" +
                              $"- Icon Title Shortcut can contain only letters and numbers, {TimerItemViewModel.IconTitleAllowMinChar} characters at minimum and {TimerItemViewModel.IconTitleAllowMaxChar} characters at maximum. {Environment.NewLine}{Environment.NewLine}" +
                              $"- There is limitation to number of timers in the overlay to {TimerItemViewModel.OverlayTimerLimitCount}.",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Save changes. TODO
            TimerItemViewModel.Title = Title;
            TimerItemViewModel.IconTitleShortcut = IconTitleShortcut;
            TimerItemViewModel.IconBackgroundHEX = IconBackgroundHEX;
            TimerItemViewModel.TimeDuration = TimeDuration;
            TimerItemViewModel.CountdownDuration = TimeSpan.FromSeconds(CountdownDuration);
            TimerItemViewModel.IsLoopActive = IsLoopActive;
            TimerItemViewModel.ShowInOverlay = ShowInOverlay;

            // Resort groups alphabetically.
            IoC.DataContent.TimerGroupListDesignModel.SortGroupList();

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        private void DeleteTimer()
        {
            if (TimerItemViewModel.State != TimerState.Ready)
                return;

            // Remove timer.
            if (!IoC.DataContent.TimerGroupListDesignModel.GroupList
                .First(o => o.ID == TimerItemViewModel.GroupID)
                .DestroyTimer(TimerItemViewModel))
            {
                // Some error occured during deleting the timer.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Cannot delete the timer!",
                    Message = $"Unexpected error occured during deleting the timer.{Environment.NewLine}" +
                               "Please, contact the developers to fix the issue.",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        #endregion
    }
}
