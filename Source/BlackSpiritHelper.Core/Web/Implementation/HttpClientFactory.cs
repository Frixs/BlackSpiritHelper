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
    /// Generally, we ave list of clients we use. If we ask for next connection we take client from our list. 
    /// Each connection represents combination of address and its parameters (e.g. timeout etc.). 
    /// If we look for client from our list we choose based on similarity of our connection.
    /// E.g. If we want to connect to google.com we look for google.com with the same parameters in the list.
    /// Because we can have 2 clients pointing to google.com, but with different parameters.
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
        /// Key is representation of URI and parameters need to set the client
        /// short represents number of accesses
        /// </summary>
        protected static readonly ConcurrentDictionary<KeyParams, ValuePair<HttpClient, short>> mHttpClientCache = new ConcurrentDictionary<KeyParams, ValuePair<HttpClient, short>>();

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
            foreach (var item in mHttpClientCache)
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

            // Search key - more in class desription
            var key = new KeyParams(addressPattern, parameters);

            // Trigger to clean cache
            mCleanCacheAccessCounter++;
            if (mCleanCacheAccessCounter > mCleanCacheAccessLimit)
            {
                CleanCache();
                mCleanCacheAccessCounter = 0;
            }

            // Get client
            var kvClient = mHttpClientCache.GetOrAdd(key, k =>
            {
                var client = new HttpClient();
                SetClientParameters(client, key);

                var sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLeaseTimeout = 60 * 1000; // 1 minute

                return new ValuePair<HttpClient, short>(client, 0);
            });

            // Increment this use
            kvClient.ValueB++;

            // Debug print
            //IoC.Logger.Log($"CACHE ITEMS: >>>>>>>>>>", LogLevel.Debug);
            //foreach (var item in mHttpClientCache)
            //    IoC.Logger.Log($"HashCode: {item.Key.GetHashCode()}\t<{item.Key.Address}, [{item.Key.Timeout}]>, <..., {item.Value.ValueB}>", LogLevel.Debug);
            //IoC.Logger.Log($"<<<<<<<<<<", LogLevel.Debug);

            // Return client
            return kvClient.ValueA;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set initial client parameters.
        /// </summary>
        private void SetClientParameters(HttpClient client, KeyParams key)
        {
            // Timeout.
            client.Timeout = key.Timeout == 0 ? client.Timeout : TimeSpan.FromMilliseconds(key.Timeout);
        }

        /// <summary>
        /// Clean cache from the unused client connections.
        /// </summary>
        private void CleanCache()
        {
            // Log it.
            IoC.Logger.Log($"Going to clean items from cache. Current size of the cache: {mHttpClientCache.Count}", LogLevel.Debug);

            // Find only clients without any use from the last check.
            var itemsToRemove = new List<KeyValuePair<KeyParams, ValuePair<HttpClient, short>>>();
            foreach (var item in mHttpClientCache)
            {
                if (item.Value.ValueB > 0)
                    continue;
                itemsToRemove.Add(item);
            }
            // Remove all found clients.
            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                ValuePair<HttpClient, short> removedVal;
                if (mHttpClientCache.TryRemove(itemsToRemove[i].Key, out removedVal))
                    removedVal.ValueA.Dispose();
            }

            // Find and remove least used clients and keep the list in proper size.
            var count = mHttpClientCache.Count;
            while (count > mCleanCacheSize)
            {
                short maxVal = short.MinValue;
                KeyParams maxKey = null;

                foreach (var item in mHttpClientCache)
                {
                    if (item.Value.ValueB >= maxVal)
                    {
                        maxVal = item.Value.ValueB;
                        maxKey = item.Key;
                    }
                }

                // Remove.
                ValuePair<HttpClient, short> removedVal;
                if (mHttpClientCache.TryRemove(maxKey, out removedVal))
                    removedVal.ValueA.Dispose();
                count = mHttpClientCache.Count;
            }

            // Set all values to zero for the next check.
            foreach (var item in mHttpClientCache)
                item.Value.ValueB = 0;

            // Log it.
            IoC.Logger.Log($"Size of the cache after cleaning: {mHttpClientCache.Count}", LogLevel.Debug);
        }

        #endregion

        #region Inner Classes
        
        /// <summary>
        /// Represents address which cliens use for its connection, and parameter needs for client init.
        /// </summary>
        protected class KeyParams
        {
            #region Public Properties

            /// <summary>
            /// URI address
            /// </summary>
            public string Address { get; private set; } //;
            
            /// <summary>
            /// Timeout in milliseconds
            /// </summary>
            public int Timeout { get; private set; } //;

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="address"></param>
            /// <param name="parameters"></param>
            public KeyParams(string address, params object[] parameters)
            {
                Address = address;
                SetParameters(parameters);
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Set parameters and its default values.
            /// </summary>
            /// <param name="client">The client</param>
            /// <param name="parameters">The same parameters as <see cref="GetClientForHost(Uri, object[])"/> method has.</param>
            private void SetParameters(params object[] parameters)
            {
                // Timeout.
                Timeout = parameters.Length > 0 ? (int)parameters[0] : 0;
            }

            #endregion

            #region HashCode & Equal

            /// <summary>
            /// Specification of hash code.
            /// Use to specify which attributes should decide about equality.
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int magic = 23;
                    int hash = 17;
                    hash = hash * magic + Address == string.Empty ? 0 : Address.GetHashCode();
                    hash = hash * magic + Timeout.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                return GetHashCode() == obj.GetHashCode();
            }
            
            #endregion
        }

        #endregion
    }
}
