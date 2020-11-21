using BlackSpiritHelper.Core;
using Composition.WindowsRuntimeHelpers;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Graphics.Capture;
using Windows.UI.Composition;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        #region Static Members

        /// <summary>
        /// Active overlay window instance.
        /// </summary>
        public static OverlayWindow Window = null;

        /// <summary>
        /// Says if the window is transparent to any action like mouse clicks.
        /// </summary>
        public static bool IsActionTransparent = true;

        #endregion

        #region Private Members

        /// <summary>
        /// Current window handle
        /// </summary>
        private IntPtr mWindowHandle;

        // Scree capture handlers
        private Compositor mCaptureCompositor;
        private Windows.UI.Composition.CompositionTarget mCaptureCompositionTarget;
        private Windows.UI.Composition.ContainerVisual mCaptureCompositionRootContainer;
        private OverlayScreenCaptureCaptureHandler mCaptureHandler;

        /// <summary>
        /// Overlay data reference
        /// </summary>
        private OverlayDataViewModel mOverlayData;

        /// <summary>
        /// Says if the overlay window is ok to show.
        /// </summary>
        private bool mIsOverlayOk = true;

        /// <summary>
        /// Depository for the relative mouse position within overlay object.
        /// </summary>
        private Point mOverlayObjectMouseRelPos = default;

        /// <summary>
        /// Brush background overlay color.
        /// </summary>
        private readonly Brush mOverlayBackgroundBrush;

        /// <summary>
        /// Brush background overlay color if there are no items.
        /// </summary>
        private readonly Brush mOverlayBackgroundBrushNoItems;

        /// <summary>
        /// Max size of the screen capture
        /// </summary>
        private Vector2 mScreenCaptureMaxSize;

        #endregion

        #region Constructor

        public OverlayWindow()
        {
            InitializeComponent();

            // Set brush colors.
            mOverlayBackgroundBrush = (Brush)FindResource("TransparentBrushKey");
            mOverlayBackgroundBrushNoItems = (Brush)FindResource("RedBrushKey");
        }

        #endregion

        #region Window Methods

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr hWndMainApp = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            if (hWndMainApp == IntPtr.Zero)
            {
                IoC.Logger.Log("App main window not found!", LogLevel.Error);
                mIsOverlayOk = false;
                return;
            }

            // Save reference to overlay data
            mOverlayData = IoC.DataContent.OverlayData;

            //this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            Topmost = true;

            SetOverlayPosition(hWndMainApp);
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

            // Save window handle
            var interopWindow = new WindowInteropHelper(this);
            mWindowHandle = interopWindow.Handle;

            // Init screen capture functions
            InitScreenCaptureComposition();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StopCapture();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            this.SetWindowExTransparent();
        }

        #endregion

        #region Drag Overlay Methods

        /// <summary>
        /// On MOUSE-DOWN
        /// </summary>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (mOverlayData.IsDraggingLocked)
                return;

            // Get clicked object.
            Mouse.Capture(sender as FrameworkElement);
            // Get relative mouse position within overlay object.
            mOverlayObjectMouseRelPos = e.GetPosition(sender as FrameworkElement);
        }

        /// <summary>
        /// On MOUSE-MOVE
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMoveProcess(sender, e);
        }

        /// <summary>
        /// On MOUSE-MOVE - specific for screen capture
        /// </summary>
        private void ScreenCaptureOverlayObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (OnMouseMoveProcess(sender, e))
                // Update position of screen capture surface
                UpdateCaptureCompositionOffset(mOverlayData.ScreenCaptureOverlay.PosX, mOverlayData.ScreenCaptureOverlay.PosY);
        }

        /// <summary>
        /// On MOUSE-UP
        /// </summary>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (mOverlayData.IsDraggingLocked)
                return;

            // Reset clicked object.
            Mouse.Capture(null);
            // Reset position.
            mOverlayObjectMouseRelPos = new Point(-1, -1);
        }

        /// <summary>
        /// Handle mouse movement
        /// </summary>
        /// <returns>TRUE if movement was handled, FALSE otherwise</returns>
        private bool OnMouseMoveProcess(object sender, MouseEventArgs e)
        {
            if (mOverlayData.IsDraggingLocked)
                return false;

            if (e.LeftButton != MouseButtonState.Pressed)
                return false;

            // Relative position was not registered yet
            if (mOverlayObjectMouseRelPos.X < 0 || mOverlayObjectMouseRelPos.Y < 0)
                return false;

            // Set X axis.
            Canvas.SetLeft(sender as FrameworkElement,
                e.GetPosition(null).X - mOverlayObjectMouseRelPos.X
                );
            // Set Y axis.
            Canvas.SetTop(sender as FrameworkElement,
                e.GetPosition(null).Y - mOverlayObjectMouseRelPos.Y
                );

            // e.GetPosition((sender as FrameworkElement).Parent as FrameworkElement).Y

            return true;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Create overlay window on the position of target window.
        /// </summary>
        /// <param name="hwndMainApp">Target window handle.</param>
        private void SetOverlayPosition(IntPtr hwndMainApp)
        {
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(hwndMainApp);

            WindowState = WindowState.Normal; // Reset.

            // Move the window to the screen of the traget window.
            Left = screen.WorkingArea.Left;
            Top = screen.WorkingArea.Top;
            Width = screen.WorkingArea.Width;
            Height = screen.WorkingArea.Height;
        }

        #endregion

        #region Events

        /// <summary>
        /// Set Overlay content if there are no items.
        /// TODO:LATER: Think about changing this to automatic procedure. We do not want to add or update this check everytime, we change the condition in code or if we add a new section.
        /// </summary>
        private void OverlayContentWrapper_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StackPanel val;

            if (sender.GetType().Equals(typeof(StackPanel)))
                val = (StackPanel)sender;
            else
            {
                IoC.Logger.Log($"The target is in invalid type ({sender.GetType().ToString()})!", LogLevel.Fatal);
                throw new InvalidOperationException($"The target is in invalid type ({sender.GetType().ToString()})!");
            }

            // Reset.
            val.ToolTip = default;

            // Timer section check.
            foreach (TimerGroupDataViewModel g in IoC.DataContent.TimerData.GroupList)
            {
                if (!g.IgnoreInOverlay)
                {
                    foreach (TimerItemDataViewModel t in g.TimerList)
                    {
                        if (t.ShowInOverlay)
                        {
                            val.Background = mOverlayBackgroundBrush;
                            return;
                        }
                    }
                }
            }

            // Apm Calculator section check.
            if (IoC.DataContent.ApmCalculatorData.ShowInOverlay)
            {
                val.Background = mOverlayBackgroundBrush;
                return;
            }

            // Schedule section check.
            if (IoC.DataContent.ScheduleData.ShowInOverlay && IoC.DataContent.ScheduleData.IsRunning)
            {
                val.Background = mOverlayBackgroundBrush;
                return;
            }

            // No items.
            val.Background = mOverlayBackgroundBrushNoItems;
            val.ToolTip = "No items to display.";
            return;
        }

        /// <summary>
        /// When <see cref="mOverlayData.IsScreenCaptureActive"/> got changed
        /// </summary>
        private void ScreenCaptureOverlayObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;

            if (value)
                StartCapture();
            else
                StopCapture();
        }

        /// <summary>
        /// PosX update trigger
        /// </summary>
        private void Slider_ValueChanged_PosY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mOverlayData == null)
                return;

            // We do not want to update position here - but we want to catch only certain position change events
            // Y resets after X - so do Y because X is already reset - otherwise Y would be still unreset
            if (mOverlayData.ScreenCaptureOverlay.PosY == 0) // Reset Position BTN - resets to zero
            {
                // Update position of screen capture surface
                UpdateCaptureCompositionOffset(mOverlayData.ScreenCaptureOverlay.PosX, mOverlayData.ScreenCaptureOverlay.PosY);
            }
        }

        /// <summary>
        /// Scale update trigger
        /// </summary>
        private void Slider_ValueChanged_Scale(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mOverlayData != null)
                UpdateCaptureCompositionScale(mOverlayData.ScreenCaptureOverlay.Scale);
        }

        /// <summary>
        /// Opacity update trigger
        /// </summary>
        private void Slider_ValueChanged_Opacity(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mOverlayData != null)
                UpdateCaptureCompositionOpacity(mOverlayData.ScreenCaptureOverlay.Opacity);
        }

        #endregion

        #region ScreenCapture Capture

        /// <summary>
        /// Initialize screen capture in the window
        /// </summary>
        private void InitScreenCaptureComposition()
        {
            // Maximal size of the scrreen capture surface
            mScreenCaptureMaxSize = new Vector2(
                System.Windows.Forms.Screen.FromHandle(mWindowHandle).Bounds.Width / 2f,
                System.Windows.Forms.Screen.FromHandle(mWindowHandle).Bounds.Height / 2f
                );

            // Create the compositor.
            mCaptureCompositor = new Compositor();

            // Create a target for the window.
            mCaptureCompositionTarget = mCaptureCompositor.CreateDesktopWindowTarget(mWindowHandle, false);

            // Attach the root visual.
            mCaptureCompositionRootContainer = mCaptureCompositor.CreateContainerVisual();
            mCaptureCompositionRootContainer.RelativeSizeAdjustment = Vector2.Zero;
            mCaptureCompositionRootContainer.RelativeOffsetAdjustment = Vector3.Zero;
            mCaptureCompositionRootContainer.Size = mScreenCaptureMaxSize;
            UpdateCaptureCompositionOffset(mOverlayData.ScreenCaptureOverlay.PosX, mOverlayData.ScreenCaptureOverlay.PosY);
            UpdateCaptureCompositionScale(mOverlayData.ScreenCaptureOverlay.Scale);
            UpdateCaptureCompositionOpacity(mOverlayData.ScreenCaptureOverlay.Opacity);
            mCaptureCompositionRootContainer.AnchorPoint = new Vector2(0, 0);
            mCaptureCompositionTarget.Root = mCaptureCompositionRootContainer;

            // Setup the rest of the sample application.
            mCaptureHandler = new OverlayScreenCaptureCaptureHandler(mCaptureCompositor);
            mCaptureCompositionRootContainer.Children.InsertAtTop(mCaptureHandler.Visual);
        }

        /// <summary>
        /// Update offset of the capture surface
        /// </summary>
        private void UpdateCaptureCompositionOffset(float x, float y)
        {
            if (mCaptureCompositionRootContainer == null)
                return;

            mCaptureCompositionRootContainer.Offset = new Vector3(
                x + 25,
                y + 25,
                0
                );
        }

        /// <summary>
        /// Update scale of the capture surface
        /// </summary>
        private void UpdateCaptureCompositionScale(float scale)
        {
            if (mCaptureCompositionRootContainer == null)
                return;

            mCaptureCompositionRootContainer.Scale = new Vector3(scale, scale, 0);

            ScreenCaptureCanvas.Width = mScreenCaptureMaxSize.X * scale;
            ScreenCaptureCanvas.Height = mScreenCaptureMaxSize.Y * scale;
        }

        /// <summary>
        /// Update opacity of the capture surface
        /// </summary>
        private void UpdateCaptureCompositionOpacity(float opacity)
        {
            if (mCaptureCompositionRootContainer == null)
                return;

            mCaptureCompositionRootContainer.Opacity = opacity;
        }

        /// <summary>
        /// Start screen capture
        /// </summary>
        private void StartCapture()
        {
            var sch = mOverlayData.ScreenCaptureHandleData;
            if (sch == null)
            {
                IoC.Logger.Log("Undefined screen capture handle!", LogLevel.Error);
                return;
            }

            // Stop capture first
            StopCapture();

            // Window
            if (sch.isWindow)
            {
                try
                {
                    GraphicsCaptureItem item = CaptureHelper.CreateItemForWindow(sch.handle);
                    if (item != null)
                        mCaptureHandler.StartCaptureFromItem(item);
                }
                catch (Exception)
                {
                    IoC.Logger.Log($"Hwnd 0x{sch.handle.ToInt32():X8} is not valid for capture!", LogLevel.Warning);
                }
            }
            // Monitor
            else
            {
                try
                {
                    GraphicsCaptureItem item = CaptureHelper.CreateItemForMonitor(sch.handle);
                    if (item != null)
                        mCaptureHandler.StartCaptureFromItem(item);
                }
                catch (Exception)
                {
                    IoC.Logger.Log($"Hmon 0x{sch.handle.ToInt32():X8} is not valid for capture!", LogLevel.Warning);
                }
            }
        }

        /// <summary>
        /// Stop screen capture
        /// </summary>
        private void StopCapture()
        {
            mCaptureHandler?.StopCapture();
        }

        #endregion
    }
}
