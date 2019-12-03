using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class PreferencesConnectionDataViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Active (activated) method's identifier.
        /// <see cref="ActiveMethodIdentifier"/>.
        /// </summary>
        private string mActiveMethodIdentifier = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Active (activated) method's identifier - <see cref="APreferencesConnBaseDataViewModel.Identifier"/>.
        /// Servers loading/saving user data, primarily.
        /// Is synced with value changing of <see cref="ActiveMethod"/>.
        /// </summary>
        public string ActiveMethodIdentifier
        {
            get => mActiveMethodIdentifier;
            set
            {
                mActiveMethodIdentifier = value;
                ActivateMethod(value);
            }
        }

        /// <summary>
        /// Active (activated) method that user uses.
        /// </summary>
        [XmlIgnore]
        public APreferencesConnBaseDataViewModel ActiveMethod { get; set; } = null;

        /// <summary>
        /// List of all available connections for user.
        /// </summary>
        public List<APreferencesConnBaseDataViewModel> Methods { get; set; } = new List<APreferencesConnBaseDataViewModel>()
        {
            // Discord
            new PreferencesConnDiscordDataViewModel(),
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PreferencesConnectionDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activate new method as the one which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void ActivateMethod(string identifier)
        {
            ActiveMethod = Methods.FirstOrDefault(o => o.Identifier.Equals(identifier));
            mActiveMethodIdentifier = ActiveMethod == null ? string.Empty : ActiveMethod.Identifier;
        }

        #endregion
    }
}
