using System;

namespace BlackSpiritHelper.Core
{
    public interface IClientFactory<T> : IDisposable
        where T : new()
    {
        #region Publi Properties

        /// <summary>
        /// Major client.
        /// </summary>
        T Client { get; }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// New client instance.
        /// </summary>
        /// <returns></returns>
        T NewClient();

        #endregion
    }
}
