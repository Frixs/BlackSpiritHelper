using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class SideMenuControlViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to open home page.
        /// </summary>
        public ICommand OpenHomePageCommand { get; set; }

        /// <summary>
        /// The command to open preferences page.
        /// </summary>
        public ICommand OpenPreferencesPageCommand { get; set; }

        /// <summary>
        /// The command to open donation link.
        /// </summary>
        public ICommand AuthorDonateLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuControlViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create Windows commands.
        /// </summary>
        private void CreateCommands()
        {
            OpenHomePageCommand = new RelayCommand(async () => await OpenHomePageAsync());
            OpenPreferencesPageCommand = new RelayCommand(async () => await OpenPreferencesPageAsync());
            AuthorDonateLinkCommand = new RelayCommand(() => AuthorDonateLinkMethod());
        }

        /// <summary>
        /// Open home page command task.
        /// </summary>
        /// <returns></returns>
        private async Task OpenHomePageAsync()
        {
            IoC.Application.GoToPage(ApplicationPage.Home);

            await Task.Delay(1);
        }

        /// <summary>
        /// Open preferences page command task.
        /// </summary>
        /// <returns></returns>
        private async Task OpenPreferencesPageAsync()
        {
            IoC.Application.GoToPage(ApplicationPage.Preferences);

            await Task.Delay(1);
        }

        /// <summary>
        /// Open donation weblink.
        /// </summary>
        private void AuthorDonateLinkMethod()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start(IoC.Application.DonationURL);
        }

        #endregion
    }
}
