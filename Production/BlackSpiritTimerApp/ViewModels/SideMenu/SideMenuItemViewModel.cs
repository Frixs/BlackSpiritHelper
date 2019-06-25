using BlackSpiritTimerApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritTimerApp.ViewModels
{
    class SideMenuItemViewModel
    {
        #region Public Properties

        /// <summary>
        /// Title of the menu item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Enum <see cref="ApplicationPage"/>.
        /// </summary>
        public ApplicationPage PageEnum { get; set; }

        /// <summary>
        /// Says, if any timer is running in the page.
        /// </summary>
        public bool IsPaused { get; set; }

        #endregion

        #region Constructor

        public SideMenuItemViewModel()
        {

        }

        #endregion
    }
}
