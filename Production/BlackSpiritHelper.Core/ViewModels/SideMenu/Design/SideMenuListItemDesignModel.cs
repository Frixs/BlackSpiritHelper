namespace BlackSpiritHelper.Core
{
    public class SideMenuListItemDesignModel : SideMenuListItemViewModel
    {
        #region New Instance Getter

        public static SideMenuListItemDesignModel Instance => new SideMenuListItemDesignModel();

        #endregion

        #region Constructor

        public SideMenuListItemDesignModel()
        {
            Title = "PLACEHOLDER";
            PageEnum = ApplicationPage.Home;
            IsActive = true;
        }

        #endregion
    }
}
