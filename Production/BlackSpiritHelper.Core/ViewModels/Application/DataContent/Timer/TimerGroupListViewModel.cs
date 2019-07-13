using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents list of all timer Groups.
    /// Timer Group view model: <see cref="TimerGroupViewModel"/>.
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
        [XmlIgnore]
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

            // Check limits.
            if (GroupList.Count + 1 > MaxNoOfGroups)
                return null;

            itemTitle = itemTitle.Trim();
            // Validate Inputs.
            if (!ValidateGroupInputs(itemTitle))
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

            // Add.
            GroupList.Add(item);

            // Check to set limits.
            if (GroupList.Count + 1 > MaxNoOfGroups)
                CanCreateNewGroup = false;

            IoC.Logger.Log($"Timer Group '{itemTitle}' added!", LogLevel.Info);
            return item;
        }

        /// <summary>
        /// Delete the group permanently.
        /// </summary>
        /// <param name="vm">The view model of the group you wish to delete.</param>
        /// <returns></returns>
        public bool DeleteGroup(TimerGroupViewModel vm)
        {
            IoC.Logger.Log($"Trying to delete Timer Group '{vm.Title}'...", LogLevel.Debug);

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

            IoC.Logger.Log($"Timer Group '{title}' deleted!", LogLevel.Info);
            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Sort <see cref="GroupList"/> alphabetically.
        /// </summary>
        public void SortGroupList()
        {
            GroupList = new ObservableCollection<TimerGroupViewModel>(
                GroupList.OrderBy(o => o.Title)
                );
        }

        /// <summary>
        /// Check group parameters.
        /// TRUE, if all parameters are OK and the group can be created.
        /// </summary>
        /// <param name="title">The group title.</param>
        /// <returns></returns>
        public bool ValidateGroupInputs(string title)
        {
            title = title.Trim();

            // Check conditions.
            if (title.Length < TimerGroupViewModel.TitleAllowMinChar || title.Length > TimerGroupViewModel.TitleAllowMaxChar)
                return false;

            // Check allowed characters.
            if (!CheckAlphanumericString(title, true, true))
                return false;

            return true;
        }

        /// <summary>
        /// Check if the string contains only letters and numbers.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <param name="underscores">Are underscores allowed?</param>
        /// <param name="spaces">Are spaces allowed?</param>
        /// <returns></returns>
        private bool CheckAlphanumericString(string input, bool underscores = false, bool spaces = false)
        {
            if (underscores && spaces)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9_ ]+$");

            if (underscores)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");

            if (spaces)
                return Regex.IsMatch(input, @"^[a-zA-Z0-9 ]+$");

            return Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");
        }

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
