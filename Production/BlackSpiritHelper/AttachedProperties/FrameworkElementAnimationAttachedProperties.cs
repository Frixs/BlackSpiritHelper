﻿using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// A base class to run any animation method when a boolean is set to true and a reverse animation when set to false.
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
        where Parent : BaseAttachedProperty<Parent, bool>, new()
    {
        #region Public Properties

        /// <summary>
        /// A flag indicating if this is the first time this property has been loaded.
        /// </summary>
        public bool FirstLoad { get; set; } = true;

        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            // Get the framework element.
            if (!(sender is FrameworkElement element))
                return;

            // Don't fire if the value doesn't change.
            if (sender.GetValue(ValueProperty) == value && !FirstLoad)
                return;

            // On first load...
            if (FirstLoad)
            {
                // Create a single self-unhookable event for the elements Loaded event.
                RoutedEventHandler onLoaded = null;
                onLoaded = (ss, ee) =>
                {
                    // Unhook ourselves.
                    element.Loaded -= onLoaded;

                    // Do desired animation.
                    DoAnimation(element, (bool)value);

                    // No longer in first load.
                    FirstLoad = false;
                };

                // Hook into the Loaded event of the element.
                element.Loaded += onLoaded;
            }
            else
            {
                // Do desired animation.
                DoAnimation(element, (bool)value);
            }
        }

        /// <summary>
        /// The animation method that is fired when the value changes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The new value.</param>
        protected virtual void DoAnimation(FrameworkElement element, bool value) { }
    }

    /// <summary>
    /// Animates a framework element sliding it in from the left on show and sliding out to the left on hide.
    /// </summary>
    public class AnimateSideMenuSlideIn : AnimateBaseProperty<AnimateSideMenuSlideIn>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            await element.SlideAndFadeInFromLeft(25, 0.6f, keepMargin: false);

            //if (value)
            //    // Animate in.
            //    await element.SlideAndFadeInFromLeft((int)element.ActualWidth, (FirstLoad ? 0f : 0.3f), keepMargin: false);
            //else
            //    // Animate out.
            //    await element.SlideAndFadeOutToLeft((int)element.ActualWidth, (FirstLoad ? 0 : 0.3f), keepMargin: false);
        }
    }
}
