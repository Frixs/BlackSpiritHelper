using BlackSpiritHelper.Core;

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
        }
    }
}
