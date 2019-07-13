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
                    Title = Properties.Strings.PageTitle1,
                    PageEnum = ApplicationPage.Timer,
                    IsActive = true,
                },
                new SideMenuListItemViewModel
                {
                    Title = Properties.Strings.PageTitle3,
                    PageEnum = ApplicationPage.Boss,
                    IsActive = true,
                },
                new SideMenuListItemViewModel
                {
                    Title = Properties.Strings.PageTitle2,
                    PageEnum = ApplicationPage.Watchdog,
                    IsActive = false,
                },
            };
        }

        #endregion
    }
}
