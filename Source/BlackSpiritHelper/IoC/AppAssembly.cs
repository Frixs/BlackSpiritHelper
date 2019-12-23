using BlackSpiritHelper.Core;
using System;
using System.Windows;

namespace BlackSpiritHelper
{
    public class AppAssembly : IAppAssembly
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AppAssembly()
        {
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Restart the application.
        /// </summary>
        public void Restart()
        {
            IoC.Logger.Log($"Restarting the application...", LogLevel.Info);

            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // from System.Windows.Forms.dll
                System.Windows.Forms.Application.Restart();
                Application.Current.Shutdown();
            }));
        }

        #endregion
    }
}
