using BlackSpiritHelper.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for ScheduleTemplateSettingsFormPage.xaml
    /// </summary>
    public partial class ScheduleTemplateSettingsFormPage : BasePage<ScheduleTemplateSettingsFormPageViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleTemplateSettingsFormPage() : base()
        {
            InitializeComponent();

            // Run lazy load.
            StartLazyLoad();
        }

        /// <summary>
        /// Constructor with specific view model.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page.</param>
        public ScheduleTemplateSettingsFormPage(ScheduleTemplateSettingsFormPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set TimeZoneRegion ComboBox ItemsSource.
            // While changing rules, do not forget to change list in ScheduleTimeZoneRegionRule.
            var items = Enum.GetValues(typeof(TimeZoneRegion));
            List<TimeZoneRegion> list = new List<TimeZoneRegion>();
            for (int i = 0; i < items.Length; i++)
            {
                if (((TimeZoneRegion)items.GetValue(i)) > 0)
                    list.Add((TimeZoneRegion)items.GetValue(i));
            }
            TimeZoneRegionComboBox.ItemsSource = list;
            TimeZoneRegionComboBox.SelectedIndex = (int)TimeZoneRegionComboBox.Tag;

            // Run lazy load.
            StartLazyLoad();
        }

        /// <summary>
        /// Run lazy load.
        /// </summary>
        private void StartLazyLoad()
        {
            // Lazy load task.
            IoC.Task.Run(async () =>
            {
                // Lazy load delay.
                await Task.Delay((int)(SlideSeconds * 2500));

                // Update UI thread.
                await IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(async () =>
                {
                    ScheduleControl.Visibility = System.Windows.Visibility.Hidden;
                    await Task.Delay(1000); // Delay some time to load (heavy) schedule in invisibility.

                    // Animation.
                    await ScheduleControl.SlideAndFadeInFromBottom(8, 0.6f);
                }));
            });
        }

        /// <summary>
        /// Serve selection changed for <see cref="BlackSpiritHelper.Core.ScheduleTemplateSettingsFormPageViewModel.SchedulePresenter"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var itemList = (ObservableCollection<ScheduleItemDataViewModel>)(sender as ComboBox).FindParent<ItemsControl>().ItemsSource;
            //int index = itemList.IndexOf((ScheduleItemDataViewModel)e.RemovedItems[0]);
            int index = (int)(sender as ComboBox).Tag;
            itemList[index] = (ScheduleItemDataViewModel)e.AddedItems[0];
        }
    }
}
