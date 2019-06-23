using BlackSpiritTimerApp.Utilities;
using System;
using System.Windows;
using System.Windows.Interop;

namespace BlackSpiritTimerApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log manager instance.
        /// </summary>
        private Logger logger;

        /// <summary>
        /// Currently active overlay window instance.
        /// </summary>
        private OverlayWindow overlayWindow = null;

        public MainWindow()
        {
            logger = new Logger(this.GetType().ToString());
            InitializeComponent();
        }

        /// <summary>
        /// Get handle to this window.
        /// </summary>
        /// <returns>Window handle.</returns>
        private IntPtr getHandle()
        {
            return new WindowInteropHelper(this).Handle;
        }

        /// <summary>
        /// Open overlay window.
        /// </summary>
        private void openOverlay()
        {
            overlayWindow = new OverlayWindow(getHandle());
            overlayWindow.Show();
        }

        /// <summary>
        /// Close overlay window.
        /// </summary>
        private void closeOverlay()
        {
            overlayWindow.Close();
            overlayWindow = null;

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
