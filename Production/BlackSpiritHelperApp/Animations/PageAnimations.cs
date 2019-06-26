using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BlackSpiritHelper.Animations
{
    /// <summary>
    /// Helpers to animate pages in specific ways.
    /// </summary>
    public static class PageAnimations
    {
        /// <summary>
        /// Slides a page in from the bottom.
        /// </summary>
        /// <param name="page">The page to animate.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="seconds">The distance to slide.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromBottom(this Page page, float seconds, int slideDistance)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide from bottom animation.
            sb.AddSlideInFromBottom(seconds, slideDistance);

            // Add fade in animation.
            sb.AddFadeIn(seconds);

            // Start animating.
            sb.Begin(page);

            // Make page visible.
            page.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int) (seconds * 1000));
        }

        /// <summary>
        /// Slides a page out to the bottom.
        /// </summary>
        /// <param name="page">The page to animate.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="seconds">The distance to slide.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToBottom(this Page page, float seconds, int slideDistance)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide from bottom animation.
            sb.AddSlideOutToBottom(seconds, slideDistance);

            // Add fade in animation.
            sb.AddFadeOut(seconds);

            // Start animating.
            sb.Begin(page);

            // Make page visible.
            page.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
