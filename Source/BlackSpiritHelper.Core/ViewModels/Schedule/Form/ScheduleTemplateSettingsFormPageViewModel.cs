using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class ScheduleTemplateSettingsFormPageViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Schedule template associated to this settings.
        /// </summary>
        private ScheduleTemplateDataViewModel mFormVM = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Schedule template associated to this settings.
        /// </summary>
        public ScheduleTemplateDataViewModel FormVM
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
        /// TimeZoneRegion binding.
        /// </summary>
        public TimeZoneRegion TimeZoneRegion { get; set; }

        /// <summary>
        /// Schedule binding.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> Schedule { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to save changes.
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// The command to delete templatep.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// The command to go back to the schedule page.
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateSettingsFormPageViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        /// <summary>
        /// Bind properties to the inputs.
        /// </summary>
        private void BindProperties()
        {
            if (FormVM == null)
                return;

            Title = FormVM.Title;
            TimeZoneRegion = FormVM.TimeZoneRegion;
            Schedule = FormVM.CreateScheduleDeepCopy();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayCommand(() => SaveChanges());
            DeleteCommand = new RelayCommand(() => DeleteTemplate());
            GoBackCommand = new RelayCommand(() => GoBack());
        }

        /// <summary>
        /// Delete template.
        /// </summary>
        private void DeleteTemplate()
        {
            if (IoC.DataContent.ScheduleDesignModel.IsRunning)
                return;

            // Remove schedule.
            if (!IoC.DataContent.ScheduleDesignModel.DestroyCustomTemplate(FormVM))
            {
                // Some error occured during deleting the template.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Cannot delete the template!",
                    Message = $"Unexpected error occured during deleting the template.{Environment.NewLine}" +
                               "Please, contact the developers to fix the issue.",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Move back to the page.
            GoBack();
        }

        /// <summary>
        /// Save settings.
        /// </summary>
        private void SaveChanges()
        {
            if (IoC.DataContent.ScheduleDesignModel.IsRunning)
                return;

            // Trim.
            string title = Title.Trim();

            // Validate inputs.
            if (!Core.ScheduleTemplateDataViewModel.ValidateInputs(FormVM, title, TimeZoneRegion, Schedule))
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

            // Sort schedule.
            FormVM.SortSchedule();

            // Save changes.
            #region Save changes

            FormVM.Title = title;
            FormVM.TimeZoneRegion = TimeZoneRegion;
            FormVM.Schedule = Schedule;

            #endregion

            // Log it.
            IoC.Logger.Log($"Template '{FormVM.Title}' settings changed!", LogLevel.Info);

            // Update template title list presenter.
            IoC.DataContent.ScheduleDesignModel.SetTemplateTitleListPresenter();

            // Move back to the page.
            GoBack();
        }

        /// <summary>
        /// Back back command.
        /// </summary>
        private void GoBack()
        {
            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Schedule);
        }

        #endregion
    }
}
