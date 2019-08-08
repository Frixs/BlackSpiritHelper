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
        #region Private Members

        /// <summary>
        /// Temporary ID is useful to track equality of time items between <see cref="ScheduleTemplateDataViewModel.Schedule"/> and <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/>.
        /// </summary>
        private int mTemporaryID = -1;

        #endregion

        #region Public Properties

        /// <summary>
        /// Temporary ID is useful to track equality of time items between <see cref="ScheduleTemplateDataViewModel.Schedule"/> and <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/>.
        /// </summary>
        [XmlIgnore]
        public int TemporaryID
        {
            get => mTemporaryID;
            set
            {
                if (mTemporaryID > -1)
                    return;
                mTemporaryID = value;
            }
        }

        /// <summary>
        /// Says, if the time is marked as next item to countdown to.
        /// It is used for GUI to show the time event with special marking.
        /// </summary>
        [XmlIgnore]
        public bool IsMarkedAsNext { get; set; } = false;

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
        /// !!! This is only for loading at application start.
        /// </summary>
        public ObservableCollection<string> ItemList { get; set; }

        /// <summary>
        /// List of events at this time (<see cref="Time"/>).
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleItemDataViewModel> ItemListPresenter { get; set; }

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
