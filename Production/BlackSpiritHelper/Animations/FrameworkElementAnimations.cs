using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways.
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Slides a element in from the left.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="slideDistance">The distance to slide.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this FrameworkElement element, int slideDistance, float seconds = 0.3f, bool keepMargin = true)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide animation.
            sb.AddSlideInFromLeft(slideDistance, seconds, keepMargin: keepMargin);

            // Add fade in animation.
            sb.AddFadeIn(seconds);

            // Start animating.
            sb.Begin(element);

            // Make element visible.
            element.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides a element out to the left.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="slideDistance">The distance to slide.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this FrameworkElement element, int slideDistance, float seconds = 0.3f, bool keepMargin = true)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide animation.
            sb.AddSlideOutToLeft(slideDistance, seconds, keepMargin: keepMargin);

            // Add fade in animation.
            sb.AddFadeOut(seconds);

            // Start animating.
            sb.Begin(element);

            // Make element visible.
            element.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides a element in from the bottom.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="slideDistance">The distance to slide.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromBottom(this FrameworkElement element, int slideDistance, float seconds = 0.3f, bool keepMargin = true)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide animation.
            sb.AddSlideInFromBottom(slideDistance, seconds, keepMargin: keepMargin);

            // Add fade in animation.
            sb.AddFadeIn(seconds);

            // Start animating.
            sb.Begin(element);

            // Make element visible.
            element.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int) (seconds * 1000));
        }

        /// <summary>
        /// Slides a element out to the bottom.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="slideDistance">The distance to slide.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToBottom(this FrameworkElement element, int slideDistance, float seconds = 0.3f, bool keepMargin = true)
        {
            // Create the storyboard.
            var sb = new Storyboard();

            // Add slide animation.
            sb.AddSlideOutToBottom(slideDistance, seconds, keepMargin: keepMargin);

            // Add fade in animation.
            sb.AddFadeOut(seconds);

            // Start animating.
            sb.Begin(element);

            // Make element visible.
            element.Visibility = Visibility.Visible;

            // Wait for it to finish.
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
