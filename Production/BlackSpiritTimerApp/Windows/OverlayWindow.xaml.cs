using System;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;

namespace BlackSpiritTimerApp.Windows
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        /** Target window name. */
        private const string WindowName = "Calculator"; // TODO.

        private bool isOverlayOk = true;

        //[DllImport("user32.dll")]
        //private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool GetWindowRect(IntPtr hwd, out WindowCornerPosition lpRect);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr overlayWindowHandle = new WindowInteropHelper(this).Handle;
            IntPtr targetWindowHandle;

        targetWindowHandle = FindWindow(null, WindowName);
            if (targetWindowHandle.Equals(IntPtr.Zero))
            {
                Console.WriteLine("Target window not found!"); // TODO - logger.
                isOverlayOk = false;
                return;
            }

            //this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            //// Set transparency for mouse events.
            //int initialStyle = GetWindowLong(overlayWindowHandle, -20);
            //SetWindowLong(overlayWindowHandle, -20, initialStyle | 0x80000 | 0x20);

            SetOverlayPosition(overlayWindowHandle, targetWindowHandle);
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isOverlayOk)
            {
                this.Close();
                return;
            }

            this.WindowState = WindowState.Maximized;
        }

        /**
         * Create overlay window on the position of target window.
         */
        private void SetOverlayPosition(IntPtr overlayWindowHandle, IntPtr targetWindowHandle)
        {
            //WindowCornerPosition rectWindow;
            Screen screen = null;

            //GetWindowRect(targetWindowHandle, out rectWindow);

            screen = Screen.FromHandle(targetWindowHandle);

            this.WindowState = WindowState.Normal; // Reset.

            this.Left = screen.WorkingArea.Left;
            this.Top = screen.WorkingArea.Top;
            this.Width = screen.WorkingArea.Width;
            this.Height = screen.WorkingArea.Height;
        }

        /** 
         * Inner struct to hold data about target window corner positions. 
         */
        //public struct WindowCornerPosition
        //{
        //    public int left, top, right, bottom;
        //}
    }
}
