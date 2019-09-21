using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper of <see cref="ScheduleItemDataViewModel"/> for schedule presenter with particular time.
    /// Previous <see cref="ScheduleTemplateDayDataViewModel"/>.
    /// </summary>
    public class ScheduleTemplateDayTimeDataViewModel : BaseViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max number of items that can be added to 1 event.
        /// </summary>
        public static byte AllowedMaxNoOfItemsInEvent { get; private set; } = 3;

        #endregion

        #region Private Members

        /// <summary>
        /// Temporary ID is useful to track equality of time items between <see cref="ScheduleTemplateDataViewModel.Schedule"/> and <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/>.
        /// </summary>
        private int mTemporaryID = -1;

        /// <summary>
        /// List of events at this time (<see cref="Time"/>).
        /// !!! This is only for loading at application start.
        /// </summary>
        private ObservableCollection<string> mItemList = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Temporary ID is useful to track equality of time items between <see cref="ScheduleTemplateDataViewModel.Schedule"/> and <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/>.
        /// </summary>
        [XmlIgnore]
        public int TemporaryID
        {
            get => mTemporaryID;
            set
            {
                if (mTemporaryID > -1)
                    return;
                mTemporaryID = value;
            }
        }

        /// <summary>
        /// Says, if the time is marked as next item to countdown to.
        /// It is used for GUI to show the time event with special marking.
        /// </summary>
        [XmlIgnore]
        public bool IsMarkedAsNext { get; set; } = false;

        /// <summary>
        /// Says, if this time item has all items ignored or not.
        /// !!! This is set only in <see cref="ScheduleTemplateDataViewModel.SchedulePresenter"/> not in <see cref="ScheduleTemplateDataViewModel.Schedule"/>.
        /// </summary>
        [XmlIgnore]
        public bool IsMarkedAsIgnored { get; set; } = false;

        /// <summary>
        /// Time.
        /// See <see cref="TimeHours"/> and <see cref="TimeMinutes"/>.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Time { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// <see cref="Time"/> hours.
        /// It is used to store <see cref="Time"/> in user settings.
        /// </summary>
        public int TimeHours
        {
            get => Time.Hours;
            set
            {
                // Do not touch TimeSpan, change the time as string.
                // Save minutes value what is currently set.
                int minutes = Time.Minutes;
                // Create new time.
                TimeSpan newTime = TimeSpan.Zero;
                // Set new value of hours.
                newTime = newTime.Add(TimeSpan.FromHours(value));
                // Set minutes value what was set before.
                newTime = newTime.Add(TimeSpan.FromMinutes(minutes));

                // Apply.
                Time = newTime;
            }
        }

        /// <summary>
        /// <see cref="Time"/> minutes.
        /// It is used to store <see cref="Time"/> in user settings.
        /// </summary>
        public int TimeMinutes
        {
            get => Time.Minutes;
            set
            {
                // Do not touch TimeSpan, change the time as string.
                // Save hours value what is currently set.
                int hours = Time.Hours;
                // Create new time.
                TimeSpan newTime = TimeSpan.Zero;
                // Set new value of minutes.
                newTime = newTime.Add(TimeSpan.FromMinutes(value));
                // Set hours value what was set before.
                newTime = newTime.Add(TimeSpan.FromHours(hours));

                // Apply.
                Time = newTime;
            }
        }

        /// <summary>
        /// List of events at this time (<see cref="Time"/>).
        /// !!! This should NOT be changed unless you are changing custom template in setting page.
        /// </summary>
        public ObservableCollection<string> ItemList
        {
            get => mItemList;
            set
            {
                mItemList = value;
                CanAddItem = value.Count < AllowedMaxNoOfItemsInEvent;
            }
        }

        /// <summary>
        /// List of events at this time (<see cref="Time"/>).
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<ScheduleItemDataViewModel> ItemListPresenter { get; set; }

        /// <summary>
        /// Can add next item to <see cref="ItemList"/>?
        /// </summary>
        [XmlIgnore]
        public bool CanAddItem { get; private set; } = true;

        #endregion

        #region Commands

        /// <summary>
        /// The command to add item.
        /// Settings command - working with presenter.
        /// </summary>
        [XmlIgnore]
        public ICommand AddItemCommand { get; set; }

        /// <summary>
        /// The command to remove item.
        /// Settings command - working with presenter.
        /// </summary>
        [XmlIgnore]
        public ICommand RemoveItemCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateDayTimeDataViewModel()
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
            AddItemCommand = new RelayCommand(() => AddItem(true));
            RemoveItemCommand = new RelayParameterizedCommand((parameter) => RemoveItem(true, parameter));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add new item.
        /// </summary>
        /// <param name="addToPresenter"></param>
        private void AddItem(bool addToPresenter)
        {
            IoC.Logger.Log("Add Item", LogLevel.Debug);

            // Add item to the list.
            if (addToPresenter)
            {
                ItemListPresenter.Add(IoC.DataContent.ScheduleDesignModel.ItemPredefinedList[0]);
                // Check limitations.
                if (ItemListPresenter.Count >= AllowedMaxNoOfItemsInEvent)
                    CanAddItem = false;
            }
            else
            {
                ItemList.Add(IoC.DataContent.ScheduleDesignModel.ItemPredefinedList[0].Name);
                // Check limitations.
                if (ItemList.Count >= AllowedMaxNoOfItemsInEvent)
                    CanAddItem = false;
            }
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="removeFromPresenter"></param>
        /// <param name="parameter"></param>
        private void RemoveItem(bool removeFromPresenter, object parameter)
        {
            IoC.Logger.Log("Remove Item", LogLevel.Debug);

            // Remove from the list and set limitation.
            if (removeFromPresenter)
            {
                if (parameter == null || !parameter.GetType().Equals(typeof(ScheduleItemDataViewModel)))
                {
                    IoC.Logger.Log($"Wrong type - {parameter.GetType().ToString()}!", LogLevel.Error);
                    return;
                }
                ScheduleItemDataViewModel par = (ScheduleItemDataViewModel)parameter;
                if (ItemListPresenter.Remove(par))
                    CanAddItem = true;
            }
            else
            {
                if (parameter == null || !parameter.GetType().Equals(typeof(string)))
                {
                    IoC.Logger.Log($"Wrong type - {parameter.GetType().ToString()}!", LogLevel.Error);
                    return;
                }
                string par = (string)parameter;
                if (ItemList.Remove((string)parameter))
                    CanAddItem = true;
            }
        }

        #endregion
    }
}
