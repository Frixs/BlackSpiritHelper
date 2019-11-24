using BlackSpiritHelper.Core.Data.Interfaces;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Internet Connection wrapper.
    /// Serves to <see cref="WatchdogConnectionWatcherDataViewModel"/>.
    /// </summary>
    public class WatchdogInternetConnectionDataViewModel : AWatchdogConnectionBase
    {
        #region Public Properties

        /// <summary>
        /// Address used for check ping connection test.
        /// </summary>
        [XmlIgnore]
        public string PingCheckAddress { get; set; } = "8.8.8.8";

        /// <summary>
        /// Address used for web client opening test.
        /// </summary>
        [XmlIgnore]
        public string ClientCheckAddress { get; set; } = "http://google.com/generate_204";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogInternetConnectionDataViewModel()
        {
        }

        #endregion
    }
}
