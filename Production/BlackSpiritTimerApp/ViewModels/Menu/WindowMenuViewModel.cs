using BlackSpiritTimerApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritTimerApp.ViewModels
{
    class WindowMenuViewModel : BaseViewModel
    {
        public List<WindowMenuItemViewModel> Items { get; set; }

        public int SideMenuWidth { get; set; } = 175;

        public int SideMenuLogoHeight { get; set; } = 112;

        public string Title { get; set; } = "BST";
    }
}
