namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Global data content DesignModel.
    /// </summary>
    public class PreferencesDesignModel : PreferencesViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
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
