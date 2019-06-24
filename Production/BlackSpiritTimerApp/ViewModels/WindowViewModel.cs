using BlackSpiritTimerApp.Utilities;
using BlackSpiritTimerApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlackSpiritTimerApp.ViewModels
{
    /// <summary>
    /// The View Model for the custom flat window.
    /// </summary>
    class WindowViewModel : BaseViewModel
    {
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
        /// The height of the title bar.
        /// </summary>
        private int mTitleBarHeight = 48;

        /// <summary>
        /// The size of the resize border around the window.
        /// </summary>
        private int mResizeBorderSize = 2;

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
        public int ResizeBorderSize { get { return mResizeBorderSize; } set { mResizeBorderSize = value; } }

        /// <summary>
        /// The thickness of the resize border around the window, taking into account the outer margin.
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorderSize + OuterMarginSize); } }

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
        public Thickness OuterMarginThickness { get { return new Thickness(OuterMarginSize); } }

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
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar.
        /// </summary>
        public int TitleBarHeight { get { return mTitleBarHeight; } set { mTitleBarHeight = value; } }

        /// <summary>
        /// The height of the title bar.
        /// </summary>
        public GridLength TitleBarHeightGridLength { get { return new GridLength(TitleBarHeight); } }

        /// <summary>
        /// The height of the space bar of the window.
        /// </summary>
        public int SpaceBarHeight { get { return mSpaceBarHeight; } set { mSpaceBarHeight = value; } }

        /// <summary>
        /// The height of the space bar of the window.
        /// </summary>
        public GridLength SpaceBarHeightGridLength { get { return new GridLength(SpaceBarHeight); } }

        /// <summary>
        /// The height of the caption of the window - draggable part of the window.
        /// </summary>
        public int CaptionHeight { get { return mTitleBarHeight + mSpaceBarHeight - ResizeBorderSize; } }

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
        public ICommand CloseCommand { get; set; }

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
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue.
            var resizer = new WindowResizer(mWindow);
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
