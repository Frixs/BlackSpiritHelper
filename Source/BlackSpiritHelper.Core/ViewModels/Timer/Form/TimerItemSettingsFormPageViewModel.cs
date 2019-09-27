using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerItemSettingsFormPageViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The Timer associated to this settings.
        /// </summary>
        public TimerItemDataViewModel mFormVM;

        #endregion

        #region Public Properties

        /// <summary>
        /// The Timer associated to this settings.
        /// </summary>
        public TimerItemDataViewModel FormVM
        {
            get
            {
                return mFormVM;
            }
            set
            {
                mFormVM = value;
                
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
        public sbyte GroupID { get; set; }

        /// <summary>
        /// Group binding.
        /// </summary>
        public TimerGroupDataViewModel AssociatedGroupViewModel { get; set; }

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

        /// <summary>
        /// The command to go back to the timer page.
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerItemSettingsFormPageViewModel()
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
            if (FormVM == null)
                return;

            Title                           = FormVM.Title;
            IconTitleShortcut               = FormVM.IconTitleShortcut;
            IconBackgroundHEX               = "#" + FormVM.IconBackgroundHEX;
            TimeDuration                    = FormVM.TimeDuration;
            CountdownDuration               = FormVM.CountdownDuration.TotalSeconds;
            IsLoopActive                    = FormVM.IsLoopActive;
            ShowInOverlay                   = FormVM.ShowInOverlay;
            GroupID                         = FormVM.GroupID;
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
            GoBackCommand = new RelayCommand(() => GoBack());
        }

        private void SaveChanges()
        {
            if (FormVM.State != TimerState.Ready)
                return;

            // Substring the HEX color to the required form.
            // We recieve f.e. #FF000000 and we want to transform it into 000000.
            string iconBackgroundHEX = IconBackgroundHEX.ToHexStringWithoutHashmark();
            
            // Trim.
            string title = Title.Trim();
            string titleShortcut = IconTitleShortcut.Trim();

            // Validate inputs.
            if (!Core.TimerItemDataViewModel.ValidateInputs(FormVM, title, titleShortcut, iconBackgroundHEX, TimeDuration, TimeSpan.FromSeconds(CountdownDuration), ShowInOverlay, AssociatedGroupViewModel, GroupID) 
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

            if (FormVM.GroupID != AssociatedGroupViewModel.ID)
            {
                // Find and remove timer from old group.
                if (!IoC.DataContent.TimerData.GetGroupByID(FormVM.GroupID).TimerList.Remove(mFormVM))
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
                AssociatedGroupViewModel.TimerList.Add(mFormVM);
                // Set group ID.
                FormVM.GroupID = AssociatedGroupViewModel.ID;

            }
            FormVM.Title = title;
            FormVM.IconTitleShortcut = titleShortcut;
            FormVM.IconBackgroundHEX = iconBackgroundHEX;
            FormVM.TimeDuration = TimeDuration;
            FormVM.CountdownDuration = TimeSpan.FromSeconds(CountdownDuration);
            FormVM.IsLoopActive = IsLoopActive;
            FormVM.ShowInOverlay = ShowInOverlay;

            #endregion

            // Resort timer list alphabetically.
            AssociatedGroupViewModel.SortTimerList();

            // Log it.
            IoC.Logger.Log($"Settings changed: timer '{FormVM.Title}'.", LogLevel.Info);

            // Move back to the page.
            GoBack();
        }

        private void DeleteTimer()
        {
            if (FormVM.State != TimerState.Ready)
                return;

            // Remove timer.
            if (!IoC.DataContent.TimerData.GetGroupByID(FormVM.GroupID).DestroyTimer(FormVM))
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
            GoBack();
        }

        private void GoBack()
        {
            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        #endregion
    }
}
