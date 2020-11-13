using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        public ObservableCollection<ScheduleDayDataViewModel> SchedulePresenter { get; set; }

        #endregion

        #region Command Flags

        private bool mModifyFlag { get; set; }

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

        #endregion

        /// <summary>
        /// Bind properties to the inputs.
        /// </summary>
        private void BindProperties()
        {
            if (FormVM == null)
                return;

            Title = FormVM.Title;
            TimeZoneRegion = FormVM.TimeZoneRegion;
            SchedulePresenter = FormVM.CreateScheduleCopy(true);
        }

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayCommand(async () => await SaveChangesCommandMethodAsync());
            DeleteCommand = new RelayCommand(async () => await DeleteTemplateCommandMethodAsync());
            GoBackCommand = new RelayCommand(() => GoBackCommandMethod());
        }

        /// <summary>
        /// Save settings.
        /// </summary>
        private async Task SaveChangesCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyFlag, async () =>
            {
                if (IoC.DataContent.ScheduleData.IsRunning)
                    return;

                // Trim.
                string title = Title.Trim();

                // Validate inputs.
                if (!Core.ScheduleTemplateDataViewModel.ValidateInputs(FormVM, title, TimeZoneRegion))
                {
                    // Some error occured during saving changes of the timer.
                    _ = IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                    {
                        Title = "INVALID VALUES",
                        Message = $"Some of the entered values are invalid. Please check them again.",
                        Result = NotificationBoxResult.Ok,
                    });

                    return;
                }

                // Save changes.
                #region Save changes

                FormVM.LastModifiedTicks = DateTime.Now.Ticks;
                FormVM.Title = title;
                FormVM.TimeZoneRegion = TimeZoneRegion;
                FormVM.Schedule = FormVM.CreateScheduleFromPresenter(SchedulePresenter);

                #endregion

                // Sort schedule.
                FormVM.SortSchedule();

                // Log it.
                IoC.Logger.Log($"Settings changed: template '{FormVM.Title}'.", LogLevel.Info);

                // Update template title list presenter.
                IoC.DataContent.ScheduleData.SetTemplateTitleListPresenter();

                // Move back to the page.
                GoBackCommandMethod();

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Delete template.
        /// </summary>
        private async Task DeleteTemplateCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyFlag, async () =>
            {
                if (IoC.DataContent.ScheduleData.IsRunning)
                    return;

                // Remove schedule.
                if (!IoC.DataContent.ScheduleData.DestroyCustomTemplate(FormVM))
                {
                    // Some error occured during deleting the template.
                    _ = IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Caption = "Cannot delete the template!",
                        Message = $"Unexpected error occurred during deleting the template.{Environment.NewLine}" +
                                  "Please, contact the developers to fix the issue.",
                        Button = System.Windows.MessageBoxButton.OK,
                        Icon = System.Windows.MessageBoxImage.Warning,
                    });

                    return;
                }

                // Move back to the page.
                GoBackCommandMethod();

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Back back command.
        /// </summary>
        private void GoBackCommandMethod()
        {
            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Schedule);
        }

        #endregion
    }
}
