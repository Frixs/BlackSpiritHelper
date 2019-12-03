using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public abstract class APreferencesConnBaseDataViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Identifier string representing particular connection.
        /// </summary>
        [XmlIgnore]
        public abstract string Identifier { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public APreferencesConnBaseDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send message to the user's connection.
        /// </summary>
        /// <param name="message"></param>
        public abstract bool SendTextMessage(string message);

        /// <summary>
        /// Activate method as the one ehich user uses.
        /// </summary>
        /// <returns></returns>
        public void ActivateMethod()
        {
            // TODO: validation stuff

            IoC.DataContent.PreferencesData.Connection.ActivateMethod(Identifier);
        }

        #endregion
    }
}
