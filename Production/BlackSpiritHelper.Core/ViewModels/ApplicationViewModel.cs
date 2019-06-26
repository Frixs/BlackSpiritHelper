namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The application state as a view model.
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application.
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = Properties.Settings.Default.LastOpenedPage > 0 ? (ApplicationPage)Properties.Settings.Default.LastOpenedPage : ApplicationPage.Home;
    }
}
