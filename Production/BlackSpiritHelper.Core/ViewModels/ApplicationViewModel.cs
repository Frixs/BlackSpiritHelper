using System;

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
        public ApplicationPage CurrentPage { get; private set; } = 
            Properties.Settings.Default.LastOpenedPage > 0 && Properties.Settings.Default.LastOpenedPage < Enum.GetNames(typeof(ApplicationPage)).Length 
            ? (ApplicationPage)Properties.Settings.Default.LastOpenedPage 
            : ApplicationPage.Home;

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="page"></param>
        public void GoToPage(ApplicationPage page)
        {
            CurrentPage = page;
        }
    }
}
