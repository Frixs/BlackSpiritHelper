using BlackSpiritHelper.Core.Data.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Internet Connection wrapper.
    /// Serves to <see cref="WatchdogConnectionWatcherDataViewModel"/>.
    /// </summary>
    public class WatchdogInternetConnectionDataViewModel : AWatchdogConnectionBase
    {
        #region Static Limitation Properties

        /// <summary>
        /// Min possible set multi-check.
        /// </summary>
        public static short AllowedMinMultiCheck { get; private set; } = 0;

        /// <summary>
        /// Max possible set multi-check.
        /// </summary>
        public static short AllowedMaxMultiCheck { get; private set; } = 4;

        /// <summary>
        /// Min possible set timeout.
        /// Unit: Milliseconds
        /// </summary>
        public static int AllowedMinTimeout { get; private set; } = 1;

        /// <summary>
        /// Max possible set timeout.
        /// Unit: Milliseconds
        /// </summary>
        public static int AllowedMaxTimeout { get; private set; } = 30000;

        #endregion

        #region Private Fields

        /// <summary>
        /// TODO
        /// </summary>
        private HttpClient Client = null; //; Constructor - Init method TODO

        /// <summary>
        /// Says if the failure action has been already proceeded.
        /// We dont want to fire the same event on each failure. Only at the time when the failure occurs for the first time.
        /// </summary>
        private bool mIsFailureActionFired = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Address used for check ping connection test.
        /// </summary>
        [XmlIgnore]
        public string PingCheckAddress { get; set; } = "8.8.8.8";

        /// <summary>
        /// Address used for web client opening test.
        /// </summary>
        [XmlIgnore]
        public string ClientCheckAddress { get; set; } = "http://google.com/generate_204";

        /// <summary>
        /// Timeout for each Ping test.
        /// Unit: Milliseconds
        /// </summary>
        public int PingTimeout { get; set; } = 1250;

        /// <summary>
        /// Timeout for each Client test.
        /// Unit: Milliseconds
        /// </summary>
        public int ClientTimeout { get; set; } = 5000;

        /// <summary>
        /// Says, how many additional checks have to be performed if the major check fails.
        /// Value set to 0 means we do not have any additional checks.
        /// </summary>
        public short MultiCheck { get; set; } = 2;

        /// <summary>
        /// Delay for the 2nd (double-check) test.
        /// Unit: Milliseconds
        /// </summary>
        [XmlIgnore]
        public int MultiCheckDelay { get; set; } = 4000;

        /// <summary>
        /// Says if the check is selected for checking loop.
        /// </summary>
        public override bool IsSelected { get; set; } = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogInternetConnectionDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The main method to check conditions.
        /// </summary>
        /// <returns></returns>
        public override bool Check()
        {
            bool isOk = false;

            for (int i = 0; i < (MultiCheck + 1); i++) // +1 due to major check
            {
                isOk = true;
                
                // Do this stuff before additional check round.
                if (i > 0)
                {
                    Thread.Sleep(MultiCheckDelay);
                }

                // Check PING first.
                if (!CheckPingConnection())
                    // Check client test if PING fails.
                    if (!CheckClientConnection())
                        isOk = false;

                // On failure hook.
                if (!isOk && !mIsFailureActionFired && i > MultiCheck - 1)
                {
                    mIsFailureActionFired = true; // Let know the procedure to do not fire this the same event again until the failure disappear.
                    IoC.DataContent.WatchdogData.Log("Internet connection lost!");
                }
                // First occurance of recovery from failure.
                else if (isOk && mIsFailureActionFired)
                {
                    mIsFailureActionFired = false; // Reset value back to false to be able to record failure event again when occurs.
                    IoC.DataContent.WatchdogData.Log("Internet connection established!");
                }

                // Once we are good. No need to additional check.
                if (isOk)
                    break;
            }

            return isOk;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Check onnection to the internet through simple ping method.
        /// </summary>
        /// <returns></returns>
        private bool CheckPingConnection()
        {
            bool pingStatus = false;

            using (Ping p = new Ping())
            {
                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // 32 chars
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                try
                {
                    PingReply reply = p.Send(PingCheckAddress, PingTimeout, buffer);
                    pingStatus = reply.Status == IPStatus.Success;

                    // Log fail.
                    if (!pingStatus)
                    {
                        IoC.Logger.Log($"Ping failed with status: {reply.Status.ToString()}", LogLevel.Debug);
                        if (reply.Status.Equals(IPStatus.TimedOut))
                            IoC.DataContent.WatchdogData.Log("Ping timed out!");
                    }
                }
                catch (PingException e) // Expected - no internet connection.
                {
                    pingStatus = false;
                    IoC.Logger.Log($"{e.GetType()}: {e.Message} (expected exception)", LogLevel.Debug);
                }
                catch (Exception e) // Unexpected.
                {
                    pingStatus = false;
                    IoC.Logger.Log($"{e.GetType()}: {e.Message}", LogLevel.Fatal);
                }
            }

            return pingStatus;
        }

        /// <summary>
        /// Check connection to the internet through <see cref="HttpWebRequest"/>.
        /// TODO: Optimalize - Change to HttpClient / singleton client for InternetConnection only.
        /// </summary>
        /// <returns></returns>
        private bool CheckClientConnection()
        {
            bool clientStatus = false;

            var request = (HttpWebRequest)WebRequest.Create(ClientCheckAddress);
            request.Timeout = ClientTimeout;
            HttpWebResponse response = null;

            try
            {
                // Try to get response.
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e) // No internet connection or timeout.
            {
                clientStatus = false;
                IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Debug);
            }
            catch (Exception e) // Unexpected.
            {
                clientStatus = false;
                IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message}", LogLevel.Fatal);
            }
            finally
            {
                // Free.
                if (response != null)
                    response.Close();
            }

            // Return status.
            return clientStatus;
        }

        #endregion
    }
}
