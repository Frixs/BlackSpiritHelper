using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        #region Private Members

        /// <summary>
        /// Temporary folder path.
        /// </summary>
        private string mTemporaryFolderRelPath = "Temporary/ScheduleSection";

        /// <summary>
        /// Temporary <see cref="Schedule"/> file name.
        /// </summary>
        private string mTemporaryScheduleFileName = "template.xml";

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
        /// Last update.
        /// It represents <see cref="DateTime.Ticks"/>.
        /// </summary>
        public long LastUpdate { get; set; }

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
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(mTimeZoneRegion.GetDescription());
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
        /// Schedule.
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

        #region Private Methods

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
        /// Sort <see cref="Schedule"/>.
        /// </summary>
        private void SortSchedule()
        {
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                Schedule[iDay].TimeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>(Schedule[iDay].TimeList.OrderBy(o => o.Time));
            }
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
        /// Save <see cref="Schedule"/> to temporary file.
        /// </summary>
        /// <returns></returns>
        private bool SaveToTemporaryFile()
        {
            XmlSerializer xs = new XmlSerializer(Schedule.GetType());

            try
            {
                Directory.CreateDirectory(mTemporaryFolderRelPath);
                FileStream file = File.Create(Path.Combine(mTemporaryFolderRelPath, mTemporaryScheduleFileName));
                xs.Serialize(file, Schedule);
            }
            catch (Exception ex)
            {
                IoC.Logger.Log($"Some error occurred during saving temporary file:{Environment.NewLine}{ex.Message}", LogLevel.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load <see cref="Schedule"/> from temporary file.
        /// </summary>
        /// <returns></returns>
        private bool LoadFromTemporaryFile()
        {
            XmlSerializer serializer = new XmlSerializer(Schedule.GetType());

            try
            {
                using (FileStream fileStream = new FileStream(Path.Combine(mTemporaryFolderRelPath, mTemporaryScheduleFileName), FileMode.Open))
                {
                    Schedule = (ObservableCollection<ScheduleTemplateDayDataViewModel>)serializer.Deserialize(fileStream);
                }
            }
            catch (Exception ex)
            {
                IoC.Logger.Log($"Some error occurred during loading temporary file:{Environment.NewLine}{ex.Message}", LogLevel.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update presenter <see cref="SchedulePresenter"/> from default values <see cref="Schedule"/>.
        /// Fill all the presenter fields instead of default one.
        /// </summary>
        private void UpdatePresenter()
        {
            SchedulePresenter = new ObservableCollection<ScheduleTemplateDayDataViewModel>();

            // Go through days.
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
            {
                ObservableCollection<ScheduleTemplateDayTimeDataViewModel> timeList = new ObservableCollection<ScheduleTemplateDayTimeDataViewModel>();

                // Go through times.
                for (int iTime = 0; iTime < Schedule[iDay].TimeList.Count; iTime++)
                {
                    ObservableCollection<ScheduleItemDataViewModel> itemList = new ObservableCollection<ScheduleItemDataViewModel>();

                    // Go through items.
                    for (int iItem = 0; iItem < Schedule[iDay].TimeList[iTime].ItemList.Count; iItem++)
                    {
                        itemList.Add(
                            IoC.DataContent.ScheduleDesignModel.GetItemByName(Schedule[iDay].TimeList[iTime].ItemList[iItem])
                            );
                    }

                    // Generate time temporary ID.
                    var tempId = int.Parse((iDay + 1).ToString() + (iTime + 1).ToString());

                    Schedule[iDay].TimeList[iTime].TemporaryID = tempId;

                    timeList.Add(new ScheduleTemplateDayTimeDataViewModel
                    {
                        TemporaryID = tempId,
                        IsMarkedAsNext = Schedule[iDay].TimeList[iTime].IsMarkedAsNext,
                        Time = Schedule[iDay].TimeList[iTime].Time,
                        ItemListPresenter = itemList,
                    });
                }

                SchedulePresenter.Add(new ScheduleTemplateDayDataViewModel
                {
                    DayOfWeek = Schedule[iDay].DayOfWeek,
                    TimeList = timeList,
                });
            }
        }

        #endregion
    }
}
