using System;
using System.Collections.Specialized;
using System.Net;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class PreferencesConnDiscordDataViewModel : APreferencesConnBaseDataViewModel
    {
        #region Static Limitation Properties

        /// <summary>
        /// Max length of <see cref="Webhook"/>.
        /// </summary>
        public static byte AllowedWebhookMaxLength { get; private set; } = 150;

        /// <summary>
        /// Max length of <see cref="UserID"/>.
        /// </summary>
        public static byte AllowedUsernameMaxLength { get; private set; } = 25;

        #endregion

        #region Public Properties

        /// <summary>
        /// Webhook to Discord server.
        /// </summary>
        public string Webhook { get; set; } = "";

        /// <summary>
        /// Discord user ID.
        /// </summary>
        public string UserID { get; set; } = "";

        /// <summary>
        /// Identifier representing particular connection.
        /// </summary>
        [XmlIgnore]
        public override PreferencesConnectionType Identifier { get; protected set; } = PreferencesConnectionType.Discord;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PreferencesConnDiscordDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send message to the user's connection.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        /// Status code:
        ///     - 0 = OK
        ///     - 1 = Unexpected error occurred - no internet connection
        ///     - 2 = Not set active connection
        /// </returns>
        public override int SendTextMessage(string message)
        {
            if (!IoC.DataContent.PreferencesData.Connection.IsActive)
                return 2;

            int status = 0;

            // Create WC.
            WebClient wc = new WebClient();
            // Create discord value collection.
            NameValueCollection discordValues = new NameValueCollection();

            // Username (BOT).
            discordValues.Add("username", IoC.Application.ProductName.Replace(" ", ""));
            // Avatar.
            discordValues.Add("avatar_url", IoC.Application.LogoURL);
            // Message.
            discordValues.Add("content", $"<@{UserID}> {message}");

            // Solve.
            try
            {
                // Send message.
                wc.UploadValues(Webhook, discordValues);
            }
            catch (WebException) // No connection.
            {
                IoC.Logger.Log("WebException = no connection", LogLevel.Debug);
                status = 1;
            }
            catch (Exception e) // Unexpected.
            {
                IoC.Logger.Log(e.Message, LogLevel.Fatal);
                status = 1;
            }

            // Dispose WC.
            wc.Dispose();

            // Return status.
            return status;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validate inputs of the connection method.
        /// </summary>
        /// <returns></returns>
        public override bool ValidateInputs()
        {
            // Webhook.
            if (!new PreferencesConnDiscordWebhookRule().Validate(Webhook, null).IsValid)
                return false;

            // Username.
            if (!new PreferencesConnDiscordUserIdRule().Validate(UserID, null).IsValid)
                return false;

            return true;
        }

        #endregion
    }
}
