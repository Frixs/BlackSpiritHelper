using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// ViewModel for the user control TimerGroupMenu.
    /// </summary>
    public class TimerGroupMenuControlViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to add a new group.
        /// </summary>
        public ICommand AddGroupCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TimerGroupMenuControlViewModel()
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
            AddGroupCommand = new RelayCommand(async () => await AddGroupAsync());
        }

        /// <summary>
        /// Command helper.
        /// </summary>
        /// <returns></returns>
        private async Task AddGroupAsync()
        {
            IoC.DataContent.TimerData.AddGroup("Untitled Group");

            await Task.Delay(1);
        }

        #endregion
    }
}
