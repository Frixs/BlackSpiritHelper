using System;
using System.Windows.Threading;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// View model that represents timer. ViewModel for TimerListItemControl.
    /// </summary>
    public class TimerItemViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// DIspatcherTimer to control the timer.
        /// </summary>
        private DispatcherTimer mTimer;

        #endregion

        #region Static Limitation Properties

        /// <summary>
        /// Limitation for min characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMinChar { get; private set; } = 3;

        /// <summary>
        /// Limitation for max characters in <see cref="Title"/>.
        /// </summary>
        public static byte TitleAllowMaxChar { get; private set; } = 20;

        /// <summary>
        /// Limitation for min characters in <see cref="IconTitleShortcut"/>.
        /// </summary>
        public static byte IconTitleAllowMinChar { get; private set; } = 1;

        /// <summary>
        /// Limitation for max characters in <see cref="IconTitleShortcut"/>.
        /// </summary>
        public static byte IconTitleAllowMaxChar { get; private set; } = 3;

        /// <summary>
        /// Limitation for max duration in <see cref="CountdownDuration"/>.
        /// </summary>
        public static TimeSpan CountdownAllowMaxDuration { get; private set; } = TimeSpan.FromSeconds(7200);

        #endregion

        #region Public Properties

        /// <summary>
        /// The group id the timer belongs to.
        /// </summary>
        public byte GroupID { get; set; }

        /// <summary>
        /// Timer title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Text shortcut to show instead of image icon. Only if there is no image icon.
        /// </summary>
        public string IconTitleShortcut { get; set; }

        /// <summary>
        /// Icon background if there is no image icon.
        /// Color representation in HEX.
        /// f.e. FA9BC2
        /// </summary>
        public string IconBackgroundHEX { get; set; }

        /// <summary>
        /// Formatted time output.
        /// </summary>
        public string TimeFormat { get; set; }

        /// <summary>
        /// Countdown before timer starts.
        /// </summary>
        public TimeSpan CountdownDuration { get; set; }

        /// <summary>
        /// Button control.
        /// </summary>
        public TimerState State { get; set; }

        /// <summary>
        /// Says, the timer is currently playing (True) or it is another state (False).
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Says, if the timer is in infinite loop.
        /// </summary>
        public bool IsLoopActive { get; set; }

        /// <summary>
        /// Says, if the timer is in warning time (less than X).
        /// </summary>
        public bool IsWarningTime { get; set; }

        #endregion

        #region Constructor

        public TimerItemViewModel()
        {
            mTimer = new DispatcherTimer();
        }

        #endregion
    }
}
