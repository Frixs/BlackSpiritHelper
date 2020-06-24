using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Previous <see cref="ScheduleTemplateDataViewModel"/>.
    /// Next <see cref="ScheduleTimeEventDataViewModel"/>.
    /// </summary>
    public class ScheduleDayDataViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of events in 1 particular day.
        /// </summary>
        public static byte AllowedMaxNoOfEventsInDay { get; private set; } = 15;

        #endregion

        #region Private Members

        /// <summary>
        /// List of all time events.
        /// </summary>
        private ObservableCollection<ScheduleTimeEventDataViewModel> mTimeList = new ObservableCollection<ScheduleTimeEventDataViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// List of all time events.
        /// </summary>
        public ObservableCollection<ScheduleTimeEventDataViewModel> TimeList
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
        public ScheduleDayDataViewModel()
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
            AddEventCommand = new RelayCommand(() => AddEvent(true));
            RemoveEventCommand = new RelayParameterizedCommand((parameter) => RemoveEvent(parameter));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add time event into <see cref="TimeList"/>.
        /// </summary>
        private void AddEvent(bool hasPresenter)
        {
            IoC.Logger.Log("Adding time-event to schedule...", LogLevel.Debug);

            var timeEvent = new ScheduleTimeEventDataViewModel();
            if (hasPresenter)
                timeEvent.ItemListPresenter = new ObservableCollection<ScheduleItemDataViewModel>();
            else
                timeEvent.ItemList = new ObservableCollection<string>();

            TimeList.Add(timeEvent);

            if (TimeList.Count >= AllowedMaxNoOfEventsInDay)
                CanAddEvent = false;
        }

        /// <summary>
        /// Remove time event into <see cref="TimeList"/>.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveEvent(object parameter)
        {
            IoC.Logger.Log("Removing time-event from schedule...", LogLevel.Debug);

            if (parameter == null || !parameter.GetType().Equals(typeof(ScheduleTimeEventDataViewModel)))
            {
                IoC.Logger.Log($"Wrong type - {parameter.GetType().ToString()}!", LogLevel.Error);
                return;
            }
            ScheduleTimeEventDataViewModel par = (ScheduleTimeEventDataViewModel)parameter;

            if (TimeList.Remove(par))
                CanAddEvent = true;
        }

        #endregion
    }
}
