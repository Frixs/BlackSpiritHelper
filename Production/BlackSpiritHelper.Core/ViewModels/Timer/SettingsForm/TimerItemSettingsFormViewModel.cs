using System;
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

        #region Command Methods

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
            if (TimerItemViewModel.State != TimerState.Ready)
                return;

            // Substring the HEX color to the required form.
            // We recieve f.e. #FF000000 and we want to transform it into 000000.
            string iconBackgroundHEX;
            if (IconBackgroundHEX.Length == 9)
                iconBackgroundHEX = IconBackgroundHEX.Substring(3);
            // Color has hashmark.
            else if (IconBackgroundHEX.Length == 7)
                iconBackgroundHEX = IconBackgroundHEX.Substring(1);
            // Color hasn't changed.
            else
                iconBackgroundHEX = IconBackgroundHEX;

            // Validate inputs.
            if (!TimerItemViewModel.ValidateTimerInputs(Title, IconTitleShortcut, iconBackgroundHEX, TimeDuration, TimeSpan.FromSeconds(CountdownDuration), ShowInOverlay, AssociatedGroupViewModel) 
                || AssociatedGroupViewModel == null)
            {
                // Some error occured during saving changes of the timer.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Invalid Parameters!",
                    Message = $"Some of entered parameters are invalid. Please check them again.{Environment.NewLine}",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Save changes.
            #region Save changes

            if (TimerItemViewModel.GroupID != AssociatedGroupViewModel.ID)
            {
                // Find and remove timer from old group.
                if (!IoC.DataContent.TimerGroupListDesignModel.GetGroupByID(TimerItemViewModel.GroupID).TimerList.Remove(mTimerItemViewModel))
                {
                    // Some error occured during removing the timer from old group.
                    IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Caption = "An unexpected error occured!",
                        Message = $"Unexpected error while removing the timer from the old group. Please contact the developers to fix the issue.{Environment.NewLine}",
                        Button = System.Windows.MessageBoxButton.OK,
                        Icon = System.Windows.MessageBoxImage.Warning,
                    });

                    return;
                }
                // Add timer to the new group.
                AssociatedGroupViewModel.TimerList.Add(mTimerItemViewModel);
                // Set group ID.
                TimerItemViewModel.GroupID = AssociatedGroupViewModel.ID;

            }
            TimerItemViewModel.Title = Title;
            TimerItemViewModel.IconTitleShortcut = IconTitleShortcut;
            TimerItemViewModel.IconBackgroundHEX = iconBackgroundHEX;
            TimerItemViewModel.TimeDuration = TimeDuration;
            TimerItemViewModel.CountdownDuration = TimeSpan.FromSeconds(CountdownDuration);
            TimerItemViewModel.IsLoopActive = IsLoopActive;
            TimerItemViewModel.ShowInOverlay = ShowInOverlay;

            #endregion

            // Resort timer list alphabetically.
            AssociatedGroupViewModel.SortTimerList();

            // Log it.
            IoC.Logger.Log($"Timer '{TimerItemViewModel.Title}' settings changed!", LogLevel.Info);

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        private void DeleteTimer()
        {
            if (TimerItemViewModel.State != TimerState.Ready)
                return;

            // Remove timer.
            if (!IoC.DataContent.TimerGroupListDesignModel.GetGroupByID(TimerItemViewModel.GroupID).DestroyTimer(TimerItemViewModel))
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
