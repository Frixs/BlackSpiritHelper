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
        /// Says, if this time item has all items ignored or not.
        /// !!! This is set only in <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/> not in <see cref="ScheduleTemplateDataViewModel.Schedule"/>.
        /// </summary>
        [XmlIgnore]
        public bool IsMarkedAsIgnored { get; set; } = false;

        /// <summary>
        /// Time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Time { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// <see cref="Time"/> hours.
        /// It is used to store <see cref="Time"/> in user settings.
        /// </summary>
        public int TimeHours
        {
            get => Time.Hours;
            set => Time = Time.Add(TimeSpan.FromHours(value));
        }

        /// <summary>
        /// <see cref="Time"/> minutes.
        /// It is used to store <see cref="Time"/> in user settings.
        /// </summary>
        public int TimeMinutes
        {
            get => Time.Minutes;
            set => Time = Time.Add(TimeSpan.FromMinutes(value));
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
