﻿using BlackSpiritHelper.Core;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Locates view models from the IoC for use in binding in Xaml files.
    /// </summary>
    public class ViewModelLocator
    {
        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator.
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model.
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => IoC.Application;

        /// <summary>
        /// The application data content.
        /// </summary>
        public static ApplicationDataContent ApplicationDataContent => IoC.DataContent;

        /// <summary>
        /// The application UI data.
        /// </summary>
        public static IUIManager ApplicationUIManager => IoC.UI;

        #endregion
    }
}
