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
        //public const string WINDOW_NAME = "Black Desert - 327975";
        public const string WINDOW_NAME = "Calculator";
        /** Target window handle. */
        private IntPtr targetWindowHandle;

        //[DllImport("user32.dll")]
        //private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hwd, out WindowCornerPosition lpRect);

        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            IntPtr overlayWindowHandle = new WindowInteropHelper(this).Handle;
            targetWindowHandle = FindWindow(null, WINDOW_NAME);
            if (targetWindowHandle.Equals(IntPtr.Zero))
            {
                Console.WriteLine("Target window not found!");
                // TODO: make variable to dont show bad window, in Load method.
            }

            this.Background = new SolidColorBrush(Colors.LightGray); // For DEBUG.
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            //// Set transparency for mouse events.
            //int initialStyle = GetWindowLong(overlayWindowHandle, -20);
            //SetWindowLong(overlayWindowHandle, -20, initialStyle | 0x80000 | 0x20);

            // Create on the position of target window.
            WindowCornerPosition rectWindow;
            GetWindowRect(targetWindowHandle, out rectWindow);

            Screen[] screens = Screen.AllScreens;
            Screen screen = Screen.FromHandle(targetWindowHandle); //this is the Form class
            Console.WriteLine(screen);
            // TODO: Solve screen position & DPI.

            this.Width = rectWindow.right - rectWindow.left;
            this.Height = rectWindow.bottom - rectWindow.top;
            this.Top = rectWindow.top;
            this.Left = 0;// rectWindow.left;
        }

        // Inner struct to hold data about target window corner positions.
        public struct WindowCornerPosition
        {
            public int left, top, right, bottom;
        }

        enum DisplayAffinity : uint
        {
            None = 0,
            Monitor = 1
        }
    }
}
