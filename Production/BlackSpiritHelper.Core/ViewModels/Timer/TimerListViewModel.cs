using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlackSpiritHelper.Core
{
    public class TimerListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// List of timers within groups.
        /// </summary>
        public Dictionary<TimerGroupMenuItemViewModel, List<TimerListItemViewModel>> TimerDictionary { get; protected set; }

        /// <summary>
        /// Max number of timers that can be created in the group.
        /// </summary>
        public byte MaxNoOfItems { get; private set; } = 10;

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        public bool CanCreateNewItem { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TimerListViewModel()
        {
            TimerDictionary = new Dictionary<TimerGroupMenuItemViewModel, List<TimerListItemViewModel>>();
        }

        #endregion

        #region Public Metohds

        /// <summary>
        /// Add timer item to specific group..
        /// </summary>
        /// <param name="groupId">Group ID.</param>
        /// <param name="item">The item.</param>
        public bool AddItem(byte groupId, TimerListItemViewModel item)
        {
            if (item == null)
                return false;

            TimerGroupMenuItemViewModel g = TimerGroupMenuDesignModel.Instance.Groups.Find(o => o.ID == groupId);
            if (g == null)
                return false;

            // TODO: sync handle.
            TimerDictionary[g].Add(item);

            // Solve limits.
            if (TimerDictionary[g].Count >= MaxNoOfItems)
                CanCreateNewItem = false;
            else
                CanCreateNewItem = true;

            return true;
        }

        #endregion
    }
}
