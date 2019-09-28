using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlackSpiritHelper.Core
{
    public class ScheduleItemControlFormPageViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Schedule template associated to this settings.
        /// </summary>
        private ScheduleDataViewModel mFormVM = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Schedule template VM associated to this settings.
        /// </summary>
        public ScheduleDataViewModel FormVM
        {
            get
            {
                return mFormVM;
            }
            set
            {
                mFormVM = value;

                // Bind properties to the inputs.
                BindProperties();
            }
        }

        /// <summary>
        /// Name of item to add.
        /// </summary>
        public string NewName { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to go back to the schedule page.
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        /// <summary>
        /// The command to add custom item.
        /// </summary>
        public ICommand AddItemCommand { get; set; }

        /// <summary>
        /// The command to remove custom item.
        /// </summary>
        public ICommand RemoveItemCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleItemControlFormPageViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        /// <summary>
        /// Bind properties to the inputs.
        /// </summary>
        private void BindProperties()
        {
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            GoBackCommand = new RelayCommand(() => GoBack());
            AddItemCommand = new RelayCommand(() => AddItem());
            RemoveItemCommand = new RelayParameterizedCommand((parameter) => RemoveItem(parameter));
        }

        /// <summary>
        /// Remove item from <see cref="ScheduleDataViewModel.ItemCustomList"/>.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveItem(object parameter)
        {
            var par = (ScheduleItemDataViewModel)parameter;

            IoC.Logger.Log($"Removing schedule custom item '{par}'...", LogLevel.Debug);

            // Remove.
            if (!FormVM.DestroyCustomItem(par))
            {
                IoC.Logger.Log($"Error occured during removing schedule custom item '{par}'!", LogLevel.Error);
                return;
            }

            // Log it.
            IoC.Logger.Log($"Removed schedule custom item '{par}'.", LogLevel.Info);
        }

        /// <summary>
        /// Add new item to <see cref="ScheduleDataViewModel.ItemCustomList"/>.
        /// </summary>
        private void AddItem()
        {
            IoC.Logger.Log("Creating new schedule custom item.", LogLevel.Debug);

            if (!IoC.DataContent.ScheduleData.CanAddCustomItem)
                return;

            if (string.IsNullOrEmpty(NewName))
                return;

            // Trim.
            string name = NewName.Trim();
            string colorHex = "000000";

            // Add item.
            if (FormVM.AddItem(name, colorHex, false) == null)
            {
                // Some error occured during adding item.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Error occured!",
                    Message = $"The name of the item is already defined or some of entered parameters are invalid. Please check them again.{Environment.NewLine}",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // Log it.
            IoC.Logger.Log($"Created new schedule custom item '{name}'.", LogLevel.Info);

            // Sort list.
            FormVM.SortItemCustomList();
        }

        /// <summary>
        /// Back back command.
        /// </summary>
        private void GoBack()
        {
            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Schedule);
        }

        #endregion
    }
}
