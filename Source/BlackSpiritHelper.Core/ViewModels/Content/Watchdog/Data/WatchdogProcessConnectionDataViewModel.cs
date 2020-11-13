using BlackSpiritHelper.Core.Data.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        #region Static Limitation Properties

        /// <summary>
        /// Max number of process connections in a list that can be created.
        /// </summary>
        public static byte AllowedMaxProcessConnections { get; private set; } = 3;

        #endregion

        #region Private Members

        /// <summary>
        /// Process list to handle of this wrapper.
        /// </summary>
        private ObservableCollection<WatchdogProcessDataViewModel> mProcessList = new ObservableCollection<WatchdogProcessDataViewModel>();


        #endregion

        #region Public Properties

        /// <summary>
        /// Process list to handle of this wrapper.
        /// </summary>
        public ObservableCollection<WatchdogProcessDataViewModel> ProcessList
        {
            get => mProcessList;
            set
            {
                mProcessList = value;

                // This is for initial set (and any other set).
                if (value.Count >= AllowedMaxProcessConnections)
                    CanAddNewProcess = false;
                else
                    CanAddNewProcess = true;
            }
        }

        /// <summary>
        /// Says if new iem can be added to the list.
        /// Checks the limit.
        /// </summary>
        [XmlIgnore]
        public bool CanAddNewProcess { get; set; } = true;

        /// <summary>
        /// Says if the check is selected for checking loop.
        /// </summary>
        public override bool IsSelected { get; set; } = false;

        #endregion

        #region Command Flags

        private bool mModifyCommandFlag { get; set; }

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
        /// Add new process item to the list.
        /// </summary>
        private async Task AddNewProcessCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                // Check space in the list.
                if (ProcessList.Count + 1 > AllowedMaxProcessConnections)
                {
                    CanAddNewProcess = false;
                    return;
                }

                // Add item.
                ProcessList.Add(new WatchdogProcessDataViewModel());

                // Check space in the list again after addition.
                if (ProcessList.Count >= AllowedMaxProcessConnections)
                    CanAddNewProcess = false;

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Remove the item from process list.
        /// </summary>
        private async Task RemoveProcessCommandMethodAsync(object parameter)
        {
            await RunCommandAsync(() => mModifyCommandFlag, async () =>
            {
                var itemToRemove = (WatchdogProcessDataViewModel)parameter;

                // Check existence of the item.
                if (itemToRemove == null)
                {
                    IoC.Logger.Log($"Null reference while removing item!", LogLevel.Error);
                    return;
                }

                // Remove item.
                ProcessList.Remove(itemToRemove);
                CanAddNewProcess = true;

                await Task.Delay(1);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The main method to check conditions.
        /// </summary>
        /// <returns></returns>
        public override bool Check()
        {
            bool ret = true;

            for (int i = 0; i < mProcessList.Count; i++)
            {
                // Get process.
                Process p = mProcessList[i].FindProcess();
                // Check if the process exists.
                if (p == null)
                {
                    IoC.DataContent.WatchdogData.Log($"No process found under the name \"{mProcessList[i].Name}\"!");
                    continue;
                }

                // Check the presence in the net table.
                if (!CheckProcessPresenceInNet(p.Id))
                {
                    IoC.DataContent.WatchdogData.Log($"Process \"{mProcessList[i].Name}\" does not have any presence in the net!");
                    ret = false;
                    // Kill the process if the process supposed to be killed.
                    if (mProcessList[i].KillOnFailure)
                    {
                        IoC.DataContent.WatchdogData.Log($"Killing \"{mProcessList[i].Name}\" process...");
                        p.Kill();
                    }
                }
            }

            return ret;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check presence of PID in netstat.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool CheckProcessPresenceInNet(int pid)
        {
            int lineCount = 0;
            ProcessStartInfo psi = new ProcessStartInfo("cmd");

            psi.Arguments = $"/c netstat /a /o /n | findstr /r {pid}$";

            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            Process process = new Process();

            process.EnableRaisingEvents = true;
            process.StartInfo = psi;
            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) // Some lines might be null
                    lineCount++;
            };
            //process.ErrorDataReceived += (s, e) => { netstatLines.Add(e.Data); }; // For debug construction only. We dont want to print error, because we are counting lines with good output only.

            process.Start();

            process.BeginOutputReadLine();
            //process.BeginErrorReadLine();

            process.WaitForExit();

            return lineCount > 0;
        }

        #endregion
    }
}
