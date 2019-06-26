namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The View Model for the custom flat window.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members
        #endregion

        #region Public Properties

        public string TestString { get; set; }

        #endregion

        #region Commands
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HomeViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        /// <summary>
        /// Create Windows commands.
        /// </summary>
        private void CreateCommands()
        {
        }
    }
}
