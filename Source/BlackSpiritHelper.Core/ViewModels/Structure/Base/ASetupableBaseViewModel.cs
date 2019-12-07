using System;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Intended only for *DataViewModel-s to be able to Init/Dispose data sub classes from master data class - <see cref="ADataContentBaseViewModel"/>
    /// </summary>
    public abstract class ASetupableBaseViewModel : BaseViewModel, IDisposable
    {
        #region Public Properties

        /// <summary>
        /// If it is false. Setup has not been called and you can check other loading procedures.
        /// </summary>
        [XmlIgnore]
        public bool mIsInitDoneFlag { get; private set; } = false;

        /// <summary>
        /// Says, if unset has been already done or not.
        /// </summary>
        [XmlIgnore]
        public bool mIsDisposeDoneFlag { get; private set; } = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        /// <param name="parameters">Additional parameters to specify initialization if needed</param>
        public void Init(params object[] parameters)
        {
            if (mIsInitDoneFlag)
                return;
            mIsInitDoneFlag = true;

            InitRoutine(parameters);
        }

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        public void Dispose()
        {
            if (!mIsInitDoneFlag || mIsDisposeDoneFlag)
                return;
            mIsDisposeDoneFlag = true;

            DisposeRoutine();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Anything you need to do after construction.
        /// Intended to be called only once!
        /// </summary>
        /// <param name="parameters">Additional parameters to specify initialization if needed</param>
        protected abstract void InitRoutine(params object[] parameters);

        /// <summary>
        /// Anything you need to do before destroy.
        /// Intended to be called only once!
        /// </summary>
        protected abstract void DisposeRoutine();

        #endregion
    }
}
