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
        /// Allow to load back only content pages, not forms etc.
        /// <see cref="ApplicationPage"/>.
        /// </summary>
        public int LoadBackPageValueLimit { get; private set; } = 100;

        /// <summary>
        /// The current page of the application.
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; }

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="page"></param>
        public void GoToPage(ApplicationPage page)
        {
            CurrentPage = page;

            if ((int)page < LoadBackPageValueLimit)
                SetWindowTitlePostfixOnly = (int)page > 0 ? page.ToString() : "";
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApplicationViewModel()
        {
            WindowTitleDefault = ApplicationName;

            CurrentPage = Properties.Settings.Default.LastOpenedPage > 0 && Properties.Settings.Default.LastOpenedPage < 100 
                ? (ApplicationPage)Properties.Settings.Default.LastOpenedPage 
                : ApplicationPage.Home;

            SetWindowTitlePostfixOnly = CurrentPage > 0 ? CurrentPage.ToString() : "";
        }

        #endregion
    }
}
