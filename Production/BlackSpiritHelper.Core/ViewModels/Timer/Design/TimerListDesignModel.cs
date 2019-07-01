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
            Items = new List<List<TimerListItemViewModel>>
            {
                new List<TimerListItemViewModel> {
                    new TimerListItemViewModel
                    {
                        GroupID = 0,
                    },
                    new TimerListItemViewModel
                    {
                        GroupID = 0,
                    },
                },
                new List<TimerListItemViewModel> {
                    new TimerListItemViewModel
                    {
                        GroupID = 1,
                    },
                    new TimerListItemViewModel
                    {
                        GroupID = 1,
                    },
                    new TimerListItemViewModel
                    {
                        GroupID = 1,
                    },
                    new TimerListItemViewModel
                    {
                        GroupID = 1,
                    },
                },
            };
        }

        #endregion
    }
}
