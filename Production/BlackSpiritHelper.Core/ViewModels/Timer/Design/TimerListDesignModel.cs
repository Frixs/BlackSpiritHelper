using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class TimerListDesignModel : TimerListViewModel
    {
        #region Singleton

        public static TimerListDesignModel Instance => new TimerListDesignModel();

        #endregion

        #region Constructor

        public TimerListDesignModel()
        {
            AddItem(0, new TimerListItemViewModel
            {
                GroupID = 0,
            });
            AddItem(0, new TimerListItemViewModel
            {
                GroupID = 0,
            });

            AddItem(1, new TimerListItemViewModel
            {
                GroupID = 1,
            });
            AddItem(1, new TimerListItemViewModel
            {
                GroupID = 1,
            });
            AddItem(1, new TimerListItemViewModel
            {
                GroupID = 1,
            });
            AddItem(1, new TimerListItemViewModel
            {
                GroupID = 1,
            });
        }

        #endregion
    }
}
