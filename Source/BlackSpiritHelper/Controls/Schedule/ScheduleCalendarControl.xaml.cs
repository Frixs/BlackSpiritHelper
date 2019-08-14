using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for ScheduleCalendarControl.xaml
    /// </summary>
    public partial class ScheduleCalendarControl : UserControl
    {
        public ScheduleCalendarControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ignore mouse wheel to scroll in the scroll view which has binded this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
