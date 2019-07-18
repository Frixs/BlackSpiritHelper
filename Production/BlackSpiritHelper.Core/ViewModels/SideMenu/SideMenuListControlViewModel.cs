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
        }

        #endregion
    }
}
