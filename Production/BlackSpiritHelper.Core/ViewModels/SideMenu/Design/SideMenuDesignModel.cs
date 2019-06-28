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
        }

        #endregion
    }
}
