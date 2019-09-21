using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Template model for <see cref="ScheduleViewModel"/>.
    /// Next <see cref="ScheduleTemplateDayDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDataViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max <see cref="Title"/> length rule.
        /// </summary>
        public static byte AllowedTitleMaxLength { get; private set; } = 10;

        /// <summary>
        /// Min <see cref="Title"/> length rule.
        /// </summary>
        public static byte AllowedTitleMinLength { get; private set; } = 3;

        #endregion

        #region Private Members

        /// <summary>
        /// Default time zone represented with region enumerate.
        /// </summary>
        private TimeZoneRegion mTimeZoneRegion = TimeZoneRegion.UTC;

        /// <summary>
        /// Says, if the template is initialized.
        /// </summary>
        private bool mIsInitialized = false;

        /// <summary>
        /// Says if the template is predefined or not.
        /// </summary>
        private bool mIsPredefined = false;

        /// <summary>
        /// Schedule.
        /// </summary>
        private ObservableCollection<ScheduleTemplateDayDataViewModel> mSchedule;

        #endregion

        #region Public Properties

        /// <summary>
        /// Last modified time.
        /// It represents <see cref="DateTime.Ticks"/>.
        /// </summary>
        [XmlIgnore]
        public long LastModifiedTicks { get; set; }

        /// <summary>
        /// Setter for <see cref="LastModifiedTicks"/>.
        /// </summary>
        public string LastModifiedString
        {
            get => new DateTime(LastModifiedTicks).ToString("yyyy-MM-dd");
            set
            {
                DateTime date;
                DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                LastModifiedTicks = date.Ticks;
            }
        }

        /// <summary>
        /// Title of the template (unique).
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Default time zone represented with region enumerate.
        /// </summary>
        public TimeZoneRegion TimeZoneRegion
        {
            get => mTimeZoneRegion;
            set
            {
                mTimeZoneRegion = value;
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(value.GetDescription());
            }
        }

        /// <summary>
        /// Default time zone.
        /// </summary>
        [XmlIgnore]
        public TimeZoneInfo TimeZone { get; private set; }

        /// <summary>
        /// Get string representation of the currently applied time zone.
        /// </summary>
        [XmlIgnore]
        public string CurrentTimeZoneString
        {
            get
            {
                if (IsSchedulePresenterConverted)
                    return IoC.DateTime.TimeZoneToString(TimeZoneInfo.Local, true, true)
                        + (IoC.DataContent.ScheduleDesignModel.LocalTimeOffsetModifier == TimeSpan.Zero ? string.Empty : $" (Modifier: {IoC.DataContent.ScheduleDesignModel.LocalTimeOffsetModifierString})");
                return IoC.DateTime.TimeZoneToString(TimeZone, true, true);
            }
        }

        /// <summary>
        /// Schedule source.
        /// ---
        /// Has <see cref="ScheduleTemplateDayTimeDataViewModel.ItemListPresenter"/> set to null.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> Schedule
        {
            get => mSchedule;
            set
            {
                mSchedule = value;

                // Update only, if it has been initialized.
                if (mIsInitialized)
                    UpdatePresenter();
            }
        }

        /// <summary>
        /// Copy of schedule with presenter values.
        /// It is used to xaml presentation.
        /// We need copy, because we are modifying it and we do not want to have possibility to save it as modified.
        /// ---
        /// Has <see cref="ScheduleTemplateDayTimeDataViewModel.ItemList"/> set to null.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleTemplateDayDataViewModel> SchedulePresenter { get; set; }

        /// <summary>
        /// Says, if the template is converted to user's local time zone.
        /// </summary>
        [XmlIgnore]
        public bool IsSchedulePresenterConverted { get; private set; } = false;

        /// <summary>
        /// <see cref="IsSchedulePresenterConverted"/> flag barriere.
        /// </summary>
        [XmlIgnore]
        public bool IsSchedulePresenterConvertedFlag { get; private set; } = false;

        /// <summary>
        /// Says if the template is predefined or not.
        /// </summary>
        [XmlIgnore]
        public bool IsPredefined => mIsPredefined;

        #endregion

        #region Commands

        /// <summary>
        /// Command to toggle calendar/schedule time.
        /// </summary>
        [XmlIgnore]
        public ICommand ToggleLocalCalendarTimeCommand { get; set; }

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

        /// <summary>
        /// Initialize the instance.
        /// </summary>
        /// <param name="isPredefined"></param>
        public void Init(bool isPredefined = false)
        {
            if (mIsInitialized)
                return;
            mIsInitialized = true;

            mIsPredefined = isPredefined;
            SortSchedule();
            UpdatePresenter();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            ToggleLocalCalendarTimeCommand = new RelayCommand(async () => await ToggleScheduleTimeZoneViewAsync());
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Unamrk all marked as <see cref="ScheduleTemplateDayTimeDataViewModel.IsMarkedAsNext"/>.
        /// </summary>
        /// <param name="firstOccuranceOnly"></param>
        public void UnmarkAllAsNext(bool firstOccuranceOnly = false)
        {
            // Umark first occurance.
            for (int iDay = 0; iDay < SchedulePresenter.Count; iDay++)
            {
                for (int iTime = 0; iTime < SchedulePresenter[iDay].TimeList.Count; iTime++)
                {
                    if (!SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext)
                        continue;
                    SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = false;
                    if (firstOccuranceOnly)
                        return;
                }
            }
        }

        /// <summary>
        /// Find <see cref="ScheduleTemplateDayTimeDataViewModel"/> in <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/> and mark it.
        /// </summary>
        /// <param name="timeItem"></param>
        public void FindAndMarkAsNext(ScheduleTemplateDayTimeDataViewModel timeItem)
        {
            if (timeItem == null)
            {
                IoC.Logger.Log("Time item not defined!", LogLevel.Error);
                return;
            }

            for (int iDay = 0; iDay < SchedulePresenter.Count; iDay++)
            {
                for (int iTime = 0; iTime < SchedulePresenter[iDay].TimeList.Count; iTime++)
                {
                    if (timeItem.TemporaryID == SchedulePresenter[iDay].TimeList[iTime].TemporaryID)
                    {
                        timeItem.IsMarkedAsNext = true;
                        SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="FindAndMarkAsNext"/> and <see cref="UnmarkAllAsNext(bool)"/> methods together in one loop.
        /// Works only for 1 occurance.
        /// </summary>
        /// <param name="timeItem"></param>
        public void FindAndRemarkAsNew(ScheduleTemplateDayTimeDataViewModel timeItem)
        {
            if (timeItem == null)
            {
                IoC.Logger.Log("Time item not defined!", LogLevel.Error);
                return;
            }

            bool doneMark = false;
            bool doneUnmark = false;

            // Unmark schedule.
            ScheduleUnmarkAllAsNext(true);

            // Go through presenter.
            for (int iDay = 0; iDay < SchedulePresenter.Count; iDay++)
            {
                var day = SchedulePresenter[iDay];

                for (int iTime = 0; iTime < day.TimeList.Count; iTime++)
                {
                    var time = day.TimeList[iTime];

                    if (!doneUnmark && time.IsMarkedAsNext)
                    {
                        time.IsMarkedAsNext = false;
                        doneUnmark = true;
                    }

                    if (!doneMark && timeItem == null)
                    {
                        doneMark = true;
                    }
                    else if (!doneMark && timeItem.TemporaryID == time.TemporaryID)
                    {
                        timeItem.IsMarkedAsNext = true;
                        time.IsMarkedAsNext = true;
                        doneMark = true;
                    }

                    if (doneMark && doneUnmark)
                        return;
                }
            }
        }

        /// <summary>
        /// Create copy of <see cref="SchedulePresenter"/> or <see cref="Schedule"/>.
        /// </summary>
        /// <param name="generatePresenter">Generate presenter or source?</param>
        /// <returns></returns>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> CreateScheduleCopy(bool generatePresenter)
        {
            return GenerateNewSchedule(generatePresenter, false);
        }

        /// <summary>
        /// Create copy of <see cref="Schedule"/> built from <see cref="SchedulePresenter"/>.
        /// ---
        /// !!! Be very careful with using this method! Generally, we do not want to override source schedule.
        /// </summary>
        /// <param name="externalSource"></param>
        /// <returns></returns>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> CreateScheduleFromPresenter(ObservableCollection<ScheduleTemplateDayDataViewModel> externalSource = null)
        {
            var schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>();

            // Specify source.
            var source = SchedulePresenter;
            if (externalSource != null)
                source = externalSource;

            // Go through days.
            for (int iDay = 0; iDay < source.Count; iDay++)
            {
                // Current day of SchedulePresenter.
                var currDay = source[iDay];
                // Create time list for source.
                ObservableCollection<ScheduleTemplateDayTimeDataViewModel> timeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>();

                // Go through times and create time event list for each day.
                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    // Current time event of SchedulePresenter.
                    var currTimeEvent = currDay.TimeList[iTime];

                    // Create time event.
                    var timeEvent = new ScheduleTemplateDayTimeDataViewModel
                    {
                        Time = currTimeEvent.Time,
                    };

                    // Create item list for source.
                    ObservableCollection<string> itemList = new ObservableCollection<string>();
                    // Go thourgh items from presenter.
                    for (int iItem = 0; iItem < currTimeEvent.ItemListPresenter.Count; iItem++)
                    {
                        // Current iem of Schedule presenter.
                        var currItem = currTimeEvent.ItemListPresenter[iItem];

                        // Create and add item to the source list.
                        itemList.Add(currItem.Name);
                    }

                    // Assign source item list.
                    timeEvent.ItemList = itemList;

                    // Add time event to the time list.
                    timeList.Add(timeEvent);
                }

                // Add day to the schedule source.
                schedule.Add(new ScheduleTemplateDayDataViewModel
                {
                    DayOfWeek = currDay.DayOfWeek,
                    TimeList = timeList,
                });
            }

            // Return.
            return schedule;
        }

        /// <summary>
        /// Sort <see cref="Schedule"/>.
        /// </summary>
        public void SortSchedule()
        {
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                Schedule[iDay].TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>(Schedule[iDay].TimeList.OrderBy(o => o.Time));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Unmark <see cref="Schedule"/> first occurance.
        /// </summary>
        /// <param name="firstOccuranceOnly"></param>
        private void ScheduleUnmarkAllAsNext(bool firstOccuranceOnly = false)
        {
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                var day = Schedule[iDay];

                for (int iTime = 0; iTime < day.TimeList.Count; iTime++)
                {
                    var time = day.TimeList[iTime];

                    if (time.IsMarkedAsNext)
                    {
                        time.IsMarkedAsNext = false;
                        if (firstOccuranceOnly)
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Toggle converted mode of schedule with possibility to show user schedule in his local time (if he is not in the local zone).
        /// </summary>
        private async Task ToggleScheduleTimeZoneViewAsync()
        {
            await RunCommandAsync(() => IsSchedulePresenterConvertedFlag, async () =>
            {
                if (IsSchedulePresenterConverted)
                {
                    if (ConvertSchedulePresenterToGivenTimeZone())
                        IsSchedulePresenterConverted = false;
                }
                else
                {
                    if (ConvertSchedulePresenterToLocal())
                        IsSchedulePresenterConverted = true;
                }


                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Convert <see cref="SchedulePresenter"/> to user local time zone.
        /// </summary>
        /// <returns></returns>
        private bool ConvertSchedulePresenterToLocal()
        {
            DateTime todayDate = DateTime.Today;
            List<ScheduleTemplateDayTimeDataViewModel> alreadyChecked = new List<ScheduleTemplateDayTimeDataViewModel>();

            for (int iDay = SchedulePresenter.Count - 1; iDay > -1; iDay--)
            {
                var day = SchedulePresenter[iDay];

                for (int iTime = day.TimeList.Count - 1; iTime > -1; iTime--)
                {
                    var time = day.TimeList[iTime];

                    // let pass only non-checked.
                    if (alreadyChecked.Contains(time))
                        continue;

                    DateTimeOffset currDate = new DateTimeOffset(todayDate);
                    // Set timezone to the correct one.
                    IoC.DateTime.SetTimeZone(ref currDate, TimeZone);
                    // Set day offset to appropriate day and set appropriate time of the day.
                    currDate = currDate
                        .AddDays(IoC.DateTime.GetDayDifferenceOffset((int)day.DayOfWeek, (int)todayDate.DayOfWeek))
                        .AddHours(time.Time.Hours)
                        .AddMinutes(time.Time.Minutes);

                    // Transform the date to user's local timezone.
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(
                        currDate.UtcDateTime + IoC.DataContent.ScheduleDesignModel.LocalTimeOffsetModifier, // We do not want to convert time with DST offset. For that reason, we use UtcDateTime property. If we so then we need to use TimeZoneInfo.ConvertTimeToUtc .
                        TimeZoneInfo.Local
                        );

                    // Set new time of the day.
                    time.Time = localDate.TimeOfDay;

                    // If modified time belongs to another day, change the day.
                    if (localDate.DayOfWeek != day.DayOfWeek)
                    {
                        SchedulePresenter[iDay].TimeList.Remove(time);
                        SchedulePresenter.First(o => o.DayOfWeek == localDate.DayOfWeek).TimeList.Add(time);
                    }

                    // Add it to already checked to avoid it to check it multiple times.
                    alreadyChecked.Add(time);
                }
            }

            // Resort.
            SortSchedulePresenter();

            // All OK.
            return true;
        }

        /// <summary>
        /// Convert <see cref="SchedulePresenter"/> to given time zone (default value of <see cref="Schedule"/>).
        /// </summary>
        /// <returns></returns>
        private bool ConvertSchedulePresenterToGivenTimeZone()
        {
            // Get default values from original.
            UpdatePresenter();

            // Update ignored list after change.
            IoC.Task.Run(async () => await ScheduleItemDataViewModel.OnItemIgnoredMoveAsync());

            // All OK.
            return true;
        }

        /// <summary>
        /// Sort <see cref="SchedulePresenter"/>.
        /// </summary>
        private void SortSchedulePresenter()
        {
            for (int iDay = 0; iDay < SchedulePresenter.Count; iDay++)
            {
                SchedulePresenter[iDay].TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>(SchedulePresenter[iDay].TimeList.OrderBy(o => o.Time));
            }
        }

        /// <summary>
        /// Update presenter <see cref="SchedulePresenter"/> from default values <see cref="Schedule"/>.
        /// </summary>
        private void UpdatePresenter()
        {
            SchedulePresenter = GenerateNewSchedule(true, true);
        }

        /// <summary>
        /// Generate new schedule (source-copy or presenter) from source <see cref="Schedule"/>.
        /// </summary>
        /// <param name="generatePresenter">Generate presenter or source?</param>
        /// <param name="tempLinkToSource">Should add pointers to source Schedule to point to new schedule?</param>
        /// <returns></returns>
        private ObservableCollection<ScheduleTemplateDayDataViewModel> GenerateNewSchedule(bool generatePresenter, bool tempLinkToSource = false)
        {
            var schedule = new ObservableCollection<ScheduleTemplateDayDataViewModel>();

            // Go through days.
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                // Current day of Schedule source.
                var currDay = Schedule[iDay];
                // Create time list for presenter.
                ObservableCollection<ScheduleTemplateDayTimeDataViewModel> timeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>();

                // Go through times and create time event list for each day.
                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    // Current time event of Schedule source.
                    var currTimeEvent = currDay.TimeList[iTime];

                    // Create time event.
                    var timeEvent = new ScheduleTemplateDayTimeDataViewModel
                    {
                        Time = currTimeEvent.Time,
                    };

                    // Generate presenter or source?
                    // Presenter item generation.
                    if (generatePresenter)
                    {
                        // Link to schedule source.
                        if (tempLinkToSource)
                        {
                            // Generate time temporary ID.
                            var tempId = int.Parse((iDay + 1).ToString() + (iTime + 1).ToString());
                            // Assign temporary ID to Schedule's list (not the presenter one).
                            currTimeEvent.TemporaryID = tempId;
                            // Assign values to presenter.
                            timeEvent.TemporaryID = tempId;
                            timeEvent.IsMarkedAsNext = currTimeEvent.IsMarkedAsNext;
                        }

                        // Create item list for presenter.
                        ObservableCollection<ScheduleItemDataViewModel> itemList = new ObservableCollection<ScheduleItemDataViewModel>();
                        // Go thourgh items from source.
                        for (int iItem = 0; iItem < currTimeEvent.ItemList.Count; iItem++)
                        {
                            // Current iem of Schedule source.
                            var currItem = currTimeEvent.ItemList[iItem];

                            // Create and add item to the presenter list.
                            itemList.Add(
                                IoC.DataContent.ScheduleDesignModel.GetItemByName(currItem)
                                );
                        }

                        // Assign presenter item list.
                        timeEvent.ItemListPresenter = itemList;
                    }
                    // Source item generation.
                    else
                    {
                        // Create item list for source-copy.
                        ObservableCollection<string> itemList = new ObservableCollection<string>();
                        // Go thourgh items from source.
                        for (int iItem = 0; iItem < currTimeEvent.ItemList.Count; iItem++)
                        {
                            // Current iem of Schedule source.
                            var currItem = currTimeEvent.ItemList[iItem];

                            // Create and add item to the source-copy list.
                            itemList.Add(currItem);
                        }

                        // Assign source-copy item list.
                        timeEvent.ItemList = itemList;
                    }

                    // Add time event to the time list.
                    timeList.Add(timeEvent);
                }

                // Add day to the schedule presenter.
                schedule.Add(new ScheduleTemplateDayDataViewModel
                {
                    DayOfWeek = currDay.DayOfWeek,
                    TimeList = timeList,
                });
            }

            // Return.
            return schedule;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Check schedule template parameters.
        /// TRUE, if all parameters are OK.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="title"></param>
        /// <param name="timeZoneRegion"></param>
        /// <returns></returns>
        public static bool ValidateInputs(ScheduleTemplateDataViewModel vm, string title, TimeZoneRegion timeZoneRegion)
        {
            #region Title

            if (!new ScheduleTitleRule().Validate(title, null).IsValid)
                return false;

            #endregion

            #region TimeZoneRegion

            if (!new ScheduleTimeZoneRegionRule().Validate(timeZoneRegion, null).IsValid)
                return false;

            #endregion

            return true;
        }

        #endregion
    }
}
