using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupMenuItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// ID of the group.
        /// </summary>
        public byte ID { get; set; }

        /// <summary>
        /// Group Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Button control - says if any of the child timers are active.
        /// </summary>
        public bool IsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command play or pause group.
        /// </summary>
        public ICommand PlayPauseCommand { get; set; }

        #endregion

        #region Constructor

        public TimerGroupMenuItemViewModel()
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
            PlayPauseCommand = new RelayCommand(async () => await PlayPauseAsync());
        }

        /// <summary>
        /// Command helper, open page async.
        /// </summary>
        /// <returns></returns>
        private async Task PlayPauseAsync()
        {
            Title = "Hey";
            System.Console.WriteLine("TRIGGER");

            await Task.Delay(1);
        }
    }
}
