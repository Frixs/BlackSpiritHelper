using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritTimerApp.ViewModels
{
    class WindowMenuDesignModel : WindowMenuViewModel
    {
        #region Singleton

        public static WindowMenuDesignModel Instance => new WindowMenuDesignModel();

        #endregion

        #region Constructor

        public WindowMenuDesignModel()
        {
            Items = new List<WindowMenuItemViewModel>
            {
                new WindowMenuItemViewModel
                {
                    Name = "První",
                    Message = "Message"
                },
                new WindowMenuItemViewModel
                {
                    Name = "Druhý",
                    Message = "Zpráva"
                },
            };
        }

        #endregion
    }
}
