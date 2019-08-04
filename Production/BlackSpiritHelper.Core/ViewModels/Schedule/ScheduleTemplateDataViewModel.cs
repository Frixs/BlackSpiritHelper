using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Template model for <see cref="ScheduleViewModel"/>.
    /// TODO: Possible rework. We do not want to store copy of <see cref="Schedule"/> as <see cref="SchedulePresenter"/>.
    /// </summary>
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
                SchedulePresenter = mSchedule;
            }
        }

        /// <summary>
        /// TOOD comments
        /// </summary>
        [XmlIgnore]
        public bool IsScheduleConverted { get; private set; } = false;

        /// <summary>
        /// TOOD comments
        /// </summary>
        [XmlIgnore]
        public bool IsScheduleConvertedFlag { get; private set; } = false;

        /// <summary>
        /// TODO comment.
        /// </summary>
        [XmlIgnore]
        public string ConvertedTimeZoneID { get; private set; } = null;

        /// <summary>
        /// Schedule presenter.
        /// This is duplicate of <see cref="Schedule"/> due to we can set schedule to local and visa versa. We can modify it regardless <see cref="Schedule"/> which is going to be saved to user settings.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleTemplateDayDataViewModel> SchedulePresenter { get; set; }

        #endregion

        #region Commands

        [XmlIgnore]
        public ICommand Btn1Command { get; set; }

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
            Btn1Command = new RelayCommand(async () => await ToggleScheduleTimeZoneViewAsync());
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Toggle converted mode of schedule with possibility to show user schedule in his local time (if he is not in the local zone).
        /// </summary>
        private async Task ToggleScheduleTimeZoneViewAsync()
        {
            await RunCommandAsync(() => IsScheduleConvertedFlag, async () =>
            {
                if (IsScheduleConverted)
                {
                    IsScheduleConverted = false;

                    SchedulePresenter = null;
                    SchedulePresenter = Schedule;
                    await Task.Delay(1); //await ConvertScheduleToGivenTimeZoneAsync();
                }
                else
                {
                    IsScheduleConverted = true;
                    // TODO: !
                    SchedulePresenter = Schedule.CopyObject<ObservableCollection<ScheduleTemplateDayDataViewModel>>();
                    await ConvertScheduleToLocalAsync();
                }
            });
        }

        /// <summary>
        /// Convert schedule to user local time zone.
        /// </summary>
        /// <returns></returns>
        private async Task ConvertScheduleToLocalAsync()
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

                    // TODO. comments

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

            // Set to let know which time zone we are going to convert in.
            ConvertedTimeZoneID = TimeZoneInfo.Local.Id;

            await Task.Delay(1);
        }

        /// <summary>
        /// Convert schedule to given time zone.
        /// </summary>
        /// <returns></returns>
        private async Task ConvertScheduleToGivenTimeZoneAsync()
        {
            for (int iDay = SchedulePresenter.Count - 1; iDay > -1; iDay--)
            {
                var currDay = SchedulePresenter[iDay];

                for (int iTime = currDay.TimeList.Count - 1; iTime > -1; iTime--)
                {
                    var time = SchedulePresenter[iDay].TimeList[iTime];

                    DateTime todayDate = DateTime.Today;
                    DateTime.SpecifyKind(todayDate, DateTimeKind.Unspecified);

                    // Get offset.
                    DateTimeOffset currDate = new DateTimeOffset(todayDate);
                    // Set offset to appropriate day.
                    currDate = currDate.AddDays(
                        GetDayDifferenceOffset((int)currDay.DayOfWeek, (int)todayDate.DayOfWeek)
                        );
                    currDate = currDate.AddHours(time.Time.Hours).AddMinutes(time.Time.Minutes);
                    // Pretend, today's date is remote date and convert it into UTC.
                    DateTime localDate = TimeZoneInfo.ConvertTimeToUtc(currDate.DateTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZone.GetDescription()));
                    // Convert pretended date to our local timezone.
                    localDate = TimeZoneInfo.ConvertTimeFromUtc(localDate, TimeZoneInfo.FindSystemTimeZoneById(ConvertedTimeZoneID));

                    // TODO. comments

                    time.Time = localDate.TimeOfDay;

                    // Shuffle.
                    if (localDate.DayOfWeek != currDay.DayOfWeek)
                    {
                        SchedulePresenter[iDay].TimeList.Remove(time);
                        SchedulePresenter.First(o => o.DayOfWeek == localDate.DayOfWeek).TimeList.Add(time);
                    }

                }
            }

            // Unset time zone ID. We are going back to given time zone.
            ConvertedTimeZoneID = null;

            await Task.Delay(1);
        }

        /// <summary>
        /// Get offset difference between two given <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="wanted">The one which we want to go.</param>
        /// <param name="current">The one where we are currently at.</param>
        /// <returns></returns>
        private int GetDayDifferenceOffset(int wanted, int current)
        {
            if (wanted == current)
                return 0;

            if (current == 0)
                return wanted;

            return wanted - current;
        }

        #endregion
    }
}
