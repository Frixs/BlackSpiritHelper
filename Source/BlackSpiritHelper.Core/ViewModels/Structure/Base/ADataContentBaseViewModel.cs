using System.Configuration;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// General base ViewModel for root user data.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class ADataContentBaseViewModel : ASetupableBaseViewModel
    {
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
        /// Indicates the section is running.
        /// </summary>
        [XmlIgnore]
        public abstract bool IsRunning { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set default values into this instance only for the first call of this method.
        /// By default, this method will not proceed unless the <see cref="mInitWithDefaultsFlag"/> is manually set ON.
        /// </summary>
        /// <remarks>
        ///     It is called only when <see cref="mInitWithDefaultsFlag"/> is initialized as TRUE
        /// </remarks>
        public void SetDefaults()
        {
            if (!mInitWithDefaultsFlag)
                return;
            mInitWithDefaultsFlag = false;

            IoC.Logger.Log("Setting default values...", LogLevel.Debug);
            SetDefaultsRoutine();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Default settings that should be set if a completely new instance is intialized without any previous user settings.
        /// It should not be called internally/manually but only by <see cref="SetDefaults"/>
        /// </summary>
        protected abstract void SetDefaultsRoutine();

        #endregion
    }

    /// <summary>
    /// Base ViewModel for root user data.
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public abstract class ADataContentBaseViewModel<VM> : ADataContentBaseViewModel
        where VM : ADataContentBaseViewModel<VM>, new()
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
