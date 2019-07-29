using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class DataContentBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// If it is false. Setup has not been called and you can check other loading procedures.
        /// </summary>
        private bool mIsSetupDoneFlag = false;

        /// <summary>
        /// TRUE, if the default values should be appended.
        /// </summary>
        protected bool mInitWithDefaultsFlag = false;

        /// <summary>
        /// Says if the Timer content is running.
        /// TRUE = at least 1 timer is running.
        /// FALSE = No timer is running at all.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Everythng you need to do after construction.
        /// </summary>
        public void Setup()
        {
            if (mIsSetupDoneFlag)
                return;
            mIsSetupDoneFlag = true;

            SetupMethod();
        }

        /// <summary>
        /// Everythng you need to do after construction.
        /// </summary>
        protected abstract void SetupMethod();

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        public void SetDefaults()
        {
            if (!mInitWithDefaultsFlag)
                return;
            mInitWithDefaultsFlag = false;

            IoC.Logger.Log("Setting default values...", LogLevel.Debug);
            SetDefaultsMethod();
        }

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        protected abstract void SetDefaultsMethod();
    }
}
