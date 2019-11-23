using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Windows process structure model.
    /// </summary>
    public class WatchdogProcessDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Process name.
        /// </summary>
        public string Name { get; set; } = "";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogProcessDataViewModel()
        {
        }

        #endregion
    }
}
