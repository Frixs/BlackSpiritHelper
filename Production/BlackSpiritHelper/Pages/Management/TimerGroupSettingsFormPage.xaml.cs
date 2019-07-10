using BlackSpiritHelper.Core;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for TimerGroupSettingsFormPage.xaml
    /// </summary>
    public partial class TimerGroupSettingsFormPage : BasePage<TimerGroupSettingsFormViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimerGroupSettingsFormPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page.</param>
        public TimerGroupSettingsFormPage(TimerGroupSettingsFormViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }
    }
}
