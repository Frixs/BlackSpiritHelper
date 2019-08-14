using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Previous <see cref="ScheduleTemplateDataViewModel"/>.
    /// Next <see cref="ScheduleTemplateDayTimeDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDayDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// List of all time events.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayTimeDataViewModel> TimeList { get; set; } = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateDayDataViewModel()
        {
        }

        #endregion
    }
}
