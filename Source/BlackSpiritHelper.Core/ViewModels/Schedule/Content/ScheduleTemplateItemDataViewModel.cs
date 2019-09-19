using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Previous <see cref="ScheduleTemplateDataViewModel"/>.
    /// Next <see cref="ScheduleTemplateDayTimeDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDayDataViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of events in 1 particular day.
        /// </summary>
        public static byte AllowedMaxNoOfEventsInDay { get; private set; } = 10;

        #endregion

        #region Private Members

        /// <summary>
        /// List of all time events.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayTimeDataViewModel> mTimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// List of all time events.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayTimeDataViewModel> TimeList
        {
            get => mTimeList;
            set
            {
                mTimeList = value;
                CanAddEvent = value.Count < AllowedMaxNoOfEventsInDay;
            }
        }
        
        /// <summary>
        /// Can add time event into <see cref="TimeList"/>?
        /// </summary>
        [XmlIgnore]
        public bool CanAddEvent { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to add event.
        /// </summary>
        [XmlIgnore]
        public ICommand AddEventCommand { get; set; }

        /// <summary>
        /// The command to remove event.
        /// </summary>
        [XmlIgnore]
        public ICommand RemoveEventCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateDayDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            AddEventCommand = new RelayCommand(() => AddEvent());
            RemoveEventCommand = new RelayParameterizedCommand((parameter) => RemoveEvent(parameter));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add time event into <see cref="TimeList"/>.
        /// </summary>
        private void AddEvent()
        {
            IoC.Logger.Log("Add Time Event", LogLevel.Debug);

            TimeList.Add(new ScheduleTemplateDayTimeDataViewModel());

            if (TimeList.Count >= AllowedMaxNoOfEventsInDay)
                CanAddEvent = false;
        }

        /// <summary>
        /// Remove time event into <see cref="TimeList"/>.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveEvent(object parameter)
        {
            IoC.Logger.Log("Remove Time Event", LogLevel.Debug);

            if (parameter == null || !parameter.GetType().Equals(typeof(ScheduleTemplateDayTimeDataViewModel)))
                return;
            ScheduleTemplateDayTimeDataViewModel par = (ScheduleTemplateDayTimeDataViewModel)parameter;

            if (TimeList.Remove(par))
                CanAddEvent = true;
        }

        #endregion
    }
}
