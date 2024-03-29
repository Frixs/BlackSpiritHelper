﻿using System;
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
    /// Template model for <see cref="ScheduleDataViewModel"/>.
    /// Next <see cref="ScheduleDayDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDataViewModel : ASetupableBaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max <see cref="Title"/> length rule.
        /// </summary>
        public static byte AllowedTitleMaxLength { get; private set; } = 15;

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
        /// Schedule.
        /// </summary>
        private ObservableCollection<ScheduleDayDataViewModel> mSchedule = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Last modified time.
        /// It represents <see cref="DateTime.Ticks"/>.
        /// </summary>
        [XmlIgnore]
        public long LastModifiedTicks { get; set; }

        /// <summary>
        /// User settings setter for <see cref="LastModifiedTicks"/>.
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
        public string Title { get; set; } = "NoTitle";

        /// <summary>
        /// Default time zone represented with region enumerate.
        /// </summary>
        public TimeZoneRegion TimeZoneRegion
        {
            get => mTimeZoneRegion;
            set
            {
                mTimeZoneRegion = SetTimeZoneByString(value.GetDescription()) ? value : TimeZoneRegion.UTC;
            }
        }

        /// <summary>
        /// Default time zone.
        /// </summary>
        [XmlIgnore]
        public TimeZoneInfo TimeZone { get; private set; } = TimeZoneInfo.Utc;

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
                        + (IoC.DataContent.ScheduleData.LocalTimeOffsetModifier == TimeSpan.Zero ? string.Empty : $" (Modifier: {IoC.DataContent.ScheduleData.LocalTimeOffsetModifierString})");
                return IoC.DateTime.TimeZoneToString(TimeZone, true, true);
            }
        }

        /// <summary>
        /// Schedule source.
        /// ---
        /// Has <see cref="ScheduleTimeEventDataViewModel.ItemListPresenter"/> set to null.
        /// </summary>
        public ObservableCollection<ScheduleDayDataViewModel> Schedule
        {
            get => mSchedule;
            set
            {
                mSchedule = value;

                // Update only, if it has been initialized.
                if (mIsInitDoneFlag)
                    UpdatePresenter();
            }
        }

        /// <summary>
        /// Copy of schedule with presenter values.
        /// It is used to xaml presentation.
        /// We need copy, because we are modifying it and we do not want to have possibility to save it as modified.
        /// ---
        /// Has <see cref="ScheduleTimeEventDataViewModel.ItemList"/> set to null.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleDayDataViewModel> SchedulePresenter { get; set; } = null;

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
        public bool IsPredefined { get; private set; } = false;

        #endregion

        #region Command Flags

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
        /// Init routine.
        /// </summary>
        /// <param name="parameters">
        ///     [0] = IsPredefined
        /// </param>
        protected override void InitRoutine(params object[] parameters)
        {
            if (!(parameters.Length > 0))
                IoC.Logger.Log("No required parameters!", LogLevel.Fatal);

            IsPredefined = (bool)parameters[0];
            SortSchedule();
            UpdatePresenter();
        }

        protected override void DisposeRoutine()
        {
            throw new NotImplementedException();
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Unamrk all marked as <see cref="ScheduleTimeEventDataViewModel.IsMarkedAsNext"/>.
        /// </summary>
        /// <param name="firstOccuranceOnly"></param>
        public void UnmarkAllAsNext(bool firstOccuranceOnly = false)
        {
            ScheduleUnmarkAllAsNext(false ,firstOccuranceOnly);
            ScheduleUnmarkAllAsNext(true ,firstOccuranceOnly);
        }

        /// <summary>
        /// Find <see cref="ScheduleTimeEventDataViewModel"/> in <see cref="SchedulePresenter"/> and mark it.
        /// </summary>
        /// <param name="sourceTimeEvent"></param>
        /// <param name="markSource">Mark source time event item too or not.</param>
        public void FindAndMarkAsNext(ScheduleTimeEventDataViewModel sourceTimeEvent, bool markSource)
        {
            if (sourceTimeEvent == null)
            {
                IoC.Logger.Log("Time-event item is not defined!", LogLevel.Error);
                return;
            }

            for (int iDay = 0; iDay < SchedulePresenter.Count; iDay++)
            {
                var currDay = SchedulePresenter[iDay];
                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    var currTimeEvent = currDay.TimeList[iTime];

                    if (sourceTimeEvent.TemporaryID == currTimeEvent.TemporaryID)
                    {
                        sourceTimeEvent.IsMarkedAsNext = true; // It should be source schedule.
                        currTimeEvent.IsMarkedAsNext = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="FindAndMarkAsNext"/> and <see cref="UnmarkAllAsNext(bool)"/> methods together in one loop.
        /// Only for 1 occurance.
        /// </summary>
        /// <param name="sourceTimeEvent"></param>
        /// <param name="markSource">Mark source time event item too or not.</param>
        public void FindAndRemarkAsNext(ScheduleTimeEventDataViewModel sourceTimeEvent, bool markSource)
        {
            if (sourceTimeEvent == null)
            {
                IoC.Logger.Log("Time-event item is not defined!", LogLevel.Error);
                return;
            }

            // Unmark an old item.
            UnmarkAllAsNext(true);

            // Mark a new item.
            FindAndMarkAsNext(sourceTimeEvent, markSource);
        }

        /// <summary>
        /// Create copy of <see cref="SchedulePresenter"/> or <see cref="Schedule"/>.
        /// </summary>
        /// <param name="generatePresenter">Generate presenter or source?</param>
        /// <returns></returns>
        public ObservableCollection<ScheduleDayDataViewModel> CreateScheduleCopy(bool generatePresenter)
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
        public ObservableCollection<ScheduleDayDataViewModel> CreateScheduleFromPresenter(ObservableCollection<ScheduleDayDataViewModel> externalSource = null)
        {
            var schedule = new ObservableCollection<ScheduleDayDataViewModel>();

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
                ObservableCollection<ScheduleTimeEventDataViewModel> timeList = new ObservableCollection<ScheduleTimeEventDataViewModel>();

                // Go through times and create time event list for each day.
                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    // Current time event of SchedulePresenter.
                    var currTimeEvent = currDay.TimeList[iTime];

                    // Create time event.
                    var timeEvent = new ScheduleTimeEventDataViewModel
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
                schedule.Add(new ScheduleDayDataViewModel
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
                Schedule[iDay].TimeList = new ObservableCollection<ScheduleTimeEventDataViewModel>(Schedule[iDay].TimeList.OrderBy(o => o.Time));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Unmark items in schedule.
        /// </summary>
        /// <param name="presenterAsSource">TRUE: Unmark items in presenter, otherwise onmark items in source schedule.</param>
        /// <param name="firstOccuranceOnly"></param>
        private void ScheduleUnmarkAllAsNext(bool presenterAsSource, bool firstOccuranceOnly = false)
        {
            var source = Schedule;
            if (presenterAsSource)
                source = SchedulePresenter;

            for (int iDay = 0; iDay < source.Count; iDay++)
            {
                var day = source[iDay];
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
        /// Convert <see cref="SchedulePresenter"/> to user local time zone.
        /// </summary>
        /// <returns></returns>
        private bool ConvertSchedulePresenterToLocal()
        {
            DateTime todayDate = DateTime.Today;
            List<ScheduleTimeEventDataViewModel> alreadyChecked = new List<ScheduleTimeEventDataViewModel>();

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
                        currDate.UtcDateTime + IoC.DataContent.ScheduleData.LocalTimeOffsetModifier, // We do not want to convert time with DST offset. For that reason, we use UtcDateTime property. If we so then we need to use TimeZoneInfo.ConvertTimeToUtc .
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
                SchedulePresenter[iDay].TimeList = new ObservableCollection<ScheduleTimeEventDataViewModel>(SchedulePresenter[iDay].TimeList.OrderBy(o => o.Time));
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
        private ObservableCollection<ScheduleDayDataViewModel> GenerateNewSchedule(bool generatePresenter, bool tempLinkToSource = false)
        {
            var schedule = new ObservableCollection<ScheduleDayDataViewModel>();

            // Go through days.
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                // Current day of Schedule source.
                var currDay = Schedule[iDay];
                // Create time list for presenter.
                ObservableCollection<ScheduleTimeEventDataViewModel> timeList = new ObservableCollection<ScheduleTimeEventDataViewModel>();

                // Go through times and create time event list for each day.
                for (int iTime = 0; iTime < currDay.TimeList.Count; iTime++)
                {
                    // Current time event of Schedule source.
                    var currTimeEvent = currDay.TimeList[iTime];

                    // Create time event.
                    var timeEvent = new ScheduleTimeEventDataViewModel
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
                                IoC.DataContent.ScheduleData.GetItemByName(currItem)
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
                schedule.Add(new ScheduleDayDataViewModel
                {
                    DayOfWeek = currDay.DayOfWeek,
                    TimeList = timeList,
                });
            }

            // Return.
            return schedule;
        }

        /// <summary>
        /// Set time zone by string ID.
        /// </summary>
        /// <param name="id">String id representing time zone.</param>
        /// <returns></returns>
        private bool SetTimeZoneByString(string id)
        {
            try
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(id);
            } 
            catch (Exception)
            {
                mTimeZoneRegion = TimeZoneRegion.UTC;
                TimeZone = TimeZoneInfo.Utc;
                return false;
            }

            return true;
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
