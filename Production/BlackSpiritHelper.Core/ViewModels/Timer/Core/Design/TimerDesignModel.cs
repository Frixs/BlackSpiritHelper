namespace BlackSpiritHelper.Core
{
    public class TimerDesignModel : TimerViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public static TimerDesignModel Instance
        {
            get
            {
                TimerDesignModel o = new TimerDesignModel
                {
                    // Set default properties while creating a new instance with this property.
                    // Thanks to this, you can load saved user data on application start with NEW statement. (default)
                    // If there is no user data to load, create a new instance with this property. (our special case)
                    mInitWithDefaultsFlag = true
                };

                return o;
            }
        }

        #endregion

        #region Constructor

        public TimerDesignModel()
        {
        }

        #endregion
    }
}
