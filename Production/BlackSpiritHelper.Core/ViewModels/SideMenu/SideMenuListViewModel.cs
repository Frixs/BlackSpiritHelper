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

        #region Commands

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        /// <summary>
        /// Create Windows commands.
        /// </summary>
        private void CreateCommands()
        {
        }
    }
}
