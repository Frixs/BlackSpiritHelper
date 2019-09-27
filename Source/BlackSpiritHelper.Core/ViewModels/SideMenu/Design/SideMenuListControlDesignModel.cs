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
                    // TODO:SectionInit: DailyCheck add data content.
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Schedule.GetDescription(),
                    PageEnum = ApplicationPage.Schedule,
                    DataContent = IoC.DataContent.ScheduleDesignModel,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Timer.GetDescription(),
                    PageEnum = ApplicationPage.Timer,
                    DataContent = IoC.DataContent.TimerData,
                },
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Watchdog.GetDescription(),
                    PageEnum = ApplicationPage.Watchdog,
                    // TODO:SectionInit: Watchdog add data content.
                },
            };
        }

        #endregion
    }
}
