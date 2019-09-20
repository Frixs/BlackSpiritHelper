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
        private ScheduleViewModel mFormVM = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Schedule template VM associated to this settings.
        /// </summary>
        public ScheduleViewModel FormVM
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
        public string Name { get; set; }

        /// <summary>
        /// Item custom list binding.
        /// </summary>
        public ObservableCollection<ScheduleItemDataViewModel> ItemCustomList { get; set; }

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

        /// <summary>
        /// The command to save changes.
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

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
            //ItemCustomList = FormVM.ItemCustomList.CopyObject;
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
            RemoveItemCommand = new RelayCommand(() => RemoveItem());
            SaveChangesCommand = new RelayCommand(() => SaveChanges());
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        private void SaveChanges()
        {
            Console.WriteLine("Save!");
        }

        /// <summary>
        /// Remove item from <see cref="ScheduleViewModel.ItemCustomList"/>.
        /// </summary>
        private void RemoveItem()
        {
            Console.WriteLine("Remove!");
        }

        /// <summary>
        /// Add new item to <see cref="ScheduleViewModel.ItemCustomList"/>.
        /// </summary>
        private void AddItem()
        {
            if (!IoC.DataContent.ScheduleDesignModel.CanAddCustomItem)
                return;

            // Trim.
            string name = Name.Trim();

            // Validate inputs.
            if (!Core.ScheduleItemDataViewModel.ValidateInputs(null, name))
            {
                // Some error occured during saving changes of the timer.
                IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Caption = "Invalid Parameters!",
                    Message = $"Some of entered parameters are invalid. Please check them again.{Environment.NewLine}",
                    Button = System.Windows.MessageBoxButton.OK,
                    Icon = System.Windows.MessageBoxImage.Warning,
                });

                return;
            }

            // TODO: Custom items can have custom colors.
            // TODO: VM as injection to this form - ScheduleViewModel?
            // TODO: use AddItem() - ScheduleViewModel

            // Sort schedule.
            //ScheduleTemplateDataViewModel.SortSchedule();

            // Save changes.
            //#region Save changes

            //ScheduleTemplateDataViewModel.Title = title;
            //ScheduleTemplateDataViewModel.TimeZoneRegion = TimeZoneRegion;
            //ScheduleTemplateDataViewModel.Schedule = Schedule;

            //#endregion

            // Log it.
            //IoC.Logger.Log($"Item '{ScheduleTemplateDataViewModel.Title}' settings changed!", LogLevel.Info);

            //IoC.DataContent.ScheduleDesignModel.ItemCustomList.Add();
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
