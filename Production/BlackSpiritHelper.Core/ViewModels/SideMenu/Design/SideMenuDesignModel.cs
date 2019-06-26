using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class SideMenuDesignModel : SideMenuViewModel
    {
        #region Singleton

        public static SideMenuDesignModel Instance => new SideMenuDesignModel();

        #endregion

        #region Constructor

        public SideMenuDesignModel()
        {
            MenuItems = new List<SideMenuItemViewModel>
            {
                new SideMenuItemViewModel
                {
                    Title = Properties.Strings.PageTitle1,
                    PageEnum = ApplicationPage.Combat,
                    IsPaused = true,
                },
                new SideMenuItemViewModel
                {
                    Title = Properties.Strings.PageTitle2,
                    PageEnum = ApplicationPage.Lifeskill,
                    IsPaused = true,
                },
                new SideMenuItemViewModel
                {
                    Title = Properties.Strings.PageTitle3,
                    PageEnum = ApplicationPage.Boss,
                    IsPaused = true,
                },
            };
        }

        #endregion
    }
}
