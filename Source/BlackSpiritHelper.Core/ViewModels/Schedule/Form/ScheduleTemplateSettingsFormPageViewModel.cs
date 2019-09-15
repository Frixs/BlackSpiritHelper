using System;
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

        #endregion

        #region Commands

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
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            GoBackCommand = new RelayCommand(() => GoBack());
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
