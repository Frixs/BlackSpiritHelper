using System;
using System.Reflection;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The application state as a view model.
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Application name.
        /// </summary>
        public string ApplicationName { get; private set; } = "Black Spirit Helper";

        /// <summary>
        /// Window default title.
        /// </summary>
        public string WindowTitleDefault { get; private set; }

        /// <summary>
        /// Window title.
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Window title change only postfix.
        /// </summary>
        public string SetWindowTitlePostfixOnly
        {
            set
            {

                WindowTitle = value.Length > 0 ? WindowTitleDefault + " : " + value : WindowTitleDefault;
            }
        }

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
            SetWindowTitlePostfixOnly = page > 0 ? page.ToString() : "";
        }

        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            WindowTitleDefault = ApplicationName;
            SetWindowTitlePostfixOnly = CurrentPage > 0 ? CurrentPage.ToString() : "";
        }

        #endregion
    }
}
