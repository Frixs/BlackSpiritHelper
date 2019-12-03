namespace BlackSpiritHelper.Core
{
    public class PreferencesConnDiscordDataViewModel : APreferencesConnBaseDataViewModel
    {
        #region Public Properties

        /// <summary>
        /// Identifier string representing particular connection.
        /// </summary>
        public override string Identifier { get; protected set; } = "DISCORD";

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
