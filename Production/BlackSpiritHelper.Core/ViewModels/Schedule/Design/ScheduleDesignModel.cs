using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ScheduleDesignModel : ScheduleViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        [XmlIgnore]
        public static ScheduleDesignModel Instance
        {
            get
            {
                ScheduleDesignModel o = new ScheduleDesignModel();
                return o;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleDesignModel()
        {
        }

        #endregion
    }
}
