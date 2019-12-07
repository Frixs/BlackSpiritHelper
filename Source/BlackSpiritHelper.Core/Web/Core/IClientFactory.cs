using System;

namespace BlackSpiritHelper.Core
{
    public interface IClientFactory<T> : IDisposable
        where T : new()
    {
        #region Public Methods

        /// <summary>
        /// Get <see cref="HttpClient"/> for specific host or add a new one.
        /// </summary>
        /// <param name="uri">Specified URI to connect to</param>
        /// <returns></returns>
        T GetClientForHost(Uri uri);

        #endregion
    }
}
