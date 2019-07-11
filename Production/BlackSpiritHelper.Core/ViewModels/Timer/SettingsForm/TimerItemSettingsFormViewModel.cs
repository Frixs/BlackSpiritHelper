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
        public TimeSpan TimeTotal { get; set; }

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
            TimeTotal                       = TimerItemViewModel.TimeTotal;
            CountdownDuration               = TimerItemViewModel.CountdownDuration.TotalSeconds;
            IsLoopActive                    = TimerItemViewModel.IsLoopActive;
            GroupID                         = TimerItemViewModel.GroupID;
            AssociatedGroupViewModel        = null;
            ShowInOverlay                   = TimerItemViewModel.ShowInOverlay;
        }

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayCommand(() => SaveChanges());
            DeleteTimerCommand = new RelayCommand(() => DeleteGroup());
        }

        private void SaveChanges()
        {
            // TODO Save changes.

            //if (!IoC.DataContent.TimerGroupListDesignModel.ValidateGroupInputs(Title))
            //{
            //    // Some error occured during saving changes of the group.
            //    IoC.UI.ShowMessage(new MessageBoxDialogViewModel
            //    {
            //        Caption = "Invalid Parameters!",
            //        Message = $"Some of entered parameters are invalid. {Environment.NewLine}Group Name can contain only letters and numbers, {TimerItemViewModel.TitleAllowMinChar} characters at minimum and {TimerItemViewModel.TitleAllowMaxChar} characters at maximum.",
            //        Button = System.Windows.MessageBoxButton.OK,
            //        Icon = System.Windows.MessageBoxImage.Warning,
            //    });

            //    return;
            //}

            // Save changes.
            TimerItemViewModel.Title = Title;

            // Resort groups alphabetically.
            IoC.DataContent.TimerGroupListDesignModel.SortGroupList();

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        private void DeleteGroup()
        {
            //if (!IoC.DataContent.TimerGroupListDesignModel.DeleteGroup())
            //{
            //    // Some error occured during deleting the group.
            //    IoC.UI.ShowMessage(new MessageBoxDialogViewModel
            //    {
            //        Caption = "Cannot delete the group!",
            //        Message = $"The group is not empty or it is the last existing group! {Environment.NewLine}Please, remove all the timers in the group first. {Environment.NewLine}Number of timers in this group is {TimerItemViewModel.TimerList.Count}.",
            //        Button = System.Windows.MessageBoxButton.OK,
            //        Icon = System.Windows.MessageBoxImage.Warning,
            //    });

            //    return;
            //}

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        #endregion
    }
}
