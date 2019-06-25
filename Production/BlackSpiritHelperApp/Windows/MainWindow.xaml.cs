using BlackSpiritHelperApp.Utilities;
using BlackSpiritHelperApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Interop;

namespace BlackSpiritHelperApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log manager instance.
        /// </summary>
        private Logger mLogger;

        /// <summary>
        /// Currently active overlay window instance.
        /// </summary>
        private OverlayWindow mOverlayWindow = null;

        /// <summary>
        /// Window view model instance of the current window.
        /// </summary>
        private WindowViewModel mWindowViewModel = null;

        public MainWindow()
        {
            mLogger = new Logger(this.GetType().ToString());

            InitializeComponent();

            this.DataContext = mWindowViewModel = new WindowViewModel(this);
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
            mOverlayWindow = new OverlayWindow(getHandle());
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
