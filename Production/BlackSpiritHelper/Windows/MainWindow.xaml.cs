using System;
using System.Windows;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Currently active overlay window instance.
        /// </summary>
        private OverlayWindow mOverlayWindow = null;

        #endregion

        #region Public Properties

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WindowViewModel(this);
        }

        #endregion

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
