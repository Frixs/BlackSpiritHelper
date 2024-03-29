﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Represents the schedule time item - only the name without time - Like Karanda, Nouver etc.
    /// </summary>
    public class ScheduleItemDataViewModel : ASetupableBaseViewModel
    {
        #region Private Properties

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
        public bool IsPredefined { get; private set; } = false;

        #endregion

        #region Command Flags

        private bool mAddItemToIgnoredCommandFlag { get; set; }
        private bool mRemoveItemFromIgnoredCommandFlag { get; set; }

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
        /// Init routine.
        /// </summary>
        /// <param name="parameters">
        ///     [0] = IsPredefined
        /// </param>
        protected override void InitRoutine(params object[] parameters)
        {
            if (!(parameters.Length > 0))
                IoC.Logger.Log("No required parameters!", LogLevel.Fatal);

            IsPredefined = (bool)parameters[0];
        }

        protected override void DisposeRoutine()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            AddItemToIgnoredCommand = new RelayParameterizedCommand(async (parameter) => await AddItemToIgnoredCommandMethodAsync(parameter));
            RemoveItemFromIgnoredCommand = new RelayParameterizedCommand(async (parameter) => await RemoveItemFromIgnoredCommandMethodAsync(parameter));
        }

        private async Task AddItemToIgnoredCommandMethodAsync(object parameter)
        {
            await RunCommandAsync(() => mAddItemToIgnoredCommandFlag, async () =>
            {
                await AddItemToIgnoredAsync(parameter);
            });
        }

        private async Task RemoveItemFromIgnoredCommandMethodAsync(object parameter)
        {
            await RunCommandAsync(() => mRemoveItemFromIgnoredCommandFlag, async () =>
            {
                await RemoveItemFromIgnoredAsync(parameter);
            });
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

            if (!IoC.DataContent.ScheduleData.IsRunning)
                return;

            if (IoC.DataContent.ScheduleData.SelectingTemplateFlag)
                return;

            IoC.DataContent.ScheduleData.FindAndRemarkIgnored();

            await Task.Delay(1);
        }

        #endregion

        #region Private Methods

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

            IoC.DataContent.ScheduleData.AddItemToIgnoredList(par);

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

            IoC.DataContent.ScheduleData.RemoveItemFromIgnoredList(par);

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
