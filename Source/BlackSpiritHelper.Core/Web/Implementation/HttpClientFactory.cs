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
        protected static readonly ConcurrentDictionary<ValuePair<string, object[]>, ValuePair<HttpClient, short>> HttpClientCache = new ConcurrentDictionary<ValuePair<string, object[]>, ValuePair<HttpClient, short>>();

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
        /// <param name="parameters">
        ///     1.  int     Timeout in milliseconds, 0 = default
        /// </param>
        /// <returns></returns>
        public HttpClient GetClientForHost(Uri uri, params object[] parameters)
        {
            // Format URI
            var addressPattern = $"{uri.Scheme}://{uri.DnsSafeHost}:{uri.Port}";

            // Search key
            var key = new ValuePair<string, object[]>(addressPattern, parameters);

            // Trigger to clean cache
            mCleanCacheAccessCounter++;
            if (mCleanCacheAccessCounter > mCleanCacheAccessLimit)
            {
                CleanCache();
                mCleanCacheAccessCounter = 0;
            }

            // Get client
            var kvClient = HttpClientCache.GetOrAdd(key, k =>
            {
                var client = new HttpClient();
                SetClientParameters(client, parameters);

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
        /// Set parameters for the client.
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="parameters">The same parameters as <see cref="GetClientForHost(Uri, object[])"/> method has.</param>
        private void SetClientParameters(HttpClient client, params object[] parameters)
        {
            // Timeout.
            var timeout = 0 < parameters.Length ? (int)parameters[0] : 0;
            client.Timeout = timeout == 0 ? client.Timeout : TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Clean cache from the unused client connections.
        /// </summary>
        private void CleanCache()
        {
            // Log it.
            IoC.Logger.Log($"Going to clean items from cache. Current size of the cache: {HttpClientCache.Count}", LogLevel.Debug);

            // Find only one time used clients or without any use.
            var itemsToRemove = new List<KeyValuePair<ValuePair<string, object[]>, ValuePair<HttpClient, short>>>();
            foreach (var item in HttpClientCache)
            {
                if (item.Value.ValueB > 1)
                    continue;
                itemsToRemove.Add(item);
            }
            // Remove all found clients.
            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                ValuePair<HttpClient, short> removedVal;
                if (HttpClientCache.TryRemove(itemsToRemove[i].Key, out removedVal))
                    removedVal.ValueA.Dispose();
            }

            // Find and remove least used clients and keep the list in proper size.
            var count = HttpClientCache.Count;
            while (count > mCleanCacheSize)
            {
                short maxVal = short.MinValue;
                ValuePair<string, object[]> maxKey = null;

                foreach (var item in HttpClientCache)
                {
                    if (item.Value.ValueB >= maxVal)
                    {
                        maxVal = item.Value.ValueB;
                        maxKey = item.Key;
                    }
                }

                // Remove.
                ValuePair<HttpClient, short> removedVal;
                if (HttpClientCache.TryRemove(maxKey, out removedVal))
                    removedVal.ValueA.Dispose();
                count = HttpClientCache.Count;
            }

            // Set all values to zero for the next check.
            foreach (var item in HttpClientCache)
                item.Value.ValueB = 0;

            // Log it.
            IoC.Logger.Log($"Size of the cache after cleaning: {HttpClientCache.Count}", LogLevel.Debug);
        }

        #endregion
    }
}
