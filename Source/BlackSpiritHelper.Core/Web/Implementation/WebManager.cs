namespace BlackSpiritHelper.Core
{
    public class WebManager : IWebManager
    {
        #region Public Properties

        /// <summary>
        /// Reference to <see cref="HttpClient"/> factory
        /// </summary>
        public HttpClientFactory Http { get; } = new HttpClientFactory();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebManager()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Http.Dispose();
        }

        #endregion
    }
}
