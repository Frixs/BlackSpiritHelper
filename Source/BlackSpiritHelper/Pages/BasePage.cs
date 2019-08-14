using BlackSpiritHelper.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BlackSpiritHelper
{
    /// <summary>
    /// The Base page for all pages to gain base functionality.
    /// </summary>
    public class BasePage : Page
    {
        #region Private Members

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        private object mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Distance of slide animation.
        /// </summary>
        public int SlideDistance { get; set; } = 25; //this.WindowHeight;

        /// <summary>
        /// The time any slide animation takes to complete.
        /// </summary>
        public float SlideSeconds { get; set; } = 0.2f;

        /// <summary>
        /// The animation to play when the page is first loaded.
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromBottom;

        /// <summary>
        /// The animation to play when the page is first unloaded.
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToBottom;

        /// <summary>
        /// A flag to indicate if this page should animate out on load.
        /// Useful for when we are moving the page to another frame.
        /// </summary>
        public bool ShouldAnimateOut { get; set; }

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        public object ViewModelObject
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
                DataContext = mViewModel;
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
            if (PageLoadAnimation != PageAnimation.None)
            {
                Visibility = Visibility.Collapsed;
            }

            // Listen out for the page loading.
            Loaded += BasePage_LoadedAsync;
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
            // If we are setup to animate out on load.
            if (ShouldAnimateOut)
            {
                // Animate out the page.
                await AnimateOutAsync();
            }
            // Otherwise...
            else
            {
                // animate the page in.
                await AnimateInAsync();
            }
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

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToBottom:

                    // Start the animation.
                    await this.SlideAndFadeOutToBottom(SlideDistance, SlideSeconds);

                    break;
            }
        }

        #endregion
    }

    /// <summary>
    /// A base page for all pages to gain base functionality.
    /// </summary>
    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        #region Public Properties

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        public VM ViewModel
        {
            get => (VM)ViewModelObject;
            set => ViewModelObject = value;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use, if any.</param>
        public BasePage() : base()
        {
            // Create a default view model.
            ViewModel = IoC.Get<VM>();
        }

        /// <summary>
        /// Constructor with specific view model.
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use, if any.</param>
        public BasePage(VM specificViewModel = null) : base()
        {
            // Set specific view model.
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
                // Create a default view model.
                ViewModel = IoC.Get<VM>();
        }

        #endregion
    }
}
