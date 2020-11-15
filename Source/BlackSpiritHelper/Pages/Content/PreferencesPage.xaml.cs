using BlackSpiritHelper.Core;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class PreferencesPage : BasePage<PreferencesPageViewModel>
    {
        public PreferencesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Selection changed - we have to save the value into <see cref="IoC.MOuse"/>
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var val = (OverlayInteractionKey)e.AddedItems[0];
            IoC.Get<IMouseKeyHook>().SetOverlayInteractionKey(val);
        }
    }
}
