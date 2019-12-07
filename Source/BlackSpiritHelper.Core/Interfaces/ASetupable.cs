using System;

namespace BlackSpiritHelper.Core
{
    public interface ISetupable : IDisposable
    {
        /// <summary>
        /// Anything you need to do after construction.
        /// </summary>
        void Init();

        ///// <summary>
        ///// Anything you need to do before destroy.
        ///// </summary>
        //new void Dispose();
    }
}
