namespace BlackSpiritHelper.Core
{
    public class SideMenuListItemControlDesignModel : SideMenuListItemControlViewModel
    {
        #region New Instance Getter

        public static SideMenuListItemControlDesignModel Instance => new SideMenuListItemControlDesignModel();

        #endregion

        #region Constructor

        public SideMenuListItemControlDesignModel()
        {
            Title = "PLACEHOLDER";
            PageEnum = ApplicationPage.Home;
        }

        #endregion
    }
}
