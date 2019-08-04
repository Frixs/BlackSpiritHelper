using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [Serializable]
    public class ScheduleTemplateDayTimeDataViewModel : BaseViewModel
    {
        #region Public Properties

        [XmlIgnore]
        public TimeSpan Time { get; set; }

        public long TimeTicks
        {
            get => Time.Ticks;
            set => Time = TimeSpan.FromTicks(value);
        }

        public ObservableCollection<string> ItemList { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateDayTimeDataViewModel()
        {
        }

        #endregion
    }
}
