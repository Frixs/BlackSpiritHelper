using BlackSpiritTimerApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritTimerApp.ViewModels
{
    class SideMenuViewModel : BaseViewModel
    {
        /// <summary>
        /// Menu items.
        /// </summary>
        public List<SideMenuItemViewModel> MenuItems { get; set; }
    }
}
