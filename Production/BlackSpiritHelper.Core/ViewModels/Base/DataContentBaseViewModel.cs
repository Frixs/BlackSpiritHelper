using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class DataContentBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Says if the Timer content is running.
        /// TRUE = at least 1 timer is running.
        /// FALSE = No timer is running at all.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Setup on load.
        /// </summary>
        public abstract void Setup();
    }
}
