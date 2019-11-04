using System;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class WatchdogDataViewModel : DataContentBaseViewModel<WatchdogDataViewModel>
    {
        #region Public Properties

        [XmlIgnore]
        public override bool IsRunning 
        {
            get => false;
            protected set => throw new NotImplementedException();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogDataViewModel()
        {
        }

        protected override void SetDefaultsMethod()
        {
        }

        protected override void SetupMethod()
        {
        }

        protected override void UnsetMethod()
        {
        }

        #endregion
    }
}
