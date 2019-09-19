using BlackSpiritHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;

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
        }
    }
}
