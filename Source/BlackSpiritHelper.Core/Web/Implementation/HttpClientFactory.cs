using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper around <see cref="HttpClient"/> to prevent known exceptions.
    /// ===
    /// References:
    /// Discussion: HttpClient work-around solution: https://github.com/aspnet/Extensions/issues/1345
    /// Topic: Effectively using HttpClient: https://www.csantos.me/effectively-using-httpclient/
    /// ---
    /// Socket section with examples: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/sockets
    /// </summary>
    public class HttpClientFactory : IClientFactory<HttpClient>
    {
        #region Protected Static Members

        /// <summary>
        /// Cache of all used Http clients.
        /// string represents URI
        /// short represents number of accesses
        /// </summary>
        protected static readonly ConcurrentDictionary<string, ValuePair<HttpClient, short>> HttpClientCache = new ConcurrentDictionary<string, ValuePair<HttpClient, short>>();

        #endregion

        #region Private Members

        /// <summary>
        /// How many clients should cache cleaning routine leave in the cache at maximum.
        /// </summary>
        private readonly byte mCleanCacheSize = 5;

        /// <summary>
        /// After how many access to <see cref="GetClientForHost(Uri)"/> is <see cref="CleanCache"/> fired.
        /// </summary>
        private readonly byte mCleanCacheAccessLimit = 60;

        /// <summary>
        /// Counter for <see cref="mCleanCacheAccessLimit"/>.
        /// </summary>
        private byte mCleanCacheAccessCounter = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpClientFactory()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (var item in HttpClientCache)
            {
                item.Value.ValueA.CancelPendingRequests();
                item.Value.ValueA.Dispose();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get <see cref="HttpClient"/> for specific host or add a new one.
        /// </summary>
        /// <param name="uri">Specified URI to connect to</param>
        /// <returns></returns>
        public HttpClient GetClientForHost(Uri uri)
        {
            // Format URI
            var key = $"{uri.Scheme}://{uri.DnsSafeHost}:{uri.Port}";

            // Clean cache trigger
            mCleanCacheAccessCounter++;
            if (mCleanCacheAccessCounter > mCleanCacheAccessLimit)
            {
                CleanCache();
                mCleanCacheAccessCounter = 0;
            }

            // Get client
            var kvClient = HttpClientCache.GetOrAdd(key, k =>
            {
                var client = new HttpClient()
                {
                    // Other setup
                };

                var sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLeaseTimeout = 60 * 1000; // 1 minute

                return new ValuePair<HttpClient, short>(client, 0);
            });

            kvClient.ValueB++;
            return kvClient.ValueA;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clean cache from the unused client connections.
        /// </summary>
        private void CleanCache()
        {
            IoC.Logger.Log($"Going to clean {HttpClientCache.Count - mCleanCacheSize} items from cache.", LogLevel.Debug);

            while (HttpClientCache.Count > mCleanCacheSize)
            {
                short maxVal = short.MinValue;
                string maxKey = string.Empty;

                foreach (var item in HttpClientCache)
                {
                    if (item.Value.ValueB > maxVal)
                    {
                        maxVal = item.Value.ValueB;
                        maxKey = item.Key;
                    }
                }

                HttpClientCache.TryRemove(maxKey, out _);
            }
        }

        #endregion
    }
}
