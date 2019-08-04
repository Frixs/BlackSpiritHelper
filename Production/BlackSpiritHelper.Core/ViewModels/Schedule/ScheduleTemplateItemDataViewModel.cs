using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [Serializable]
    public class ScheduleTemplateDayDataViewModel : BaseViewModel
    {
        #region Public Properties

        public DayOfWeek DayOfWeek { get; set; }

        public ObservableCollection<ScheduleTemplateDayTimeDataViewModel> TimeList { get; set; }

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
