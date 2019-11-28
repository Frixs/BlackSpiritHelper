using BlackSpiritHelper.Core.Data.Interfaces;
using System;
using System.Net;
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
        #region Private Fields

        /// <summary>
        /// Says if the failure action has been already proceeded.
        /// We dont want to fire te same event on each failure. Only at the time when the failure occurs for the first time.
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
        public int PingTimeout { get; set; } = 1000;

        /// <summary>
        /// Timeout for each WebClient test.
        /// Unit: Milliseconds
        /// </summary>
        public int WebClientTimeout { get; set; } = 5000;

        /// <summary>
        /// Should the check perform double-check testing?
        /// </summary>
        public bool DoubleCheck { get; set; } = true;

        /// <summary>
        /// Delay for the 2nd (double-check) test.
        /// Unit: Milliseconds
        /// </summary>
        public int DoubleCheckDelay { get; set; } = 1000;

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

            for (int i = 0; i < (DoubleCheck ? 2 : 1); i++)
            {
                isOk = true;

                // Do stuff before double check round.
                if (i > 0)
                    Thread.Sleep(DoubleCheckDelay);

                // Check PING first.
                if (!CheckPingConnection())
                    // Check WebClient if PING fails.
                    if (!CheckWebClientConnection())
                        isOk = false;

                // On failure hook.
                if (!isOk && !mIsFailureActionFired && i > 0)
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
                }
                catch (Exception)
                {
                    pingStatus = false;
                }
            }

            return pingStatus;
        }

        /// <summary>
        /// Check connection to the internet through <see cref="WebClient"/>.
        /// </summary>
        /// <returns></returns>
        private bool CheckWebClientConnection()
        {
            // TODO timeout
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(ClientCheckAddress))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
