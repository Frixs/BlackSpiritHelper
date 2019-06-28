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
        #region Private Members

        /// <summary>
        /// Currently active overlay window instance.
        /// </summary>
        private OverlayWindow mOverlayWindow = null;

        #endregion

        #region Constructor

        public SideMenuControl()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Open overlay window.
        /// </summary>
        private void openOverlay()
        {
            mOverlayWindow = new OverlayWindow(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            mOverlayWindow.Show();
        }

        /// <summary>
        /// Close overlay window.
        /// </summary>
        private void closeOverlay()
        {
            mOverlayWindow.Close();
            mOverlayWindow = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void ShowOverlayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            openOverlay();
        }

        private void ShowOverlayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            closeOverlay();
        }
    }
}
