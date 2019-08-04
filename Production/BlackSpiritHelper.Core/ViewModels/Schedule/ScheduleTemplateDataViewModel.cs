using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class ScheduleTemplateDataViewModel : BaseViewModel
    {
        #region Private Members

        public ObservableCollection<ScheduleTemplateDayDataViewModel> mSchedule;

        #endregion

        #region Public Properties

        /// <summary>
        /// Last update.
        /// It represents <see cref="DateTime.Ticks"/>.
        /// </summary>
        public long LastUpdate { get; set; }

        /// <summary>
        /// Says, if the template is custom or not.
        /// </summary>
        public bool IsCustom { get; set; } = true;

        /// <summary>
        /// Time zone.
        /// </summary>
        public RegionTimeZone TimeZone { get; set; } = RegionTimeZone.UTC;

        /// <summary>
        /// Schedule.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> Schedule
        {
            get => mSchedule;
            set
            {
                mSchedule = value;

                SchedulePresenter = Schedule.CopyObject<ObservableCollection<ScheduleTemplateDayDataViewModel>>();
            }
        }

        /// <summary>
        /// Schedule.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleTemplateDayDataViewModel> SchedulePresenter { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// TODO
        /// </summary>
        [XmlIgnore]
        public ICommand Btn1Command { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [XmlIgnore]
        public ICommand Btn2Command { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateDataViewModel()
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
            Btn1Command = new RelayCommand(async () => await Btn1Async());
            Btn2Command = new RelayCommand(async () => await Btn2Async());
        }

        private async Task Btn1Async()
        {
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                var currDay = Schedule[iDay];

                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    var time = Schedule[iDay].TimeList[iTime];

                    DateTime todayDate = DateTime.Today;
                    // Get offset.
                    DateTimeOffset currDate = new DateTimeOffset(todayDate);
                    // Set offset to appropriate day.
                    currDate = currDate.AddDays(
                        GetDayDifferenceOffset((int)currDay.DayOfWeek, (int)todayDate.DayOfWeek)
                        );
                    currDate = currDate.AddHours(time.Time.Hours).AddMinutes(time.Time.Minutes);
                    // Get remote date.
                    DateTime remoteDate = TimeZoneInfo.ConvertTimeFromUtc(
                        currDate.UtcDateTime,
                        TimeZoneInfo.FindSystemTimeZoneById(TimeZone.GetDescription())
                        );

                    // TODO.

                    var timePresenter = SchedulePresenter[iDay].TimeList.First(o => o.Time.Ticks == time.Time.Ticks);
                    timePresenter.Time = remoteDate.TimeOfDay;
                    // Shuffle.
                    if (remoteDate.DayOfWeek != currDay.DayOfWeek)
                    {
                        SchedulePresenter[iDay].TimeList.Remove(timePresenter);
                        SchedulePresenter.First(o => o.DayOfWeek == remoteDate.DayOfWeek).TimeList.Add(timePresenter);
                    }

                }
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// <see cref="DayOfWeek"/>. TODO
        /// </summary>
        /// <param name="wanted"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private int GetDayDifferenceOffset(int wanted, int current)
        {
            if (wanted == current)
                return 0;

            if (current == 0)
                return wanted;

            return wanted - current;
        }

        private async Task Btn2Async()
        {
            Console.WriteLine("B");

            await Task.Delay(1);
        }

        #endregion
    }
}
