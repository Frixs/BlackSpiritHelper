using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for NotificationBoxDialogControl.xaml
    /// </summary>
    public partial class NotificationBoxDialogControl : UserControl
    {
        public NotificationBoxDialogControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
