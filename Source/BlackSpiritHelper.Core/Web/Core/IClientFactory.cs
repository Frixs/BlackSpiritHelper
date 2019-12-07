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
        /// <param name="parameters">
        ///     1.  int     Timeout in milliseconds, 0 = default
        /// </param>
        /// <returns></returns>
        T GetClientForHost(Uri uri, params object[] parameters);

        #endregion
    }
}
