using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Timer = 1,

        /// <summary>
        /// The Lifeskill page.
        /// </summary>
        Watchdog = 2,

        /// <summary>
        /// The Boss page.
        /// </summary>
        Boss = 3,

        ///
        /// Only pages with value lower than 100 can be loaded back on application start.
        /// If you want to add more pages change limitation in <see cref="ApplicationViewModel"/>.
        ///

        /// <summary>
        /// The group settings form.
        /// </summary>
        TimerGroupSettingsForm = 100,
    }
}
