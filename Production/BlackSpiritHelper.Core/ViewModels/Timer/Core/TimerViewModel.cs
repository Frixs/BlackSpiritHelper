using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents list of all timer Groups.
    /// Timer Group view model: <see cref="TimerGroupDataViewModel"/>.
    /// </summary>
    public class TimerViewModel : DataContentBaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of groups that can be created.
        /// </summary>
        public static byte AllowedMaxNoOfGroups { get; private set; } = 5;

        #endregion

        #region Public Properties

        /// <summary>
        /// List of timer groups.
        /// </summary>
        public ObservableCollection<TimerGroupDataViewModel> GroupList { get; set; }

        /// <summary>
        /// 1st notification time.
        /// </summary>
        public int TimerNotificationTime1 { get; set; } = 50;

        /// <summary>
        /// 1st notification time. Property to load value from user settings on application load.
        /// </summary>
        [XmlIgnore]
        public double TimerNotificationTime1Value
        {
            get => TimerNotificationTime1;
            set
            {
                if (IsRunning)
                    return;

                TimerNotificationTime1 = (int)value;
            }
        }

        /// <summary>
        /// 2nd notification time.
        /// </summary>
        public int TimerNotificationTime2 { get; set; } = 15;

        /// <summary>
        /// 2nd notification time. Property to load value from user settings on application load.
        /// </summary>
        [XmlIgnore]
        public double TimerNotificationTime2Value
        {
            get => TimerNotificationTime2;
            set
            {
                if (IsRunning)
                    return;

                TimerNotificationTime2 = (int)value;
            }
        }

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        [XmlIgnore]
        public bool CanCreateNewGroup => GroupList.Count < AllowedMaxNoOfGroups;

        /// <summary>
        /// Says if the Timer content is running.
        /// TRUE = at least 1 timer is running.
        /// FALSE = No timer is running at all.
        /// </summary>
        [XmlIgnore]
        public override bool IsRunning
        {
            get => GroupList.FirstOrDefault(o => o.IsRunning == true) == null ? false : true;
            protected set => throw new NotImplementedException();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TimerViewModel()
        {
            GroupList = new ObservableCollection<TimerGroupDataViewModel>();
        }

        /// <summary>
        /// Everythng you need to do after construction.
        /// </summary>
        protected override void SetupMethod()
        {
            // This is initialization setup after loading from user settings.
            // We need to run setup method manually.
            foreach (TimerGroupDataViewModel g in GroupList)
                foreach (TimerItemDataViewModel t in g.TimerList)
                    t.Setup();
        }

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        protected override void SetDefaultsMethod()
        {
            TimerGroupDataViewModel g = AddGroup("Default Group");
            g.AddTimer(new TimerItemDataViewModel
            {
                GroupID = 0,
                Title = "My First Timer",
                IconTitleShortcut = "BSH",
                IconBackgroundHEX = "820808",
                TimeDuration = new TimeSpan(0, 1, 30),
                CountdownDuration = TimeSpan.FromSeconds(3),
                State = TimerState.Ready,
                IsLoopActive = false,
                ShowInOverlay = true,
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new group.
        /// </summary>
        /// <param name="itemTitle">The group item title.</param>
        /// <returns></returns>
        public TimerGroupDataViewModel AddGroup(string itemTitle)
        {
            IoC.Logger.Log($"Trying to add Timer Group '{itemTitle}'...", LogLevel.Debug);

            // Check limits.
            if (!CanCreateNewGroup)
                return null;

            itemTitle = itemTitle.Trim();
            // Validate Inputs.
            if (!TimerGroupDataViewModel.ValidateGroupInputs(itemTitle))
                return null;

            // Sort Groups by ID.
            GroupList.OrderBy(o => o.ID);

            // Create a new item (Default Group).
            TimerGroupDataViewModel item = new TimerGroupDataViewModel
            {
                ID = FindNewID(0, (sbyte)(GroupList.Count - 1)),
                Title = itemTitle,
            };

            // Add.
            GroupList.Add(item);

            // Update properties.
            OnPropertyChanged(nameof(CanCreateNewGroup));

            IoC.Logger.Log($"Timer Group '{itemTitle}' added!", LogLevel.Info);
            return item;
        }

        /// <summary>
        /// Dispose/delete the group.
        /// </summary>
        /// <param name="vm">The view model of the group you wish to delete.</param>
        /// <returns></returns>
        public bool DestroyGroup(TimerGroupDataViewModel vm)
        {
            IoC.Logger.Log($"Trying to destroy Timer Group '{vm.Title}'...", LogLevel.Debug);

            if (vm == null)
                return false;

            // CHeck it does not contain any timer.
            if (vm.TimerList.Count > 0)
                return false;

            // Check it is not the last group. There should be always at least 1 group.
            if (GroupList.Count <= 1)
                return false;

            var title = vm.Title;
            // Remove the group from the list.
            if (!GroupList.Remove(vm))
                return false;

            // Destroy reference to group instance.
            vm = null;

            // Release GC.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Update properties.
            OnPropertyChanged(nameof(CanCreateNewGroup));

            IoC.Logger.Log($"Timer Group '{title}' destroyed!", LogLevel.Info);
            return true;
        }

        /// <summary>
        /// Sort <see cref="GroupList"/> alphabetically.
        /// </summary>
        public void SortGroupList()
        {
            GroupList = new ObservableCollection<TimerGroupDataViewModel>(
                GroupList.OrderBy(o => o.Title)
                );
        }

        /// <summary>
        /// Get the group from <see cref="GroupList"/> by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns></returns>
        public TimerGroupDataViewModel GetGroupByID(sbyte id)
        {
            return GroupList.FirstOrDefault(o => o.ID == id);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Find the smallest missing key in the list as a new representation for a new group ID.
        /// </summary>
        /// <param name="start">The start index of list.</param>
        /// <param name="end">The end index of list.</param>
        /// <returns></returns>
        private sbyte FindNewID(sbyte start, sbyte end)
        {
            if (start > end)
                return (sbyte)(end + 1);

            if (start != GroupList[start].ID)
                return start;

            sbyte mid = (sbyte)((start + end) / 2);

            // Left half has all elements from 0 to mid 
            if (GroupList[mid].ID == mid)
                return FindNewID((sbyte)(mid + 1), end);

            return FindNewID(start, mid);
        }

        #endregion
    }
}
