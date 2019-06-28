using System;
using System.Threading.Tasks;
using System.Windows.Input;

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

        /// <summary>
        /// The command to switch the page to Combat.
        /// </summary>
        public ICommand CombatCommand { get; set; }

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
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            CombatCommand = new RelayCommand(async () => await CombatAsync());
        }

        private async Task CombatAsync()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Timer);

            await Task.Delay(1);
        }
    }
}
