using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class SideMenuListDesignModel : SideMenuListViewModel
    {
        #region New Instance Getter

        public static SideMenuListDesignModel Instance => new SideMenuListDesignModel();

        #endregion

        #region Constructor

        public SideMenuListDesignModel()
        {
            Items = new List<SideMenuListItemViewModel>
            {
                new SideMenuListItemViewModel
                {
                    Title = ApplicationPage.Boss.ToString(),
                    PageEnum = ApplicationPage.Boss,
                },
                new SideMenuListItemViewModel
                {
                    Title = ApplicationPage.Timer.ToString(),
                    PageEnum = ApplicationPage.Timer,
                    DataContent = IoC.DataContent.TimerGroupListDesignModel,
                },
                new SideMenuListItemViewModel
                {
                    Title = ApplicationPage.Watchdog.ToString(),
                    PageEnum = ApplicationPage.Watchdog,
                },
            };
        }

        #endregion
    }
}
