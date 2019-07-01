using System.Collections.Generic;

namespace BlackSpiritHelper.Core
{
    public class TimerListViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// List of timers.
        /// </summary>
        public List<List<TimerListItemViewModel>> Items { get; set; }

        #endregion
    }
}
