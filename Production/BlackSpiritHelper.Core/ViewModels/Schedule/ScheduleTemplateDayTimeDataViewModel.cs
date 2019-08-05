using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Previous <see cref="ScheduleTemplateDayDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDayTimeDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Time { get; set; }

        /// <summary>
        /// <see cref="Time"/> ticks.
        /// It is used to store <see cref="Time"/> in user settings.
        /// </summary>
        public long TimeTicks
        {
            get => Time.Ticks;
            set => Time = TimeSpan.FromTicks(value);
        }

        /// <summary>
        /// List of events at this time (<see cref="Time"/>).
        /// </summary>
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
