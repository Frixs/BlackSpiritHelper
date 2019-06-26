using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Animation helpers for <see cref="StoryBoard"/>.
    /// </summary>
    public static class StoryboardHelpers
    {
        /// <summary>
        /// Adds a slide in from left animation to the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="offset">The distance to the left to start from.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="deceleration">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideInFromLeft(this Storyboard storyboard, double offset, float seconds, float deceleration = 0.9f, bool keepMargin = true)
        {
            // Create the margin animate from right.
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(-offset, 0, (keepMargin ? offset : 0), 0),
                To = new Thickness(0),
                DecelerationRatio = deceleration
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide out to left animation to the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="offset">The distance to the left to start from.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="deceleration">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideOutToLeft(this Storyboard storyboard, double offset, float seconds, float deceleration = 0.9f, bool keepMargin = true)
        {
            // Create the margin animate from right.
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, (keepMargin ? offset : 0), 0),
                DecelerationRatio = deceleration
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide from bottom animation in the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="offset">The distance to the bottom to start from</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="deceleration">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width/height during animation.</param>
        public static void AddSlideInFromBottom(this Storyboard storyboard, double offset, float seconds, float deceleration = 0.9f, bool keepMargin = true)
        {
            // Create the margin animation from the bottom.
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0, (keepMargin ? offset : 0), 0, -offset),
                To = new Thickness(0),
                DecelerationRatio = deceleration
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide to bottom animation in the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="offset">The distance to the bottom to start from</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="deceleration">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width/height during animation.</param>
        public static void AddSlideOutToBottom(this Storyboard storyboard, double offset, float seconds, float deceleration = 0.9f, bool keepMargin = true)
        {
            // Create the margin animation from the bottom.
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(0, (keepMargin ? offset : 0), 0, -offset),
                DecelerationRatio = deceleration
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds fade in animation in the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            // Create the margin animation from the bottom.
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1,
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// Adds fade out animation in the storyboard.
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            // Create the margin animation from the bottom.
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0,
            };

            // Set the target property name.
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard.
            storyboard.Children.Add(animation);
        }
    }
}
