using System;
using System.Net.Http;

namespace BlackSpiritHelper.Core
{
    public interface IWebManager : IDisposable
    {
        /// <summary>
        /// Reference to <see cref="HttpClient"/> factory.
        /// </summary>
        HttpClientFactory Http { get; }
    }
}
