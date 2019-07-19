using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class OverlayDesignModel : OverlayViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        [XmlIgnore]
        public static OverlayDesignModel Instance
        {
            get
            {
                return new OverlayDesignModel();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OverlayDesignModel()
        {
        }

        #endregion
    }
}
