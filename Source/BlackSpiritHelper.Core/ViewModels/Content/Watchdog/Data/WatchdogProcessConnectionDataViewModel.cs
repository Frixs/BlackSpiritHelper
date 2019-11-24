using BlackSpiritHelper.Core.Data.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Process Connection wrapper.
    /// Serves to <see cref="WatchdogConnectionWatcherDataViewModel"/>.
    /// </summary>
    public class WatchdogProcessConnectionDataViewModel : AWatchdogConnectionBase
    {
        #region Public Properties

        /// <summary>
        /// Process list to handle of this wrapper.
        /// </summary>
        public ObservableCollection<WatchdogProcessDataViewModel> ProcessList { get; set; } = new ObservableCollection<WatchdogProcessDataViewModel>();

        /// <summary>
        /// Says if the check is selected for checking loop.
        /// </summary>
        public override bool IsSelected { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// Command to add new process to the list.
        /// </summary>
        [XmlIgnore]
        public ICommand AddNewProcessCommand { get; set; }

        /// <summary>
        /// Command to remove process from the list.
        /// </summary>
        [XmlIgnore]
        public ICommand RemoveProcessCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogProcessConnectionDataViewModel()
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
            AddNewProcessCommand = new RelayCommand(async () => await AddNewProcessCommandMethodAsync());
            RemoveProcessCommand = new RelayParameterizedCommand(async (parameter) => await RemoveProcessCommandMethodAsync(parameter));
        }

        /// <summary>
        /// TODO add process
        /// </summary>
        private async Task AddNewProcessCommandMethodAsync() 
        {
            System.Console.WriteLine("Add");
            ProcessList.Add(new WatchdogProcessDataViewModel() { Name = "Yo" });
            await Task.Delay(1);
        }

        /// <summary>
        /// TODO Remove process
        /// </summary>
        private async Task RemoveProcessCommandMethodAsync(object parameter)
        {
            System.Console.WriteLine("Remove");
            System.Console.WriteLine(((WatchdogProcessDataViewModel)parameter).Name);
            await Task.Delay(1);
        }

        #endregion
    }
}
