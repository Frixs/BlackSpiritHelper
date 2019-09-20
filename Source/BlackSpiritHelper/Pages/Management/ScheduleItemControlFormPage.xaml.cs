using BlackSpiritHelper.Core;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for ScheduleItemControlFormPage.xaml
    /// </summary>
    public partial class ScheduleItemControlFormPage : BasePage<ScheduleItemControlFormPageViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScheduleItemControlFormPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page.</param>
        public ScheduleItemControlFormPage(ScheduleItemControlFormPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}
