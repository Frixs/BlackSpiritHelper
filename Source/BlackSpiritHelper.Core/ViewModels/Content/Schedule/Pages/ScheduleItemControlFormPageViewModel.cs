using System;
using System.Threading.Tasks;
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

        #region Command Flags

        private bool mModifyFlag { get; set; }

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
            GoBackCommand = new RelayCommand(() => GoBackCommandMethod());
            AddItemCommand = new RelayCommand(async () => await AddItemCommandMethodAsync());
            RemoveItemCommand = new RelayParameterizedCommand(async (parameter) => await RemoveItemCommandMethodAsync(parameter));
        }

        /// <summary>
        /// Remove item from <see cref="ScheduleDataViewModel.ItemCustomList"/>.
        /// </summary>
        /// <param name="parameter"></param>
        private async Task RemoveItemCommandMethodAsync(object parameter)
        {
            await RunCommandAsync(() => mModifyFlag, async () =>
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

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Add new item to <see cref="ScheduleDataViewModel.ItemCustomList"/>.
        /// </summary>
        private async Task AddItemCommandMethodAsync()
        {
            await RunCommandAsync(() => mModifyFlag, async () =>
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
                    _ = IoC.UI.ShowNotification(new NotificationBoxDialogViewModel()
                    {
                        Title = "ERROR OCCURRED!",
                        Message = $"The name of the item is already defined or some of the entered values are invalid. Please, check them again.",
                        Result = NotificationBoxResult.Ok,
                    });

                    return;
                }

                // Log it.
                IoC.Logger.Log($"Created new schedule custom item '{name}'.", LogLevel.Info);

                // Sort list.
                FormVM.SortItemCustomList();

                await Task.Delay(1);
            });
        }

        /// <summary>
        /// Back back command.
        /// </summary>
        private void GoBackCommandMethod()
        {
            // Move back to the page.
            IoC.Application.GoToPage(ApplicationPage.Schedule);
        }

        #endregion
    }
}
