using System;

namespace BlackSpiritHelper.Core
{
    public abstract class ASetupableBaseViewModel : BaseViewModel, IDisposable
    {
        #region Private Members

        /// <summary>
        /// If it is false. Setup has not been called and you can check other loading procedures.
        /// </summary>
        private bool mIsInitDoneFlag = false;

        /// <summary>
        /// Says, if unset has been already done or not.
        /// </summary>
        private bool mIsDisposeDoneFlag = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        public void Init()
        {
            if (mIsInitDoneFlag)
                return;
            mIsInitDoneFlag = true;

            InitRoutine();
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
        /// Can be run only once.
        /// </summary>
        protected abstract void InitRoutine();

        /// <summary>
        /// Anything you need to do before destroy.
        /// Can be run only once.
        /// </summary>
        protected abstract void DisposeRoutine();

        #endregion
    }
}
