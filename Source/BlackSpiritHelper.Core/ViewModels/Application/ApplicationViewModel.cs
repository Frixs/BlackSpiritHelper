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
        /// Application product name.
        /// Represents how the Windows shortcuts will be named, e.g.
        /// ---
        /// This const is duplicate of the value from ClickOnce installation settings (Publish->Options)!!!
        /// Do not forget to change the value on both places!!!
        /// </summary>
        public string ProductName { get; } = "Black Spirit Helper";

        /// <summary>
        /// Application publisher name.
        /// Represents name of directory in Start Menu, where the application deploys its shortcuts.
        /// ---
        /// This const is duplicate of the value from ClickOnce installation settings (Publish->Options)!!!
        /// Do not forget to change the value on both places!!!
        /// </summary>
        public string PublisherName { get; } = "Tomas Frixs";

        /// <summary>
        /// Application Title/Name.
        /// </summary>
        public string ApplicationName { get; private set; } = "Black Spirit Helper (BETA)";

        /// <summary>
        /// Assembly set of the application.
        /// </summary>
        public Assembly ApplicationExecutingAssembly { get; set; }

        /// <summary>
        /// Assembly application version.
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Assembly copyright.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Donation URL address.
        /// </summary>
        public string DonationURL { get; private set; } = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=QE2V3BNQJVG5W&source=url";

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
        /// The view model to use for the current page when the <see cref="CurrentPage"/> changes.
        /// NOTE: This is not a love up-to-date view model of the current page.
        ///       It is simply used to set the view model of the current page at the time it chages.
        ///       In other words, we can pass data to the new page.
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// Maximal enum value of <see cref="ApplicationPage"/> for content pages.
        /// The rest are pages that are not accessible as a content (forms etc.).
        /// </summary>
        public int ApplicationContentPageValueLimit { get; private set; } = 99;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApplicationViewModel()
        {
            WindowTitleDefault = ApplicationName;

            CurrentPage = IoC.SettingsStorage.LastOpenedPage > 0 && IoC.SettingsStorage.LastOpenedPage < 100 
                ? (ApplicationPage)IoC.SettingsStorage.LastOpenedPage 
                : ApplicationPage.Home;

            SetWindowTitlePostfixOnly = CurrentPage > 0 ? CurrentPage.GetDescription() : "";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="page">The page to go to.</param>
        /// <param name="viewModel">The view model, if any, ti set explicitly to the new page.</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model.
            CurrentPageViewModel = viewModel;

            // Set the current page.
            CurrentPage = page;

            // Fire off a CurrentPage chaned event.
            OnPropertyChanged(nameof(CurrentPage));

            // Set window title page name.
            if ((int)page < LoadBackPageValueLimit)
                SetWindowTitlePostfixOnly = (int)page > 0 ? page.GetDescription() : "";
        }

        #endregion
    }
}
