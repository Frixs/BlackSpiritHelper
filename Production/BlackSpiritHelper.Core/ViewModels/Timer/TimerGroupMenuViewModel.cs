using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// List of timer groups.
        /// </summary>
        public List<TimerGroupMenuItemViewModel> Groups { get; set; }

        #endregion
    }
}
