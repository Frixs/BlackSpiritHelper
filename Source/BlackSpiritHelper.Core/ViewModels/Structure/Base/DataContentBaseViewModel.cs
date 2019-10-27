using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// General base ViewModel for root user data.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class DataContentBaseViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// If it is false. Setup has not been called and you can check other loading procedures.
        /// </summary>
        private bool mIsSetupDoneFlag = false;

        #endregion

        #region Protected Members

        /// <summary>
        /// TRUE, Set flag up for creating default properties (calling <see cref="SetDefaults"/>) while creating a new instance with this property.
        /// Thanks to this, constructor is free for loading from user settings 
        /// and you can manually call this if user settings does not exists.
        /// </summary>
        protected bool mInitWithDefaultsFlag = false;

        /// <summary>
        /// Says, if unset has been already done or not.
        /// </summary>
        protected bool mIsUnsetDoneFlag = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says, if a section is running.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        #endregion

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

    /// <summary>
    /// Base ViewModel for root user data.
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class DataContentBaseViewModel<VM> : DataContentBaseViewModel
        where VM : DataContentBaseViewModel<VM>, new()
    {
        /// <summary>
        /// Create a new data instance.
        /// This is different from creating the instance through normal constructor. 
        /// This will additionally mark the instance to run additional procedures after creation in <see cref="ApplicationDataContent"/>.
        /// </summary>
        public static VM NewDataInstance
        {
            get
            {
                return new VM
                {
                    mInitWithDefaultsFlag = true
                };
            }
        }
    }
}
