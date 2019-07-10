using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class TimerGroupSettingsFormViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The Group associated to this settings.
        /// </summary>
        public TimerGroupViewModel TimerGroupViewModel { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to save changes in the group settings.
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// The command to delete group.
        /// </summary>
        public ICommand DeleteGroupCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerGroupSettingsFormViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Helpers

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            SaveChangesCommand = new RelayParameterizedCommand((parameter) => SaveChanges(parameter));
            DeleteGroupCommand = new RelayCommand(() => DeleteGroup());
        }

        private void SaveChanges(object parameter)
        {
            Console.WriteLine("TODO");
        }

        private void DeleteGroup()
        {
            if (!IoC.DataContent.TimerGroupListDesignModel.DeleteGroup(TimerGroupViewModel))
            {
                // Some error occured during deleting the group.

            }
        }

        #endregion
    }
}
