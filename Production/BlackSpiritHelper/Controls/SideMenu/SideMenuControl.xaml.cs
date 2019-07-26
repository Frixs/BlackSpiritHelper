using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        #region Constructor

        public SideMenuControl()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Open overlay window.
        /// </summary>
        private void OpenOverlay()
        {
            if (OverlayWindow.Window != null)
                return;

            OverlayWindow.Window = new OverlayWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            OverlayWindow.Window.Show();
        }

        /// <summary>
        /// Close overlay window.
        /// </summary>
        private void CloseOverlay()
        {
            if (OverlayWindow.Window == null)
                return;

            OverlayWindow.Window.Close();
            OverlayWindow.Window = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void ShowOverlayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OpenOverlay();
        }

        private void ShowOverlayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CloseOverlay();
        }
    }
}
