using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class SideMenuListControlDesignModel : SideMenuListControlViewModel
    {
        #region New Instance Getter

        public static SideMenuListControlDesignModel Instance => new SideMenuListControlDesignModel();

        #endregion

        #region Constructor

        public SideMenuListControlDesignModel()
        {
            Items = new List<SideMenuListItemControlViewModel>
            {
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Boss.ToString(),
                    PageEnum = ApplicationPage.Boss,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Timer.ToString(),
                    PageEnum = ApplicationPage.Timer,
                    DataContent = IoC.DataContent.TimerDesignModel,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Watchdog.ToString(),
                    PageEnum = ApplicationPage.Watchdog,
                },
            };
        }

        #endregion
    }
}
