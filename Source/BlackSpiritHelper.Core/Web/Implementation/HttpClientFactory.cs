using System.Net.Http;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper around <see cref="HttpClient"/> to prevent known exceptions.
    /// ---
    /// HttpClient followed work-around solution: https://github.com/aspnet/Extensions/issues/1345
    /// Socket section with examples: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/sockets
    /// </summary>
    public class HttpClientFactory : IClientFactory<HttpClient>
    {
        #region Public Properties

        /// <summary>
        /// Major client.
        /// </summary>
        public HttpClient Client { get; } //; Constructor

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpClientFactory()
        {
            // TODO
            Client = new HttpClient();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// New client instance.
        /// </summary>
        /// <returns></returns>
        public HttpClient NewClient()
        {
            return new HttpClient();
        }

        #endregion
    }
}
