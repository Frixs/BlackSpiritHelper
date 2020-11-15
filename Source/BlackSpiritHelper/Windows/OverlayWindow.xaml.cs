using BlackSpiritHelper.Core;
using Composition.WindowsRuntimeHelpers;
using System;
using System.Diagnostics;
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
        /// TODO .
        /// </summary>
        private int mDragBorderMargin = 25;

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

        #endregion

        #region Constructor

        public OverlayWindow()
        {
            InitializeComponent();

            // Set brush colors.
            mOverlayBackgroundBrush = (Brush)FindResource("TransparentBrushKey");
            mOverlayBackgroundBrushNoItems = (Brush)FindResource("RedBrushKey");

#if DEBUG
            // Force graphicscapture.dll to load.
            var picker = new GraphicsCapturePicker();
#endif
        }

        #endregion

        #region Window Methods

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr hWndMainApp = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr hWndOverlay = new WindowInteropHelper(this).Handle;

            if (hWndMainApp == IntPtr.Zero)
            {
                IoC.Logger.Log("App main window not found!", LogLevel.Error);
                mIsOverlayOk = false;
                return;
            }

            //this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
            Topmost = true;

            SetOverlayPosition(hWndOverlay, hWndMainApp);
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
            if (IoC.DataContent.OverlayData.IsDraggingLocked)
                return;

            // Get clicked object.
            Mouse.Capture(sender as FrameworkElement);
            // Get relative mouse position within overlay object.
            mOverlayObjectMouseRelPos = e.GetPosition(sender as UIElement);
        }

        /// <summary>
        /// On MOUSE-MOVE
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IoC.DataContent.OverlayData.IsDraggingLocked)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            // Set X axis.
            Canvas.SetLeft(sender as FrameworkElement,
                e.GetPosition(null).X - mOverlayObjectMouseRelPos.X
                );
            // Set Y axis.
            Canvas.SetTop(sender as FrameworkElement,
                e.GetPosition(null).Y - mOverlayObjectMouseRelPos.Y
                );

            // TODO .
            mCaptureCompositionRootContainer.Offset = new Vector3(IoC.DataContent.OverlayData.ScreenCaptureOverlay.PosX + mDragBorderMargin, IoC.DataContent.OverlayData.ScreenCaptureOverlay.PosY + mDragBorderMargin, 0);

            // e.GetPosition((sender as FrameworkElement).Parent as FrameworkElement).Y
        }

        /// <summary>
        /// On MOUSE-UP
        /// </summary>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IoC.DataContent.OverlayData.IsDraggingLocked)
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
        /// <param name="hwndOverlay">Overlay window handle.</param>
        /// <param name="hwndMainApp">Target window handle.</param>
        private void SetOverlayPosition(IntPtr hwndOverlay, IntPtr hwndMainApp)
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                foreach (TimerItemDataViewModel t in g.TimerList)
                {
                    if (t.ShowInOverlay)
                    {
                        val.Background = mOverlayBackgroundBrush;
                        return;
                    }
                }
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
        /// When <see cref="IoC.DataContent.OverlayData.IsScreenCaptureActive"/> got changed
        /// </summary>
        private void ScreenCaptureOverlayObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue;

            if (value)
                StartCapture();
            else
                StopCapture();
        }

        #endregion

        #region ScreenCapture Capture

        private void InitScreenCaptureComposition()
        {
            var controlsWidth = System.Windows.Forms.Screen.FromHandle(mWindowHandle).Bounds.Width / 2f;
            var controlsHeight = System.Windows.Forms.Screen.FromHandle(mWindowHandle).Bounds.Height / 2f;

            // Create the compositor.
            mCaptureCompositor = new Compositor();

            // Create a target for the window.
            mCaptureCompositionTarget = mCaptureCompositor.CreateDesktopWindowTarget(mWindowHandle, false);

            // Attach the root visual.
            mCaptureCompositionRootContainer = mCaptureCompositor.CreateContainerVisual();
            mCaptureCompositionRootContainer.RelativeSizeAdjustment = Vector2.Zero;
            mCaptureCompositionRootContainer.RelativeOffsetAdjustment = Vector3.Zero;
            mCaptureCompositionRootContainer.Size = new Vector2(controlsWidth, controlsHeight);
            mCaptureCompositionRootContainer.Offset = new Vector3(IoC.DataContent.OverlayData.ScreenCaptureOverlay.PosX + mDragBorderMargin, IoC.DataContent.OverlayData.ScreenCaptureOverlay.PosY + mDragBorderMargin, 0);
            mCaptureCompositionRootContainer.AnchorPoint = new Vector2(0, 0);
            mCaptureCompositionRootContainer.Scale = new Vector3(1, 1, 0);
            mCaptureCompositionRootContainer.Opacity = 0.9f;
            mCaptureCompositionTarget.Root = mCaptureCompositionRootContainer;

            // Setup the rest of the sample application.
            mCaptureHandler = new OverlayScreenCaptureCaptureHandler(mCaptureCompositor);
            mCaptureCompositionRootContainer.Children.InsertAtTop(mCaptureHandler.Visual);

            ScreenCaptureCanvas.Width = controlsWidth;
            ScreenCaptureCanvas.Height = controlsHeight;
        }

        /// <summary>
        /// Start screen capture
        /// </summary>
        private void StartCapture()
        {
            var sch = IoC.DataContent.OverlayData.ScreenCaptureHandleData;
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
