using BlackSpiritHelper.Core;
using System.Windows;
using System.Windows.Input;

namespace BlackSpiritHelper
{
    /// <summary>
    /// The View Model for the custom flat window.
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Static Members

        /// <summary>
        /// Windows tray icon of the application.
        /// ---
        /// TODO:LATER: Tray icon - not important - We need to access it from IoC to be able to update it from other parts of the code.
        /// </summary>
        public static System.Windows.Forms.NotifyIcon TrayIcon = null;

        #endregion

        #region Private Members

        /// <summary>
        /// The window his view model controls.
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The height of the space bar of the window.
        /// </summary>
        private int mSpaceBarHeight = 8;

        /// <summary>
        /// The size of the resize border around the window.
        /// </summary>
        private int mResizeBorderSize = 4;

        /// <summary>
        /// The margin around the window to allow for a drop shadow.
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window.
        /// </summary>
        private int mWindowRadius = 0;

        #endregion

        #region Public Properties

        /// <summary>
        /// The size of the resize border around the window.
        /// </summary>
        public int ResizeBorderSize 
        { 
            get 
            { 
                return mWindow.WindowState == WindowState.Maximized ? 0 : mResizeBorderSize; 
            } 
            set 
            { 
                mResizeBorderSize = value; 
            } 
        }

        /// <summary>
        /// The thickness of the resize border around the window, taking into account the outer margin.
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorderSize + OuterMarginSize);

        /// <summary>
        /// The margin around the window to allow for a drop shadow.
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The margin thickness around the window to allow for a drop shadow.
        /// </summary>
        public Thickness OuterMarginThickness => new Thickness(OuterMarginSize);

        /// <summary>
        /// The radius of the edges of the window.
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// The radius of the edges of the window.
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the title bar.
        /// </summary>
        public int TitleBarHeight { get; set; } = 40;

        /// <summary>
        /// The height of the title bar.
        /// </summary>
        public GridLength TitleBarHeightGridLength => new GridLength(TitleBarHeight);

        /// <summary>
        /// The height of the space bar of the window.
        /// </summary>
        public int SpaceBarHeight
        {
            get
            {
                return mSpaceBarHeight;
            }
            set
            {
                mSpaceBarHeight = value;
                OnPropertyChanged(nameof(SpaceBarHeightGridLength));
                OnPropertyChanged(nameof(CaptionHeight));
                OnPropertyChanged(nameof(CaptionOverlayHeight));
            }
        }

        /// <summary>
        /// The height of the space bar of the window.
        /// </summary>
        public GridLength SpaceBarHeightGridLength => new GridLength(SpaceBarHeight);

        /// <summary>
        /// The height of the caption of the window - draggable part of the window.
        /// </summary>
        public int CaptionHeight => TitleBarHeight + mSpaceBarHeight - ResizeBorderSize;

        /// <summary>
        /// Overlay to ignore Caption Height.
        /// </summary>
        public int CaptionOverlayHeight => CaptionHeight + 1;

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window.
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window.
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window.
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// The command to save settings
        /// </summary>
        public ICommand SaveSettingsCommand { get; set; }

        /// <summary>
        /// The command to close the window to tray.
        /// </summary>
        public ICommand CloseTrayCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window.
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing.
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize. (Ignore these events on window resize.)
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Create commands.
            CreateCommands();

            // Fix window resize issue.
            var resizer = new WindowResizer(mWindow);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create Windows commands.
        /// </summary>
        private void CreateCommands()
        {
            // Minimize.
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);

            // Maximize.
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);

            // Close.
            ExitCommand = new RelayCommand(() => IoC.Application.Exit());

            // Menu.
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Save settings
            SaveSettingsCommand = new RelayCommand(() => IoC.Application.AppAssembly.Restart($"{ApplicationArgument.SaveSettings}="));

            // Application MainWindow close to Tray.
            CloseTrayCommand = new RelayCommand(() => IoC.UI.CloseMainWindowToTray());
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Dispose <see cref="TrayIcon"/>.
        /// </summary>
        public static void DisposeTrayIcon()
        {
            if (TrayIcon == null)
                return;

            TrayIcon.Dispose();
            TrayIcon = null;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the current mouse position on the screen.
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window.
            var position = Mouse.GetPosition(mWindow);

            // Add the window position so its a "ToScreen".
            return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
        }

        #endregion
    }
}
