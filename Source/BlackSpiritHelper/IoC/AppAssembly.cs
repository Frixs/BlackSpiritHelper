using BlackSpiritHelper.Core;
using System;
using System.Diagnostics;
using System.Reflection;
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
        /// <param name="args">Arguments in string form to pass for a new start of the app</param>
        public void Restart(string args = "")
        {
            IoC.Logger.Log($"Restarting the application...", LogLevel.Info);

            if (string.IsNullOrEmpty(args))
                NativeRestart();
            else
                ArgumentRestart(args);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Native restart option made for WPF.
        /// </summary>
        private void NativeRestart()
        {
            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // from System.Windows.Forms.dll
                System.Windows.Forms.Application.Restart();
                Application.Current.Shutdown();
            }));
        }

        /// <summary>
        /// Restart the application by init a new process and kill the current one.
        /// Posibility to pass parameters.
        /// </summary>
        /// <param name="args"></param>
        private void ArgumentRestart(string args)
        {
            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // It is not possible to launch a ClickOnce app as administrator directly, so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                // Add arguments.
                processInfo.Arguments = args;

                try
                {
                    // Start the new process.
                    Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred during restarting the application.{Environment.NewLine}Please, contact the developers to fix the issue.");
                    IoC.Logger.Log($"An unexpected error occurred during restarting the application - {ex.GetType().ToString()}: {ex.Message}", LogLevel.Fatal);
                }

                // Shutdown the current process.
                Application.Current.Shutdown();
            }));
        }

        #endregion
    }
}
