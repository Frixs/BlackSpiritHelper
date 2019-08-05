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
        /// Says, if the template is initialized.
        /// </summary>
        private bool mIsInitialized = false;

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

                // Update only, if it has been initialized.
                if (mIsInitialized)
                    UpdatePresenter();
            }
        }

        /// <summary>
        /// TODO comment
        /// </summary>
        public ObservableCollection<ScheduleTemplateDayDataViewModel> SchedulePresenter { get; set; }

        /// <summary>
        /// Says, if the template is converted to user's local time zone.
        /// </summary>
        [XmlIgnore]
        public bool IsScheduleConverted { get; private set; } = false;

        /// <summary>
        /// <see cref="IsScheduleConverted"/> flag barriere.
        /// </summary>
        [XmlIgnore]
        public bool IsScheduleConvertedFlag { get; private set; } = false;

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

        /// <summary>
        /// Initialize the instance.
        /// </summary>
        public void Init()
        {
            if (mIsInitialized)
                return;
            mIsInitialized = true;

            UpdatePresenter();
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
                    if (ConvertScheduleToGivenTimeZone())
                        IsScheduleConverted = false;
                }
                else
                {
                    if (ConvertScheduleToLocal())
                        IsScheduleConverted = true;
                }

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Convert schedule to user local time zone.
        /// </summary>
        /// <returns></returns>
        private bool ConvertScheduleToLocal()
        {
            DateTime todayDate = DateTime.Today;
            List<ScheduleTemplateDayTimeDataViewModel> alreadyChecked = new List<ScheduleTemplateDayTimeDataViewModel>();

            for (int iDay = SchedulePresenter.Count - 1; iDay > -1; iDay--)
            {
                var currDay = SchedulePresenter[iDay];

                for (int iTime = currDay.TimeList.Count - 1; iTime > -1; iTime--)
                {
                    var time = currDay.TimeList[iTime];

                    // let pass only non-checked.
                    if (alreadyChecked.Contains(time))
                        continue;

                    // Get offset.
                    DateTimeOffset currDate = new DateTimeOffset(todayDate);
                    // Set offset to appropriate day.
                    currDate = currDate.AddDays(
                        GetDayDifferenceOffset((int)currDay.DayOfWeek, (int)todayDate.DayOfWeek)
                        );
                    // Set appropriate time of the day.
                    currDate = currDate.AddHours(time.Time.Hours).AddMinutes(time.Time.Minutes);

                    // Get remote date.
                    DateTime remoteDate = TimeZoneInfo.ConvertTimeFromUtc(
                        currDate.UtcDateTime, // We do not want to convert time with DST offset. If we so then we need to use TimeZoneInfo.ConvertTimeToUtc .
                        TimeZoneInfo.FindSystemTimeZoneById(TimeZone.GetDescription())
                        );

                    // Set new time of the day.
                    time.Time = remoteDate.TimeOfDay;

                    // If modified time belongs to another day, change the day.
                    if (remoteDate.DayOfWeek != currDay.DayOfWeek)
                    {
                        SchedulePresenter[iDay].TimeList.Remove(time);
                        SchedulePresenter.First(o => o.DayOfWeek == remoteDate.DayOfWeek).TimeList.Add(time);
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
        /// Convert schedule to given time zone.
        /// </summary>
        /// <returns></returns>
        private bool ConvertScheduleToGivenTimeZone()
        {
            // Get default values from original.
            UpdatePresenter();

            // All OK.
            return true;
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

        /// <summary>
        /// Sort schedule.
        /// </summary>
        private void SortSchedulePresenter()
        {
            for (int iDay = 0; iDay < Schedule.Count; iDay++)
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
        /// TODO comment
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

                    timeList.Add(new ScheduleTemplateDayTimeDataViewModel
                    {
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
