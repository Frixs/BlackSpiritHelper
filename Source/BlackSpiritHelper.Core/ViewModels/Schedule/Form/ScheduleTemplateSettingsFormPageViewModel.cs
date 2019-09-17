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
        private ScheduleTemplateDataViewModel mScheduleTemplateDataViewModel = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Schedule template associated to this settings.
        /// </summary>
        public ScheduleTemplateDataViewModel ScheduleTemplateDataViewModel
        {
            get
            {
                return mScheduleTemplateDataViewModel;
            }
            set
            {
                mScheduleTemplateDataViewModel = value;

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
            if (ScheduleTemplateDataViewModel == null)
                return;

            Title = ScheduleTemplateDataViewModel.Title;
            TimeZoneRegion = ScheduleTemplateDataViewModel.TimeZoneRegion;
            Schedule = ScheduleTemplateDataViewModel.CreateScheduleDeepCopy();
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
        /// TODO
        /// </summary>
        private void DeleteTemplate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        private void SaveChanges()
        {
            Console.WriteLine(TimeZoneRegion.ToString() + " " + TimeZoneRegion.GetDescription());
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
