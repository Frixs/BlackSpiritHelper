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
        /// Max length of <see cref="Username"/>.
        /// </summary>
        public static byte AllowedUsernameMaxLength { get; private set; } = 25;

        #endregion

        #region Public Properties

        /// <summary>
        /// Webhook to Discord server.
        /// </summary>
        public string Webhook { get; set; } = "";

        /// <summary>
        /// Discord username.
        /// </summary>
        public string Username { get; set; } = "";

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
        /// <param name="message"></param>
        public override bool SendTextMessage(string message)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Validate inputs of connection method.
        /// </summary>
        /// <returns></returns>
        public override bool ValidateInputs()
        {
            // Webhook.
            if (!new PreferencesConnDiscordWebhookRule().Validate(Webhook, null).IsValid)
                return false;

            // Username.
            if (!new PreferencesConnDiscordUsernameRule().Validate(Username, null).IsValid)
                return false;

            return true;
        }

        #endregion
    }
}
