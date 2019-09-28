namespace BlackSpiritHelper.Core
{
    public class OverlayDesignModel : OverlayViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
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
