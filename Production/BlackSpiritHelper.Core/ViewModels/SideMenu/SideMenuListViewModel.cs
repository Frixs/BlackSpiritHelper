using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class SideMenuListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Menu items.
        /// </summary>
        public List<SideMenuListItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListViewModel()
        {
        }

        #endregion
    }
}
