using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    #region Public Properties

    public class TimerGroupMenuViewModel : BaseViewModel
    {
        /// <summary>
        /// List of timer groups.
        /// </summary>
        public ObservableCollection<TimerGroupMenuItemViewModel> Groups { get; set; }

        /// <summary>
        /// Max number of groups that can be created.
        /// </summary>
        public byte MaxNoOfGroups { get; private set; } = 5;

        /// <summary>
        /// Says if you can create a new item. Limit check.
        /// </summary>
        public bool CanCreateNewItem { get; private set; }

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
            Groups = new ObservableCollection<TimerGroupMenuItemViewModel>();

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
            AddGroup("hey2");

            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new group.
        /// </summary>
        /// <param name="itemTitle">The group item title.</param>
        /// <returns></returns>
        public bool AddGroup(string itemTitle)
        {
            IoC.Logger.Log($"Trying to add Timer Group '{itemTitle}'...", LogLevel.Debug);

            //TODO conditions.
            //if (itemTitle.Trim().Length <= 0)
            //    return false;

            //if (Groups.Count + 1 > MaxNoOfGroups)
            //    return false;

            //// Sort Groups by ID.
            //Groups.OrderBy(o => o.ID);

            //// Create a new item.
            //TimerGroupMenuItemViewModel item = new TimerGroupMenuItemViewModel
            //{
            //    ID = (byte)FindNewID(0, Groups.Count - 1),
            //    Title = itemTitle,
            //    IsRunning = false,
            //};

            ////TODO oslve sync.
            //Groups.Add(item);

            //// Create a new key in the timer dictionary.
            //TimerListDesignModel.Instance.TimerDictionary.Add(item, new List<TimerListItemViewModel>());

            //// Sort dictionary.
            //TimerListDesignModel.Instance.TimerDictionary.OrderBy(o => o.Key);

            IoC.Logger.Log($"Timer Group '{itemTitle}' added!", LogLevel.Info);
            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Find the smallest missing key in the list as a new representation for a new group ID.
        /// </summary>
        /// <param name="start">The start index of list.</param>
        /// <param name="end">The end index of list.</param>
        /// <returns></returns>
        private int FindNewID(int start, int end)
        {
            if (start > end)
                return end + 1;

            if (start != Groups[start].ID)
                return start;

            int mid = (start + end) / 2;

            // Left half has all elements from 0 to mid 
            if (Groups[mid].ID == mid)
                return FindNewID(mid + 1, end);

            return FindNewID(start, mid);
        }

        #endregion
    }
}
