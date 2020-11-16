using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
        ///     - 2 = Not set active connection - No Check in this method !!!!! - This is subclass of this manager <see cref="PreferencesConnectionDataViewModel"/>
        /// </returns>
        public override int SendTextMessage(string message)
        {
            var response = SendTextMessageAsync(message);
            response.Wait();
            return response.Result;
        }

        /// <summary>
        /// Async version of <see cref="SendTextMessage(string)"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override async Task<int> SendTextMessageAsync(string message)
        {
            int status = 0;
            var address = Webhook;

            // Get client.
            var client = IoC.Web.Http.GetClientForHost(new Uri(address), mSendingTimeout);

            // Create discord value collection.
            var discordValues = new List<KeyValuePair<string, string>>
            {
                // Username (BOT).
                new KeyValuePair<string, string>("username", IoC.Application.ProductName.Replace(" ", "")),
                // Avatar.
                new KeyValuePair<string, string>("avatar_url", IoC.Application.LogoURL),
                // Message.
                new KeyValuePair<string, string>("content", $"{UserID} {message}")
            };

            // Solve.
            try
            {
                // Encode values.
                FormUrlEncodedContent content = new FormUrlEncodedContent(discordValues);

                // Send data.
                await client.PostAsync(address, content);
                IoC.Logger.Log($"Message sent!", LogLevel.Debug);
            }
            catch (HttpRequestException e) // Internet connection issues.
            {
                IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Verbose);
                status = 1;
            }
            catch (TaskCanceledException e) // Timeout.
            {
                IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message} (expected exception)", LogLevel.Debug);
                status = 1;
            }
            catch (Exception e) // Unexpected.
            {
                IoC.Logger.Log($"{e.GetType().ToString()}: {e.Message}", LogLevel.Fatal);
                status = 1;
            }

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
