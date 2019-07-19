using System;
using System.Windows;
using System.Windows.Interop;
using BlackSpiritHelper.Core;
using System.Windows.Input;
using System.Windows.Controls;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Target window handle where to open overlay window.
        /// </summary>
        private IntPtr mTargetWindowHandle;

        /// <summary>
        /// Says if the overlay window is ok to show.
        /// </summary>
        private bool mIsOverlayOk = true;

        /// <summary>
        /// Depository for the relative mouse position within overlay object.
        /// </summary>
        private Point mOverlayObjectMouseRelPos = default;

        #endregion

        #region Constructor

        public OverlayWindow(IntPtr targetWindowReference)
        {
            mTargetWindowHandle = targetWindowReference;
            InitializeComponent();
        }

        #endregion

        #region Window Methods

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr overlayWindowHandle = new WindowInteropHelper(this).Handle;

            if (mTargetWindowHandle.Equals(IntPtr.Zero))
            {
                IoC.Logger.Log("Target window not found!", LogLevel.Error);
                mIsOverlayOk = false;
                return;
            }

            //this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            Topmost = true;
            
            SetOverlayPosition(overlayWindowHandle, mTargetWindowHandle);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!mIsOverlayOk)
            {
                Close();
                return;
            }
            
            // Maximize the window.
            WindowState = WindowState.Maximized;
        }

        #endregion

        #region Drag Overlay Methods

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IoC.DataContent.OverlayDesignModel.IsDraggingLocked)
                return;

            // Get clicked object.
            Mouse.Capture(OverlayObject);
            // Get relative mouse position within overlay object.
            mOverlayObjectMouseRelPos = e.GetPosition(sender as UIElement);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IoC.DataContent.OverlayDesignModel.IsDraggingLocked)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            
            // Set X axis.
            Canvas.SetLeft(OverlayObject as FrameworkElement,
                e.GetPosition(null).X - mOverlayObjectMouseRelPos.X
                );
            // Set Y axis.
            Canvas.SetTop(OverlayObject as FrameworkElement,
                e.GetPosition(null).Y - mOverlayObjectMouseRelPos.Y
                );

            // e.GetPosition((sender as FrameworkElement).Parent as FrameworkElement).Y
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IoC.DataContent.OverlayDesignModel.IsDraggingLocked)
                return;

            // Reset clicked object.
            Mouse.Capture(null);
            // Reset position.
            mOverlayObjectMouseRelPos = default;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Create overlay window on the position of target window.
        /// </summary>
        /// <param name="overlayWindowHandle">Overlay window handle.</param>
        /// <param name="targetWindowHandle">Target window handle.</param>
        private void SetOverlayPosition(IntPtr overlayWindowHandle, IntPtr targetWindowHandle)
        {
            System.Windows.Forms.Screen screen = null;

            screen = System.Windows.Forms.Screen.FromHandle(targetWindowHandle);

            WindowState = WindowState.Normal; // Reset.

            // Move the window to the screen of the traget window.
            Left = screen.WorkingArea.Left;
            Top = screen.WorkingArea.Top;
            Width = screen.WorkingArea.Width;
            Height = screen.WorkingArea.Height;
        }

        #endregion
    }
}
