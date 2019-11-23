using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// TODO comment
    /// </summary>
    public abstract class AWatchdogConnection : BaseViewModel
    {
        /// <summary>
        /// TODO comment
        /// </summary>
        public WatchdogFailureActionDataViewModel FailureAction { get; set; } = new WatchdogFailureActionDataViewModel();
    }
}
