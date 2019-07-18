using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class DataContentBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// TODO xyz
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; }
    }
}
