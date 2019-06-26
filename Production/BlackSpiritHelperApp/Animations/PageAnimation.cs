using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Animations
{
    /// <summary>
    /// Styles of page animations for appearing/disappearing.
    /// </summary>
    public enum PageAnimation
    {
        /// <summary>
        /// No animation.
        /// </summary>
        None = 0,

        /// <summary>
        /// The page slides in and fades in from the bottom.
        /// </summary>
        SlideAndFadeInFromBottom = 1,

        /// <summary>
        /// The page slides out and fades out to the top.
        /// </summary>
        SlideAndFadeOutToBottom = 2,
    }
}
