using System;

namespace BlackSpiritHelper.Core
{
    public abstract class ASetupable : IDisposable
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

        #region Public Methods

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
        /// Anything you need to do before destroy.
        /// </summary>
        public void Dispose()
        {
            if (!mIsSetupDoneFlag || mIsUnsetDoneFlag)
                return;
            mIsUnsetDoneFlag = true;

            DisposeRoutine();
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        protected abstract void InitRoutine();

        /// <summary>
        /// Anything you need to do before destroy.
        /// </summary>
        protected abstract void DisposeRoutine(); 

        #endregion
    }
}
