using BlackSpiritHelper.Core.Data.Interfaces;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Process Connection wrapper.
    /// Serves to <see cref="WatchdogConnectionWatcherDataViewModel"/>.
    /// </summary>
    public class WatchdogProcessConnectionDataViewModel : AWatchdogConnectionBase
    {
        #region Public Properties

        /// <summary>
        /// Process of this wrapper to handle.
        /// </summary>
        public WatchdogProcessDataViewModel Process { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogProcessConnectionDataViewModel()
        {
        }

        #endregion
    }
}
