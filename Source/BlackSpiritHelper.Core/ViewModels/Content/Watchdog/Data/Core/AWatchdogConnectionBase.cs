using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// Abstract class as interface for all checks in <see cref="WatchdogConnectionWatcherDataViewModel"/>.
    /// </summary>
    public abstract class AWatchdogConnectionBase : BaseViewModel
    {
        #region Abstract Properties

        /// <summary>
        /// Says if the check is selected for checking loop.
        /// </summary>
        public abstract bool IsSelected { get; set; }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// The main method to check conditions.
        /// </summary>
        /// <returns></returns>
        public abstract bool Check();

        #endregion
    }
}
