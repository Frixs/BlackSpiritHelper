using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// General base ViewModel for root user data.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class DataContentBaseViewModel : BaseViewModel, ISetupable
    {
        #region Private Members

        /// <summary>
        /// If it is false. Setup has not been called and you can check other loading procedures.
        /// </summary>
        private bool mIsSetupDoneFlag = false;

        /// <summary>
        /// Says, if unset has been already done or not.
        /// </summary>
        private bool mIsUnsetDoneFlag = false;

        #endregion

        #region Protected Members

        /// <summary>
        /// TRUE, Set flag up for creating default properties (calling <see cref="SetDefaults"/>) while creating a new instance with this property.
        /// Thanks to this, constructor is free for loading from user settings 
        /// and you can manually call this if user settings does not exists.
        /// </summary>
        protected bool mInitWithDefaultsFlag = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says, if a section is running.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        public void Init()
        {
            if (mIsSetupDoneFlag)
                return;
            mIsSetupDoneFlag = true;

            InitRoutine();
        }

        /// <summary>
        /// Anything you need to do after construction.
        /// Can be run only once.
        /// </summary>
        protected abstract void InitRoutine();

        /// <summary>
        /// Set default values into this instance.
        /// </summary>
        public void SetDefaults()
        {
            if (!mInitWithDefaultsFlag)
                return;
            mInitWithDefaultsFlag = false;

            IoC.Logger.Log("Setting default values...", LogLevel.Debug);
            SetDefaultsRoutine();
        }

        /// <summary>
        /// Set default values into this instance.
        /// Can be run only once.
        /// </summary>
        protected abstract void SetDefaultsRoutine();

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        public void Dispose()
        {
            if (!mIsSetupDoneFlag || mIsUnsetDoneFlag)
                return;
            mIsUnsetDoneFlag = true;

            DisposeRoutine();
        }

        /// <summary>
        /// Anything you need to do before destroy.
        /// Can be run only once.
        /// </summary>
        protected abstract void DisposeRoutine();

        #endregion
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
