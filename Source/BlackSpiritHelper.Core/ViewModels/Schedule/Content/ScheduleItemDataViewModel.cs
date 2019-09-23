using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Represents the schedule time item - only the name without time - Like Karanda, Nouver etc.
    /// </summary>
    public class ScheduleItemDataViewModel : BaseViewModel
    {
        #region Private Properties

        /// <summary>
        /// Says, if the template is initialized.
        /// </summary>
        private bool mIsInitialized = false;

        /// <summary>
        /// Says if the item is predefined or not.
        /// </summary>
        private bool mIsPredefined = false;

        /// <summary>
        /// Flag that counts number of changes, but only the one can be done per <see cref="mIgnoreListMoveCounterFlagTime"/> time period.
        /// </summary>
        private static int mIgnoreListMoveCounterFlag = 0;

        /// <summary>
        /// Time period (delay) during which <see cref="OnItemIgnoredMoveAsync"/> cannot be done.
        /// </summary>
        private static TimeSpan mIgnoreListMoveCounterFlagTime = TimeSpan.FromSeconds(5);

        #endregion

        #region Public Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; } = "NoName";

        /// <summary>
        /// Name with only 2 leading letters.
        /// </summary>
        [XmlIgnore]
        public string NameShortcut => Name.Length > 1 ? Name.Substring(0, 2).ToUpper() : Name.ToUpper();

        /// <summary>
        /// Representing color.
        /// </summary>
        public string ColorHEX { get; set; } = "000000";

        /// <summary>
        /// Says if the item is predefined or not.
        /// </summary>
        [XmlIgnore]
        public bool IsPredefined => mIsPredefined;

        #endregion

        #region Commands

        /// <summary>
        /// Command to add item to ignored list.
        /// </summary>
        [XmlIgnore]
        public ICommand AddItemToIgnoredCommand { get; set; }

        /// <summary>
        /// Command to remove item from ignored list.
        /// </summary>
        [XmlIgnore]
        public ICommand RemoveItemFromIgnoredCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleItemDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        /// <summary>
        /// Initialize the instance.
        /// </summary>
        /// <param name="isPredefined"></param>
        public void Init(bool isPredefined = false)
        {
            if (mIsInitialized)
                return;
            mIsInitialized = true;

            mIsPredefined = isPredefined;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            AddItemToIgnoredCommand = new RelayParameterizedCommand(async (parameter) => await AddItemToIgnoredAsync(parameter));
            RemoveItemFromIgnoredCommand = new RelayParameterizedCommand(async (parameter) => await RemoveItemFromIgnoredAsync(parameter));
        }


        /// <summary>
        /// Add item to ignore list.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task AddItemToIgnoredAsync(object parameter)
        {
            if (parameter == null || !parameter.GetType().Equals(typeof(string)))
                return;
            string par = (string)parameter;

            IoC.DataContent.ScheduleDesignModel.AddItemToIgnoredList(par);

            await Task.Delay(1);
        }

        /// <summary>
        /// Remove item from ignore list.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task RemoveItemFromIgnoredAsync(object parameter)
        {
            if (parameter == null || !parameter.GetType().Equals(typeof(string)))
                return;
            string par = (string)parameter;

            IoC.DataContent.ScheduleDesignModel.RemoveItemFromIgnoredList(par);

            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update UI schedule for user.
        /// All procedures will be done after certain time period without user interactions.
        /// This method is done only if no items is moved from or to ignored list within <see cref="mIgnoreListMoveCounterFlagTime"/>.
        /// </summary>
        /// <returns></returns>
        public static async Task OnItemIgnoredMoveAsync()
        {
            mIgnoreListMoveCounterFlag++;
            await Task.Delay(mIgnoreListMoveCounterFlagTime);
            mIgnoreListMoveCounterFlag--;

            if (mIgnoreListMoveCounterFlag > 0)
                return;

            if (!IoC.DataContent.ScheduleDesignModel.IsRunning)
                return;

            if (IoC.DataContent.ScheduleDesignModel.SelectingTemplateFlag)
                return;

            IoC.DataContent.ScheduleDesignModel.FindAndRemarkIgnored();

            await Task.Delay(1);
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Check schedule template parameters.
        /// TRUE, if all parameters are OK.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateInputs(ScheduleItemDataViewModel vm, string name, string colorHEX)
        {
            #region Name

            if (!new ScheduleItemNameRule().Validate(name, null).IsValid)
                return false;

            #endregion

            #region ColorHEX

            if (!colorHEX.CheckColorHEX())
                return false;

            #endregion

            return true;
        }

        #endregion
    }
}
