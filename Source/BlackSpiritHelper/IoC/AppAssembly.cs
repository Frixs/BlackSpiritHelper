using BlackSpiritHelper.Core;
using System;
using System.Diagnostics;
using System.Management;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        /// <inheritdoc/>
        public void Restart(string args = "", bool saveSettings = false)
        {
            IoC.Logger.Log($"Restarting the application...", LogLevel.Info);

            if (string.IsNullOrEmpty(args))
                NativeRestart(saveSettings);
            else
                ArgumentRestart(args, saveSettings);
        }

        /// <summary>
        /// Update active user counter. Anonymous approach.
        /// </summary>
        public void UpdateActiveUserCounter()
        {
            IoC.Task.Run(async () =>
            {
                // Compile URL.
                string url = SettingsConfiguration.UpdateActiveUserCounterURL + $@"?hash={CalculateUHWID()}";

                // Get client.
                var client = IoC.Web.Http.GetClientForHost(new Uri(url));

                // Get data.
                try
                {
                    // Read data.
                    await client.GetAsync(url);
                    IoC.Logger.Log($"Request to update active user counter has been successfully sent!", LogLevel.Debug);
                }
                catch (HttpRequestException e) // Internet connection issues.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Verbose);
                }
                catch (TaskCanceledException e) // Timeout.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Debug);
                }
                catch (Exception e) // Unexpected.
                {
                    IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message}", LogLevel.Fatal);
                }
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Native restart option made for WPF.
        /// </summary>
        /// <param name="saveSettings">Indicates if the settings should be saved on app restart or not</param>
        private void NativeRestart(bool saveSettings)
        {
            IoC.Dispatcher.UI.BeginInvokeOrDie((Action)(() =>
            {
                // from System.Windows.Forms.dll
                System.Windows.Forms.Application.Restart();
                IoC.Application.Exit(saveSettings);
            }));
        }

        /// <summary>
        /// Restart the application by init a new process and kill the current one.
        /// Posibility to pass parameters.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="saveSettings">Indicates if the settings should be saved on app restart or not</param>
        private void ArgumentRestart(string args, bool saveSettings)
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
                IoC.Application.Exit(saveSettings);
            }));
        }

        /// <summary>
        /// Generate Unique HardWare IDentifier.
        /// </summary>
        /// <returns></returns>
        private string CalculateUHWID()
        {
            string ret;
            MD5 md5 = null;

            try
            {
                // Get CPU ID.
                string cpuId = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (cpuId == string.Empty)
                    {
                        //Get only the first CPU's ID
                        cpuId = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }

                // Step 1, calculate MD5 hash from input.
                md5 = MD5.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(
                    System.Security.Principal.WindowsIdentity.GetCurrent().Name + cpuId
                    );
                byte[] hash = md5.ComputeHash(inputBytes);

                // Step 2, convert byte array to hex string.
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("X2"));
                ret = sb.ToString();
            }
            catch (Exception ex)
            {
                IoC.Logger.Log($"{ex.GetType().ToString()}: {ex.Message} (expected exception)", LogLevel.Error);
                ret = "null";
            }
            finally
            {
                if (md5 != null)
                    md5.Dispose();
            }

            return ret;
        }

        #endregion
    }
}
