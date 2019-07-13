using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupSettingsFormViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The Group associated to this settings.
        /// </summary>
        public TimerGroupViewModel mTimerGroupViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The Group associated to this settings.
        /// </summary>
        public TimerGroupViewModel TimerGroupViewModel
        {
            get
            {
                return mTimerGroupViewModel;
            }
            set
            {
                mTimerGroupViewModel = value;

                // Bind properties to the inputs.
                BindProperties();
            }
        }

        /// <summary>
        /// Title binding.
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to save changes in the group settings.
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// The command to delete group.
        /// </summary>
        public ICommand DeleteGroupCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerGroupSettingsFormViewModel()
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
            if (TimerGroupViewModel == null)
                return;

            Title = TimerGroupViewModel.Title;
        }

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayCommand(() => SaveChanges());
            DeleteGroupCommand = new RelayCommand(() => DeleteGroup());
        }

        private void SaveChanges()
        {
            if (!TimerGroupViewModel.ValidateGroupInputs(Title))
            {
                // Some error occured during saving changes of the group.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Invalid Parameters!",
                    Message = $"Some of entered parameters are invalid. {Environment.NewLine}Group Name can contain only letters and numbers, {TimerGroupViewModel.TitleAllowMinChar} characters at minimum and {TimerGroupViewModel.TitleAllowMaxChar} characters at maximum.",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Save changes.
            TimerGroupViewModel.Title = Title;

            // Resort groups alphabetically.
            IoC.DataContent.TimerGroupListDesignModel.SortGroupList();

            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Timer);
        }

        private void DeleteGroup()
        {
            if (!IoC.DataContent.TimerGroupListDesignModel.DestroyGroup(TimerGroupViewModel))
            {
                // Some error occured during deleting the group.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Cannot delete the group!",
                    Message = $"The group is not empty or it is the last existing group! {Environment.NewLine}Please, remove all the timers in the group first. {Environment.NewLine}Number of timers in this group is {TimerGroupViewModel.TimerList.Count}.",
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
