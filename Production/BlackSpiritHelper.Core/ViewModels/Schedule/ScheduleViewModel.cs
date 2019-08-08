using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// TODO: Add logger to whole Schedule section.
    /// </summary>
    public class ScheduleViewModel : DataContentBaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Timer control.
        /// </summary>
        private Timer mTimer;

        /// <summary>
        /// Time left to the next schedule event.
        /// </summary>
        private TimeSpan mTimeLeft = TimeSpan.Zero;

        /// <summary>
        /// Indicates the time when the timer is not shown, but instead of the timer, there is string overlay. If it is zero, it has no effect. Only value higher than zero indiacte this.
        /// </summary>
        private TimeSpan mTimeActiveCountdown = TimeSpan.Zero;

        /// <summary>
        /// Countdown time of <see cref="mTimeActiveCountdown"/> in seconds.
        /// </summary>
        private const double mTimeActiveCountdownSeconds = 60;

        /// <summary>
        /// Says, if the timer should show active countdown.
        /// </summary>
        private bool mIsActiveCountdownTime = false;

        /// <summary>
        /// Selected template.
        /// </summary>
        private ScheduleTemplateDataViewModel mSelectedTemplate;

        /// <summary>
        /// Template list, custom.
        /// </summary>
        private ObservableCollection<ScheduleTemplateDataViewModel> mTemplateCustomList = new ObservableCollection<ScheduleTemplateDataViewModel>();

        /// <summary>
        /// Item list, custom.
        /// </summary>
        private ObservableCollection<ScheduleItemDataViewModel> mItemCustomList = new ObservableCollection<ScheduleItemDataViewModel>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Selected template Title.
        /// </summary>
        public string SelectedTemplateTitle { get; set; } = string.Empty;

        /// <summary>
        /// Selected template.
        /// </summary>
        [XmlIgnore]
        public ScheduleTemplateDataViewModel SelectedTemplate
        {
            get => mSelectedTemplate;
            set
            {
                mSelectedTemplate = value;
                SelectedTemplateTitle = mSelectedTemplate.Title;
            }
        }

        /// <summary>
        /// Template list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleTemplateDataViewModel> TemplatePredefinedList { get; protected set; } = new List<ScheduleTemplateDataViewModel>();

        /// <summary>
        /// Template list, custom.
        /// </summary>
        public ObservableCollection<ScheduleTemplateDataViewModel> TemplateCustomList
        {
            get => mTemplateCustomList;
            set
            {
                mTemplateCustomList = value;
                CheckTemplateDuplicityCustom();
            }
        }

        /// <summary>
        /// Template list, presenter.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleTemplateDataViewModel> TemplateListPresenter
        {
            get
            {
                var l = new List<ScheduleTemplateDataViewModel>();
                l.AddRange(TemplatePredefinedList);
                l.AddRange(TemplateCustomList);
                return l;
            }
        }

        /// <summary>
        /// Item list, predefined.
        /// </summary>
        [XmlIgnore]
        public List<ScheduleItemDataViewModel> ItemPredefinedList { get; protected set; } = new List<ScheduleItemDataViewModel>();

        /// <summary>
        /// Item list, custom.
        /// </summary>
        public ObservableCollection<ScheduleItemDataViewModel> ItemCustomList
        {
            get => mItemCustomList;
            set
            {
                mItemCustomList = value;
                CheckItemDuplicityCustom();
            }
        }

        /// <summary>
        /// List of ignored items.
        /// </summary>
        public ObservableCollection<string> ItemIgnoredList { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// List of ignored items.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleItemDataViewModel> ItemIgnoredListPresenter { get; private set; } = new ObservableCollection<ScheduleItemDataViewModel>();

        /// <summary>
        /// List of all items except ignored one.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleItemDataViewModel> ItemIgnoreExceptList
        {
            get
            {
                var l = new List<ScheduleItemDataViewModel>();
                l.AddRange(ItemPredefinedList);
                l.AddRange(ItemCustomList);
                return new ObservableCollection<ScheduleItemDataViewModel>(l.Except(ItemIgnoredListPresenter).ToList());
            }
        }

        /// <summary>
        /// Offset modifier for the local time.
        /// </summary>
        [XmlIgnore]
        public TimeSpan LocalTimeOffsetModifier { get; set; }

        /// <summary>
        /// <see cref="LocalTimeOffsetModifier"/> Ticks.
        /// It is used to store <see cref="LocalTimeOffsetModifier"/> in user settings.
        /// </summary>
        public long LocalTimeOffsetModifierTicks
        {
            get => LocalTimeOffsetModifier.Ticks;
            set => LocalTimeOffsetModifier = TimeSpan.FromTicks(value);
        }

        /// <summary>
        /// Offset <see cref="LocalTimeOffsetModifier"/> in string form.
        /// </summary>
        public string LocalTimeOffsetModifierString
        {
            get
            {
                if (LocalTimeOffsetModifier > TimeSpan.Zero)
                    return $"+{LocalTimeOffsetModifier}";
                else if (LocalTimeOffsetModifier < TimeSpan.Zero)
                    return $"-{LocalTimeOffsetModifier}";
                else
                    return "0";
            }
        }

        /// <summary>
        /// Says, if a section is running.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning { get; protected set; }

        /// <summary>
        /// Run the section on application load.
        /// </summary>
        public bool RunOnLoad
        {
            get => IsRunning;
            set => IsRunning = value;
        }

        /// <summary>
        /// <see cref="mTimeLeft"/> presenter.
        /// Option to display time in GUI with the text. Not only a time.
        /// </summary>
        [XmlIgnore]
        public string TimeLeftPresenter { get; private set; } = "OFF";

        /// <summary>
        /// List of items that are currently in target.
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleItemDataViewModel> NextItemPresenterList { get; private set; } = new ObservableCollection<ScheduleItemDataViewModel>();

        /// <summary>
        /// TODO comment
        /// </summary>
        [XmlIgnore]
        public bool IsWarningTime { get; private set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to play the section.
        /// </summary>
        [XmlIgnore]
        public ICommand PlayCommand { get; set; }

        /// <summary>
        /// Command to stop the section.
        /// </summary>
        [XmlIgnore]
        public ICommand StopCommand { get; set; }

        /// <summary>
        /// Command to add a new template.
        /// </summary>
        [XmlIgnore]
        public ICommand AddTemplateCommand { get; set; }

        /// <summary>
        /// Command to clone a new template.
        /// </summary>
        [XmlIgnore]
        public ICommand CloneTemplateCommand { get; set; }

        /// <summary>
        /// Command to edit a new template.
        /// </summary>
        [XmlIgnore]
        public ICommand EditTemplateCommand { get; set; }

        /// <summary>
        /// Command to manage items.
        /// </summary>
        [XmlIgnore]
        public ICommand ManageItemsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleViewModel()
        {
            // Set the timer.
            SetTimer();

            // Create commands.
            CreateCommands();
        }

        protected override void SetDefaultsMethod()
        {
        }

        protected override void SetupMethod()
        {
            // Check duplicity.
            CheckItemDuplicityCustom();
            CheckTemplateDuplicityCustom();

            // Initialize items.
            for (int i = 0; i < ItemPredefinedList.Count; i++)
                ItemPredefinedList[i].Init(true);
            for (int i = 0; i < ItemCustomList.Count; i++)
                ItemCustomList[i].Init();

            // Initialize templates.
            for (int i = 0; i < TemplatePredefinedList.Count; i++)
                TemplatePredefinedList[i].Init(true);
            for (int i = 0; i < TemplateCustomList.Count; i++)
                TemplateCustomList[i].Init();

            // Set selected item.
            SelectedTemplate = GetTemplateByName(SelectedTemplateTitle);
            if (SelectedTemplate == null)
                SelectedTemplate = TemplatePredefinedList[0];

            // Set Ignored list.
            for (int i = 0; i < ItemIgnoredList.Count; i++)
            {
                var item = GetItemByName(ItemIgnoredList[i]);
                if (item != null && !ItemIgnoredListPresenter.Contains(item))
                    ItemIgnoredListPresenter.Add(item);
            }

            // Sort collections.
            TemplatePredefinedList = new List<ScheduleTemplateDataViewModel>(TemplatePredefinedList.OrderBy(o => o.Title));
            ItemPredefinedList = new List<ScheduleItemDataViewModel>(ItemPredefinedList.OrderBy(o => o.Name));
            SortTemplateCustomList();
            SortItemCustomList();
            SortItemIgnoredList();

            // Run if user wants.
            if (RunOnLoad)
                PlayAsync();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            PlayCommand = new RelayCommand(async () => await PlayAsync());
            StopCommand = new RelayCommand(async () => await StopAsync());
            // TODO: Template, Item control methods.
        }

        #endregion

        #region Timer Methods

        /// <summary>
        /// Set the timer.
        /// </summary>
        private void SetTimer()
        {
            mTimer = new Timer(1000);
            mTimer.Elapsed += TimerOnElapsed;
            mTimer.AutoReset = true;
        }

        /// <summary>
        /// Dispose timer calculations.
        /// Use this only while destroying the instance.
        /// </summary>
        public void DisposeTimer()
        {
            mTimer.Stop();
            mTimer.Elapsed -= TimerOnElapsed;
            mTimer.Dispose();
            mTimer = null;
            // TODO: Disposable DataContent?
        }

        /// <summary>
        /// On Tick timer event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            // Calculate time.
            mTimeLeft = mTimeLeft.Subtract(TimeSpan.FromSeconds(1));
            
            // Handle notification events.
            HandleNotificationEvents(mTimeLeft);

            // Update GUI time.
            if (mTimeActiveCountdown > TimeSpan.Zero)
            {
                mTimeActiveCountdown = mTimeActiveCountdown.Subtract(TimeSpan.FromSeconds(1));
                // Update active time string.
                UpdateTimeInUI("ACTIVE");
            }
            else
            {
                // Update with time value.
                UpdateTimeInUI(mTimeLeft);
            }

            // Timer reached zero.
            if (mTimeLeft.TotalSeconds <= 0)
            {
                PlayActiveCountdown();

                if (mTimeActiveCountdown <= TimeSpan.Zero)
                {
                    UpdateTimeTargetAsync();
                    StopActiveCountdown();
                }
            }
        }

        /// <summary>
        /// Update time in UI thread.
        /// </summary>
        /// <param name="ts"></param>
        private void UpdateTimeInUI(TimeSpan ts)
        {
            // Update UI thread.
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                TimeLeftPresenter = ts.ToString(@"%d\.hh\:mm\:ss");
            }));
        }

        /// <summary>
        /// Update time with string value in UI thread.
        /// </summary>
        /// <param name="ts"></param>
        private void UpdateTimeInUI(string str)
        {
            // Update UI thread.
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                TimeLeftPresenter = str;
            }));
        }

        /// <summary>
        /// Update time to the next target.
        /// </summary>
        private async Task UpdateTimeTargetAsync()
        {
            DateTime today = DateTime.Today;
            DateTime nowUtc = new DateTimeOffset(DateTime.Now + LocalTimeOffsetModifier).UtcDateTime;
            ScheduleTemplateDayTimeDataViewModel lastMatchingTimeItem = null;
            DateTimeOffset lastMatchingDate = default;

            // Get.
            for (int iDay = 0; iDay < SelectedTemplate.Schedule.Count; iDay++)
            {
                var day = SelectedTemplate.Schedule[iDay];

                DateTimeOffset dt = new DateTimeOffset(today);
                IoC.DateTime.SetTimeZone(ref dt, SelectedTemplate.TimeZone);
                dt = dt.AddDays(IoC.DateTime.GetDayDifferenceOffset((int)day.DayOfWeek, (int)today.DayOfWeek));

                for (int iTime = 0; iTime < day.TimeList.Count; iTime++)
                {
                    var time = day.TimeList[iTime];

                    dt += time.Time;

                    if (nowUtc < dt.UtcDateTime)
                        if (lastMatchingTimeItem == null || (lastMatchingTimeItem != null && lastMatchingDate.UtcDateTime > dt.UtcDateTime))
                        {
                            lastMatchingTimeItem = time;
                            lastMatchingDate = dt;
                        }

                    dt -= time.Time;
                }
            }

            // Mark as next.
            FindAndRemarkAsNew(lastMatchingTimeItem);

            // Update.
            if (lastMatchingTimeItem == null)
            {
                StopAsync();
                return;
            }

            // Set new countdown time.
            mTimeLeft = lastMatchingDate.UtcDateTime - nowUtc;
            // Set time items.
            NextItemPresenterList.Clear();
            for (int i = 0; i < lastMatchingTimeItem.ItemList.Count; i++)
            {
                var item = GetItemByName(lastMatchingTimeItem.ItemList[i]);
                if (item != null)
                    NextItemPresenterList.Add(item);
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Play active countdown.
        /// </summary>
        private void PlayActiveCountdown()
        {
            if (mIsActiveCountdownTime)
                return;
            mIsActiveCountdownTime = true;

            mTimeActiveCountdown = TimeSpan.FromSeconds(mTimeActiveCountdownSeconds);
        }

        /// <summary>
        /// Stop active countdown.
        /// </summary>
        private void StopActiveCountdown()
        {
            if (!mIsActiveCountdownTime)
                return;
            mIsActiveCountdownTime = false;
            mTimeActiveCountdown = TimeSpan.Zero;
        }

        /// <summary>
        /// Handle notification events.
        /// </summary>
        /// <param name="ts"></param>
        private void HandleNotificationEvents(TimeSpan ts)
        {
            // TODO: notification events.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="colorHex"></param>
        /// <param name="isPredefined"></param>
        /// <returns></returns>
        public ScheduleItemDataViewModel AddItem(string name, string colorHex, bool isPredefined = false)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(colorHex))
                return null;

            if (!colorHex.CheckColorHEX())
                return null;

            // Check duplicity.
            if (IsItemAlreadyDefined(name))
                return null;

            // Create the item.
            var item = new ScheduleItemDataViewModel
            {
                Name = name.Trim(),
                ColorHEX = colorHex,
            };

            // Ad the item to the list.
            if (isPredefined)
                ItemPredefinedList.Add(item);
            else
                ItemCustomList.Add(item);

            return item;
        }

        /// <summary>
        /// Check if the item is already defined.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsItemAlreadyDefined(string name)
        {
            if (ItemPredefinedList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                return true;

            if (ItemCustomList != null && ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim())) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Get item by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ScheduleItemDataViewModel GetItemByName(string name)
        {
            ScheduleItemDataViewModel ret = null;

            ret = ItemPredefinedList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim()));

            if (ret == null)
                ret = ItemCustomList.FirstOrDefault(o => o.Name.ToLower().Equals(name.ToLower().Trim()));

            return ret;
        }

        /// <summary>
        /// Is item ignored.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsItemIgnored(ScheduleItemDataViewModel item)
        {
            if (ItemIgnoredList.Contains(item.Name))
                return true;
            return false;
        }

        /// <summary>
        /// Add a new template.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="isPredefined"></param>
        /// <returns></returns>
        public ScheduleTemplateDataViewModel AddTemplate(ScheduleTemplateDataViewModel template, bool isPredefined = false)
        {
            if (IsTemplateAlreadyDefined(template.Title))
                return null;

            if (isPredefined)
                TemplatePredefinedList.Add(template);
            else
                TemplateCustomList.Add(template);

            return template;
        }

        /// <summary>
        /// Check if the template is already defined.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsTemplateAlreadyDefined(string title)
        {
            if (TemplatePredefinedList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim())) != null)
                return true;

            if (TemplateCustomList != null && TemplateCustomList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim())) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Get template by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public ScheduleTemplateDataViewModel GetTemplateByName(string title)
        {
            ScheduleTemplateDataViewModel ret = null;

            ret = TemplatePredefinedList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim()));

            if (ret == null)
                ret = TemplateCustomList.FirstOrDefault(o => o.Title.ToLower().Equals(title.ToLower().Trim()));

            return ret;
        }

        /// <summary>
        /// Sort collection <see cref="TemplateCustomList"/>.
        /// </summary>
        public void SortTemplateCustomList()
        {
            TemplateCustomList = new ObservableCollection<ScheduleTemplateDataViewModel>(TemplateCustomList.OrderBy(o => o.Title));
        }

        /// <summary>
        /// Sort collection <see cref="ItemCustomList"/>
        /// </summary>
        public void SortItemCustomList()
        {
            ItemCustomList = new ObservableCollection<ScheduleItemDataViewModel>(ItemCustomList.OrderBy(o => o.Name));
        }

        /// <summary>
        /// Sort collection <see cref="ItemIgnoredListPresenter"/>
        /// </summary>
        public void SortItemIgnoredList()
        {
            ItemIgnoredListPresenter = new ObservableCollection<ScheduleItemDataViewModel>(ItemIgnoredListPresenter.OrderBy(o => o.Name));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Play the section.
        /// </summary>
        /// <returns></returns>
        private async Task PlayAsync()
        {
            IsRunning = true;

            UpdateTimeInUI("STARTING");
            UpdateTimeTargetAsync();
            mTimer.Start();

            await Task.Delay(1);
        }

        /// <summary>
        /// Stop the section.
        /// </summary>
        /// <returns></returns>
        private async Task StopAsync()
        {
            IsRunning = false;

            mTimer.Stop();
            StopActiveCountdown();
            UpdateTimeInUI("OFF");
            NextItemPresenterList.Clear();
            UnmarkAllAsNext();

            await Task.Delay(1);
        }

        /// <summary>
        /// Check duplicity of item custom list on application load and remove it.
        /// </summary>
        private void CheckItemDuplicityCustom()
        {
            for (int i = 0; i < ItemPredefinedList.Count; i++)
            {
                ItemCustomList.RemoveAll(
                    o => o.Name.ToLower().Trim().Equals(ItemPredefinedList[i].Name.ToLower())
                    );
            }
        }

        /// <summary>
        /// Check duplicity of template custom list on application load and remove it.
        /// </summary>
        private void CheckTemplateDuplicityCustom()
        {

            for (int i = 0; i < TemplatePredefinedList.Count; i++)
            {
                TemplateCustomList.RemoveAll(
                    o => o.Title.ToLower().Trim().Equals(TemplatePredefinedList[i].Title.ToLower())
                    );
            }
        }

        /// <summary>
        /// Unamrk all marked as <see cref="ScheduleTemplateDayTimeDataViewModel.IsMarkedAsNext"/>.
        /// </summary>
        /// <param name="firstOccuranceOnly"></param>
        private void UnmarkAllAsNext(bool firstOccuranceOnly = false)
        {
            // Umark first occurance.
            for (int iDay = 0; iDay < SelectedTemplate.SchedulePresenter.Count; iDay++)
            {
                for (int iTime = 0; iTime < SelectedTemplate.SchedulePresenter[iDay].TimeList.Count; iTime++)
                {
                    if (!SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext)
                        continue;
                    SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = false;
                    if (firstOccuranceOnly)
                        return;
                }
            }
        }

        /// <summary>
        /// Find <see cref="ScheduleTemplateDayTimeDataViewModel"/> in <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/> and mark it.
        /// </summary>
        /// <param name="timeItem"></param>
        private void FindAndMarkAsNew(ScheduleTemplateDayTimeDataViewModel timeItem)
        {
            for (int iDay = 0; iDay < SelectedTemplate.SchedulePresenter.Count; iDay++)
            {
                for (int iTime = 0; iTime < SelectedTemplate.SchedulePresenter[iDay].TimeList.Count; iTime++)
                {
                    if (timeItem.TemporaryID == SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].TemporaryID)
                    {
                        SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="FindAndMarkAsNew"/> and <see cref="UnmarkAllAsNext(bool)"/> methods together in one loop.
        /// Works only for 1 occurance.
        /// </summary>
        /// <param name="timeItem"></param>
        private void FindAndRemarkAsNew(ScheduleTemplateDayTimeDataViewModel timeItem)
        {
            bool doneMark = false;
            bool doneUnmark = false;

            for (int iDay = 0; iDay < SelectedTemplate.SchedulePresenter.Count; iDay++)
            {
                for (int iTime = 0; iTime < SelectedTemplate.SchedulePresenter[iDay].TimeList.Count; iTime++)
                {
                    if (!doneUnmark && SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext)
                    {
                        SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = false;
                        doneUnmark = true;
                    }

                    if (!doneMark && timeItem == null)
                    {
                        doneMark = true;
                    }
                    else if (!doneMark && timeItem.TemporaryID == SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].TemporaryID)
                    {
                        SelectedTemplate.SchedulePresenter[iDay].TimeList[iTime].IsMarkedAsNext = true;
                        doneMark = true;
                    }

                    if (doneMark && doneUnmark)
                        return;
                }
            }
        }

        /// <summary>
        /// TODO.
        /// </summary>
        private void FindAndRemarkIgnored()
        {

        }

        #endregion
    }
}
