using System;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;
using BlackSpiritHelper.Utilities;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Log manager instance.
        /// </summary>
        private Logger mLogger;

        /// <summary>
        /// Target window handle where to open overlay window.
        /// </summary>
        private IntPtr mTargetWindowHandle;

        ///// <summary>
        ///// Target window name.
        ///// </summary>
        //private const string WindowName = "Calculator";

        /// <summary>
        /// Says if the overlay window is ok to show.
        /// </summary>
        private bool mIsOverlayOk = true;

        #endregion

        #region Constructor

        public OverlayWindow(IntPtr targetWindowReference)
        {
            mLogger = new Logger(this.GetType().ToString());
            this.mTargetWindowHandle = targetWindowReference;
            InitializeComponent();
        }

        #endregion

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr overlayWindowHandle = new WindowInteropHelper(this).Handle;

            if (mTargetWindowHandle.Equals(IntPtr.Zero))
            {
                mLogger.Warn("Target window not found!");
                mIsOverlayOk = false;
                return;
            }

            //this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            this.Topmost = true;

            SetOverlayPosition(overlayWindowHandle, mTargetWindowHandle);
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!mIsOverlayOk)
            {
                this.Close();
                return;
            }

            this.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Create overlay window on the position of target window.
        /// </summary>
        /// <param name="overlayWindowHandle">Overlay window handle.</param>
        /// <param name="targetWindowHandle">Target window handle.</param>
        private void SetOverlayPosition(IntPtr overlayWindowHandle, IntPtr targetWindowHandle)
        {
            Screen screen = null;

            screen = Screen.FromHandle(targetWindowHandle);

            this.WindowState = WindowState.Normal; // Reset.

            this.Left = screen.WorkingArea.Left;
            this.Top = screen.WorkingArea.Top;
            this.Width = screen.WorkingArea.Width;
            this.Height = screen.WorkingArea.Height;
        }
    }
}
