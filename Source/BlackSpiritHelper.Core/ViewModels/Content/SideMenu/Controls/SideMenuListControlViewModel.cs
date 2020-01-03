using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class SideMenuListControlViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Menu items.
        /// </summary>
        public List<SideMenuListItemControlViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListControlViewModel()
        {
            Items = new List<SideMenuListItemControlViewModel>
            {
                new SideMenuListItemControlViewModel
                {
                    Title = ApplicationPage.Schedule.GetDescription(),
                    PageEnum = ApplicationPage.Schedule,
                    DataContent = IoC.DataContent.ScheduleData,
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
                    DataContent = IoC.DataContent.WatchdogData,
                },
            };
        }

        #endregion
    }
}
