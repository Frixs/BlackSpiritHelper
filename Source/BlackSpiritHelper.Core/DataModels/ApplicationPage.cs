using System.ComponentModel;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// <see langword="abstract"/>page of the application.
    /// </summary>
    public enum ApplicationPage
    {
        /// <summary>
        /// The home page with applcation description and welcome.
        /// </summary>
        Home = 0,

        /// <summary>
        /// The Combat page.
        /// </summary>
        [Description("Timer")]
        Timer = 1,

        /// <summary>
        /// The Lifeskill page.
        /// </summary>
        [Description("Watchdog")]
        Watchdog = 2,

        /// <summary>
        /// The Boss page.
        /// </summary>
        [Description("Schedule")]
        Schedule = 3,

        /// <summary>
        /// The Daily Check page.
        /// </summary>
        [Description("Daily Check")]
        DailyCheck = 4,

        /// <summary>
        /// The Settings page.
        /// </summary>
        Preferences = 10,

        ///
        /// Application content pages are until value 99. Set limit: <see cref="ApplicationViewModel.ApplicationContentPageValueLimit"/>.
        /// Only pages with value lower than 100 can be loaded back on application start.
        /// If you want to add more pages change limitation in <see cref="ApplicationViewModel"/>.
        ///

        /// <summary>
        /// The timergroup settings form.
        /// </summary>
        TimerGroupSettingsForm = 100,

        /// <summary>
        /// The timer item settings form.
        /// </summary>
        TimerItemSettingsForm = 101,

        /// <summary>
        /// The schedule template settings form.
        /// </summary>
        ScheduleTemplateSettingsForm = 102,

        /// <summary>
        /// The schedule item control form.
        /// </summary>
        ScheduleItemControlForm = 103,
    }
}
