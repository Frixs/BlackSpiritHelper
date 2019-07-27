using Ninject;
using System;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The IoC container for our application.
    /// </summary>
    public static class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for our IoC container. // Inversion of Control.
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="BlackSpiritHelper.Core.Properties.Settings"/>.
        /// </summary>
        public static BlackSpiritHelper.Core.Properties.Settings SettingsStorage => Get<BlackSpiritHelper.Core.Properties.Settings>();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>.
        /// </summary>
        public static ApplicationViewModel Application => Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="IUIManager"/>.
        /// </summary>
        public static IUIManager UI => Get<IUIManager>();

        /// <summary>
        /// A shortcut to access the <see cref="ILogFactory"/>.
        /// </summary>
        public static ILogFactory Logger => Get<ILogFactory>();

        /// <summary>
        /// A shortcut to access the <see cref="IFileManager"/>.
        /// </summary>
        public static IFileManager File => Get<IFileManager>();

        /// <summary>
        /// A shortcut to access the <see cref="ITaskManager"/>.
        /// </summary>
        public static ITaskManager Task => Get<ITaskManager>();

        /// <summary>
        /// A shortcut to access the <see cref="IAudioFactory"/>.
        /// </summary>
        public static IAudioFactory Audio => Get<IAudioFactory>();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationDataContent"/>.
        /// </summary>
        public static ApplicationDataContent DataContent => Get<ApplicationDataContent>();

        #endregion

        #region Construction

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use.
        /// NOTE: Must be called as soon as your application starts up to ensure all services can be found.
        /// </summary>
        public static void Setup()
        {
            // Bind settings storage.
            Kernel.Bind<BlackSpiritHelper.Core.Properties.Settings>().ToConstant(BlackSpiritHelper.Core.Properties.Settings.Default);

            // Bind all required view models.
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models.
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model.
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            // Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel { CurrentPage = ApplicationPage.Home }); // Option to set initial parameters.

        }

        #endregion

        /// <summary>
        /// Get's a services from the IoC, of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
