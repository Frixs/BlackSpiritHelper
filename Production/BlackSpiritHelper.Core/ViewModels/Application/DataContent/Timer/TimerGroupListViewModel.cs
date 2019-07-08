using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents list of groups.
    /// </summary>
    public class TimerGroupListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// List of timer groups.
        /// </summary>
        public ObservableCollection<TimerGroupViewModel> GroupList { get; set; }

        /// <summary>
        /// Max number of groups that can be created.
        /// </summary>
        public byte MaxNoOfGroups { get; private set; } = 5;

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        public bool CanCreateNewGroup { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TimerGroupListViewModel()
        {
            GroupList = new ObservableCollection<TimerGroupViewModel>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new group.
        /// </summary>
        /// <param name="itemTitle">The group item title.</param>
        /// <returns></returns>
        public TimerGroupViewModel AddGroup(string itemTitle)
        {
            IoC.Logger.Log($"Trying to add Timer Group '{itemTitle}'...", LogLevel.Debug);

            itemTitle = itemTitle.Trim();
            // Check conditions.
            if (itemTitle.Length < TimerGroupViewModel.TitleAllowMinChar && itemTitle.Length > TimerGroupViewModel.TitleAllowMaxChar)
                return null;

            // Check limits.
            if (GroupList.Count + 1 > MaxNoOfGroups)
                return null;

            // Sort Groups by ID.
            GroupList.OrderBy(o => o.ID);

            // Create a new item.
            TimerGroupViewModel item = new TimerGroupViewModel
            {
                ID = (byte)FindNewID(0, GroupList.Count - 1),
                Title = itemTitle,
                IsRunning = false,
                CanCreateNewTimer = false,
            };

            // TODO: Test Timers.
            item.AddTimer(new TimerItemViewModel
            {
                GroupID = 0,
                Title = "My Timer",
                IconTitleShortcut = "NT",
                IconBackgroundHEX = "FA2C9B",
                TimeFormat = "00:02:30",
                CountdownDuration = TimeSpan.FromSeconds(3),
                State = TimerState.Ready,
                IsRunning = true,
                IsLoopActive = false,
                IsWarningTime = false,
            });
            item.AddTimer(new TimerItemViewModel
            {
                GroupID = 0,
                Title = "Another One",
                IconTitleShortcut = "AnO",
                IconBackgroundHEX = "002F0B",
                TimeFormat = "01:15:21",
                CountdownDuration = TimeSpan.FromSeconds(0),
                State = TimerState.Ready,
                IsRunning = false,
                IsLoopActive = true,
                IsWarningTime = false,
            });

            GroupList.Add(item);

            // Check to set limits.
            if (GroupList.Count + 1 > MaxNoOfGroups)
                CanCreateNewGroup = false;

            IoC.Logger.Log($"Timer Group '{itemTitle}' added!", LogLevel.Info);
            return item;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Find the smallest missing key in the list as a new representation for a new group ID.
        /// </summary>
        /// <param name="start">The start index of list.</param>
        /// <param name="end">The end index of list.</param>
        /// <returns></returns>
        private int FindNewID(int start, int end)
        {
            if (start > end)
                return end + 1;

            if (start != GroupList[start].ID)
                return start;

            int mid = (start + end) / 2;

            // Left half has all elements from 0 to mid 
            if (GroupList[mid].ID == mid)
                return FindNewID(mid + 1, end);

            return FindNewID(start, mid);
        }

        #endregion
    }
}
