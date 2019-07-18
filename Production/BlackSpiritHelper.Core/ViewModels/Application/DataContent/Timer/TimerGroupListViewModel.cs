using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents list of all timer Groups.
    /// Timer Group view model: <see cref="TimerGroupViewModel"/>.
    /// </summary>
    public class TimerGroupListViewModel : DataContentBaseViewModel
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
        public ObservableCollection<TimerGroupViewModel> GroupList { get; set; }

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        [XmlIgnore]
        public bool CanCreateNewGroup => GroupList.Count < AllowedMaxNoOfGroups;

        /// <summary>
        /// TODO xyz
        /// </summary>
        public override bool IsRunning => GroupList.FirstOrDefault(o => o.IsRunning == true) == null ? false : true;


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
            if (!CanCreateNewGroup)
                return null;

            itemTitle = itemTitle.Trim();
            // Validate Inputs.
            if (!TimerGroupViewModel.ValidateGroupInputs(itemTitle))
                return null;

            // Sort Groups by ID.
            GroupList.OrderBy(o => o.ID);

            // Create a new item (Default Group).
            TimerGroupViewModel item = new TimerGroupViewModel
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
        public bool DestroyGroup(TimerGroupViewModel vm)
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
            GroupList = new ObservableCollection<TimerGroupViewModel>(
                GroupList.OrderBy(o => o.Title)
                );
        }

        /// <summary>
        /// Get the group from <see cref="GroupList"/> by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns></returns>
        public TimerGroupViewModel GetGroupByID(sbyte id)
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
