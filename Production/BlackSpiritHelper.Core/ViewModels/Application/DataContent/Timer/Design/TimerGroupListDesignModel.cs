using System;
using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class TimerDesignModel : TimerViewModel
    {
        #region New Instance Getter

        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        [XmlIgnore]
        public static TimerDesignModel Instance
        {
            get
            {
                TimerDesignModel o = new TimerDesignModel();

                // Set default properties while creating a new instance with this property.
                // Thanks to this, you can load saved user data on application start with NEW statement. (default)
                // If there is no user data to load, create a new instance with this property. (our special case)
                o.SetDefaults();

                return o;
            }
        }

        //[XmlIgnore]
        //private static TimerDesignModel mInstance = null;

        //[XmlIgnore]
        //public static TimerDesignModel Instance
        //{
        //    get
        //    {
        //        if (mInstance == null)
        //            mInstance = new TimerDesignModel();

        //        return mInstance;
        //    }
        //    // Use this only to load user settings on application start!
        //    private set
        //    {
        //        mInstance = value;
        //    }
        //}

        #endregion

        #region Constructor

        public TimerDesignModel()
        {
        }

        /// <summary>
        /// Set default properties.
        /// </summary>
        private void SetDefaults()
        {
            TimerGroupViewModel g = AddGroup("Default Group");
            g.AddTimer(new TimerItemViewModel
            {
                GroupID = 0,
                Title = "My First Timer",
                IconTitleShortcut = "MFT",
                IconBackgroundHEX = "1f61cc",
                TimeDuration = new TimeSpan(0, 1, 30),
                CountdownDuration = TimeSpan.FromSeconds(3),
                State = TimerState.Ready,
                IsLoopActive = false,
                ShowInOverlay = false,
            });
        }

        #endregion
    }
}
