using BlackSpiritHelper.Core;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for TimerItemSettingsFormPage.xaml
    /// </summary>
    public partial class TimerItemSettingsFormPage : BasePage<TimerItemSettingsFormViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerItemSettingsFormPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page.</param>
        public TimerItemSettingsFormPage(TimerItemSettingsFormViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();

            // Set the current group ID to the validation rule.
            GroupChangeComboBoxRule.CurrentGroupID = (sbyte)GroupChangeComboBox.Tag;

            // Set ComboBox ItemsSource.
            GroupChangeComboBox.ItemsSource = IoC.DataContent.TimerGroupListDesignModel.GroupList;
            GroupChangeComboBox.DisplayMemberPath = "Title";
            GroupChangeComboBox.SelectedIndex = IoC.DataContent.TimerGroupListDesignModel.GroupList
                .Select((c, i) => new { Group = c, Index = i })
                .First(o => o.Group.ID == (sbyte)GroupChangeComboBox.Tag)
                .Index;
        }
    }
}
