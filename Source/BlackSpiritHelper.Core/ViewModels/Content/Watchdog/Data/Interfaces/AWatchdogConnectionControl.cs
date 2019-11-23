using System.Xml.Serialization;

namespace BlackSpiritHelper.Core.Data.Interfaces
{
    /// <summary>
    /// TODO comment
    /// </summary>
    public abstract class AWatchdogConnectionControl : BaseViewModel
    {
        /// <summary>
        /// TODO comment
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }
    }
}
