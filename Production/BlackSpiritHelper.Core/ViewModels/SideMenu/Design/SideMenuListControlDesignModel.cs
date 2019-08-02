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
                    Title = ApplicationPage.DailyCheck.GetDescription(),
                    PageEnum = ApplicationPage.DailyCheck,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Schedule.GetDescription(),
                    PageEnum = ApplicationPage.Schedule,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Timer.GetDescription(),
                    PageEnum = ApplicationPage.Timer,
                    DataContent = IoC.DataContent.TimerDesignModel,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Watchdog.GetDescription(),
                    PageEnum = ApplicationPage.Watchdog,
                },
            };
        }

        #endregion
    }
}
