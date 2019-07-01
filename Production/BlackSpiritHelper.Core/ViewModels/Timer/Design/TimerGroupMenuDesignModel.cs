using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuDesignModel : TimerGroupMenuViewModel
    {
        #region Singleton

        public static TimerGroupMenuDesignModel Instance => new TimerGroupMenuDesignModel();

        #endregion

        #region Constructor

        public TimerGroupMenuDesignModel()
        {
            Groups = new List<TimerGroupMenuItemViewModel>
            {
                new TimerGroupMenuItemViewModel
                {
                    GroupID = 0,
                },
            };
        }

        #endregion
    }
}
