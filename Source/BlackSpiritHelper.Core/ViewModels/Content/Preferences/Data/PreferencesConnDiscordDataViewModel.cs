using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class PreferencesConnDiscordDataViewModel : APreferencesConnBaseDataViewModel
    {
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

        #endregion
    }
}
