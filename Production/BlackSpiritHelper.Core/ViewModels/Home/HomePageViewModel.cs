using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The View Model for the custom flat window.
    /// </summary>
    public class HomePageViewModel : BaseViewModel
    {
        #region Command

        /// <summary>
        /// The command to reset overlay position.
        /// </summary>
        public ICommand GetStartedCommand { get; set; }

        /// <summary>
        /// The command to open author webpage.
        /// </summary>
        public ICommand AuthorWebpageLinkCommand { get; set; }

        /// <summary>
        /// The command to open donation link.
        /// </summary>
        public ICommand AuthorDonateLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public HomePageViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            GetStartedCommand = new RelayCommand(async () => await GetStartedCommandMethodAsync());
            AuthorWebpageLinkCommand = new RelayCommand(async () => await AuthorWebpageLinkMethodAsync());
            AuthorDonateLinkCommand = new RelayCommand(async () => await AuthorDonateLinkMethodAsync());
        }

        /// <summary>
        /// Open webpage with the Get started info.
        /// </summary>
        /// <returns></returns>
        private async Task GetStartedCommandMethodAsync()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start("https://github.com/Frixs/BlackSpiritHelper/wiki");

            await Task.Delay(1);
        }

        /// <summary>
        /// Open author webpage.
        /// </summary>
        private async Task AuthorWebpageLinkMethodAsync()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start("https://github.com/Frixs");

            await Task.Delay(1);
        }

        /// <summary>
        /// Open donation weblink.
        /// </summary>
        private async Task AuthorDonateLinkMethodAsync()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start(IoC.Application.DonationURL);

            await Task.Delay(1);
        }

        #endregion
    }
}
