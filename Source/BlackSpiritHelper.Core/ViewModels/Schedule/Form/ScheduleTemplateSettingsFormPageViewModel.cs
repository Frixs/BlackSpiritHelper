namespace BlackSpiritHelper.Core
{
    public class ScheduleTemplateSettingsFormPageViewModel : BaseViewModel
    {
        #region Private Members

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



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateSettingsFormPageViewModel()
        {
        }

        /// <summary>
        /// Bind properties to the inputs.
        /// </summary>
        private void BindProperties()
        {
            if (ScheduleTemplateDataViewModel == null)
                return;

            //Title = ScheduleTemplateDataViewModel.Title;
        }

        #endregion
    }
}
