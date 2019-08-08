using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
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
        /// Says, if unset has been already done or not.
        /// </summary>
        protected bool mIsUnsetDoneFlag = false;

        /// <summary>
        /// Says, if a section is running.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        public void Setup()
        {
            if (mIsSetupDoneFlag)
                return;
            mIsSetupDoneFlag = true;

            SetupMethod();
        }

        /// <summary>
        /// Anything you need to do after construction.
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

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        public void Unset()
        {
            if (!mIsSetupDoneFlag || mIsUnsetDoneFlag)
                return;
            mIsUnsetDoneFlag = true;

            UnsetMethod();
        }

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        protected abstract void UnsetMethod();
    }
}
