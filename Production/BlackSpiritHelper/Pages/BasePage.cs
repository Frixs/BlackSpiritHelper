using BlackSpiritHelper.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BlackSpiritHelper
{
    /// <summary>
    /// A base page for all pages to gain base functionality.
    /// </summary>
    public class BasePage<VM> : Page
        where VM : BaseViewModel, new()
    {
        #region Private Members

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        private VM mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Distance of slide animation.
        /// </summary>
        public int SlideDistance { get; set; } = 25; //this.WindowHeight;

        /// <summary>
        /// The time any slide animation takes to complete.
        /// </summary>
        public float SlideSeconds { get; set; } = 0.6f;

        /// <summary>
        /// The animation to play when the page is first loaded.
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromBottom;

        /// <summary>
        /// The animation to play when the page is first unloaded.
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToBottom;

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        public VM ViewModel
        {
            get
            {
                return mViewModel;
            }
            set
            {
                // If nothing has changed, return.
                if (mViewModel == value)
                    return;

                mViewModel = value;

                // Set the data context for this page.
                this.DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasePage()
        {
            // If we are animation in, hide to begin with.
            if (this.PageLoadAnimation != PageAnimation.None)
            {
                this.Visibility = Visibility.Collapsed;
            }

            // Listen out for the page loading.
            this.Loaded += BasePage_LoadedAsync;

            // Create a default view model.
            this.ViewModel = new VM();
        }

        #endregion

        #region Animation Load / Unload

        /// <summary>
        /// Once the page is loaded, perform any required animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_LoadedAsync(object sender, RoutedEventArgs e)
        {
            // animate the page in.
            await AnimateInAsync();
        }

        /// <summary>
        /// Animates the page in.
        /// </summary>
        /// <returns></returns>
        public async Task AnimateInAsync()
        {
            // Make sure we have something to do.
            if (PageLoadAnimation == PageAnimation.None)
            {
                return;
            }

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromBottom:

                    // Start the animation.
                    await this.SlideAndFadeInFromBottom(SlideDistance, SlideSeconds);

                    break;
            }
        }

        /// <summary>
        /// Animates the page out.
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOutAsync()
        {
            // Make sure we have something to do.
            if (PageUnloadAnimation == PageAnimation.None)
            {
                return;
            }

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToBottom:

                    // Start the animation.
                    await this.SlideAndFadeOutToBottom(SlideDistance, SlideSeconds);

                    break;
            }
        }

        #endregion
    }
}
