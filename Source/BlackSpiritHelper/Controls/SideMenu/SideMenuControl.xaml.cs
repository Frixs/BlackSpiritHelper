using BlackSpiritHelper.Core;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Windows.Foundation.Metadata;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        #region Private Members

        private ComboBox mWindowComboBox;
        private ComboBox mMonitorComboBox;

        #endregion

        #region Constructor

        public SideMenuControl()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Window Combo - SelectionChanged
        /// </summary>
        private void ComboBox_SelectionChanged_Window(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
                ((SideMenuControlViewModel)DataContext).SelectScreenCaptureWindowCommand.Execute(new ScreenCaptureHandle(((Process)items[0]).MainWindowHandle, true));
        }

        /// <summary>
        /// Window Combo - DropDownOpened
        /// </summary>
        private void ComboBox_DropDownOpened_Window(object sender, System.EventArgs e)
        {
            ((SideMenuControlViewModel)DataContext).StopScreenCaptureCommand.Execute(null);
            ComboBox_Loaded_Window(sender, null);
        }

        /// <summary>
        /// Window Combo - Loaded
        /// </summary>
        private void ComboBox_Loaded_Window(object sender, System.Windows.RoutedEventArgs e)
        {
            mWindowComboBox = (ComboBox)sender;

            if (ApiInformation.IsApiContractPresent(typeof(Windows.Foundation.UniversalApiContract).FullName, 8))
            {
                var processesWithWindows = from p in Process.GetProcesses()
                                           where !string.IsNullOrWhiteSpace(p.MainWindowTitle) && WindowEnumerationHelper.IsWindowValidForCapture(p.MainWindowHandle)
                                           select p;
                ((ComboBox)sender).ItemsSource = processesWithWindows;
                ((ComboBox)sender).IsEnabled = true;
            }
            else
            {
                ((ComboBox)sender).IsEnabled = false;
            }

            if (e != null)
                ((ComboBox)sender).SelectedIndex = -1;
            else
            {
                mWindowComboBox.SelectedIndex = -1;
                mMonitorComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Monitor Combo - SelectionChanged
        /// </summary>
        private void ComboBox_SelectionChanged_Monitor(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
                ((SideMenuControlViewModel)DataContext).SelectScreenCaptureWindowCommand.Execute(new ScreenCaptureHandle(((MonitorInfo)items[0]).Hmon, false));
        }

        /// <summary>
        /// Monitor Combo - DropDownOpened
        /// </summary>
        private void ComboBox_DropDownOpened_Monitor(object sender, System.EventArgs e)
        {
            ((SideMenuControlViewModel)DataContext).StopScreenCaptureCommand.Execute(null);
            ComboBox_Loaded_Monitor(sender, null);
        }

        /// <summary>
        /// Monitor Combo - Loaded
        /// </summary>
        private void ComboBox_Loaded_Monitor(object sender, System.Windows.RoutedEventArgs e)
        {
            mMonitorComboBox = (ComboBox)sender;

            if (ApiInformation.IsApiContractPresent(typeof(Windows.Foundation.UniversalApiContract).FullName, 8))
            {
                ((ComboBox)sender).ItemsSource = MonitorEnumerationHelper.GetMonitors();
                ((ComboBox)sender).IsEnabled = true;
            }
            else
            {
                ((ComboBox)sender).IsEnabled = false;
            }

            if (e != null)
                ((ComboBox)sender).SelectedIndex = -1;
            else
            {
                mWindowComboBox.SelectedIndex = -1;
                mMonitorComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// On isScreenCaptureActive change
        /// </summary>
        private void Button_IsEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                if (mWindowComboBox != null)
                    mWindowComboBox.SelectedIndex = -1;
                if (mMonitorComboBox != null)
                    mMonitorComboBox.SelectedIndex = -1;
            }
        }
    }
}
