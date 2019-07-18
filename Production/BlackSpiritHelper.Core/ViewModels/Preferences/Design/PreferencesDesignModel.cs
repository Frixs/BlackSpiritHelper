using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Global data content DesignModel.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class PreferencesDesignModel : PreferencesViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        [XmlIgnore]
        public static PreferencesDesignModel Instance
        {
            get
            {
                return new PreferencesDesignModel();
            }
        }

        #endregion

        #region Constructor

        public PreferencesDesignModel()
        {
        }

        #endregion
    }
}
