using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Group List data structure.
        /// </summary>
        public TimerGroupListDesignModel TimerGroupListDesignModel { get; private set; } = IoC.DataContent.TimerGroupListDesignModel;

        #endregion

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
        public TimerGroupMenuViewModel()
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
            TimerGroupListDesignModel.AddGroup("hey2");

            await Task.Delay(1);
        }

        #endregion
    }
}
