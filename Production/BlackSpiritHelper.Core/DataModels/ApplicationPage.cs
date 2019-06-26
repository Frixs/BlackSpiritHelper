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
        Combat = 1,

        /// <summary>
        /// The Lifeskill page.
        /// </summary>
        Lifeskill = 2,

        /// <summary>
        /// The Boss page.
        /// </summary>
        Boss = 3,
    }
}
