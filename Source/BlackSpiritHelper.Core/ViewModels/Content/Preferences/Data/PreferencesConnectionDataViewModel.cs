using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private PreferencesConnectionType mActiveMethodIdentifier = PreferencesConnectionType.None;

        #endregion

        #region Public Properties

        /// <summary>
        /// Says if the application has set active connection or not.
        /// </summary>
        [XmlIgnore]
        public bool IsActive => ActiveMethod == null ? false : true;

        /// <summary>
        /// Active (activated) method's identifier - <see cref="APreferencesConnBaseDataViewModel.Identifier"/>.
        /// Servers loading/saving user data, primarily.
        /// Is synced with value changing of <see cref="ActiveMethod"/>.
        /// </summary>
        public PreferencesConnectionType ActiveMethodIdentifier
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

        #region Public Properties (Template Only)

        /// <summary>
        /// Discord Method.
        /// Method shortcut property for template binding only!!!
        /// </summary>
        [XmlIgnore]
        public APreferencesConnBaseDataViewModel MethodDiscord => Methods.FirstOrDefault(o => o.Identifier.Equals(PreferencesConnectionType.Discord));

        #endregion

        #region Commands

        /// <summary>
        /// Command to activate connection.
        /// </summary>
        [XmlIgnore]
        public ICommand DeactivateCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PreferencesConnectionDataViewModel()
        {
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            DeactivateCommand = new RelayCommand(async () => await DeactivateCommandMethodAsync());
        }

        /// <summary>
        /// Deactivate connection method async wrapper.
        /// </summary>
        /// <returns></returns>
        private async Task DeactivateCommandMethodAsync()
        {
            DeactivateMethod();
            await Task.Delay(1);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Activate new connection method as the one which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void ActivateMethod(PreferencesConnectionType identifier)
        {
            ActiveMethod = Methods.FirstOrDefault(o => o.Identifier.Equals(identifier));
            mActiveMethodIdentifier = ActiveMethod == null ? PreferencesConnectionType.None : ActiveMethod.Identifier;
            OnPropertyChanged(nameof(ActiveMethodIdentifier));
            OnPropertyChanged(nameof(IsActive));
        }

        /// <summary>
        /// Deactivate connection method which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void DeactivateMethod()
        {
            ActiveMethod = null;
            mActiveMethodIdentifier = PreferencesConnectionType.None;
            OnPropertyChanged(nameof(ActiveMethodIdentifier));
            OnPropertyChanged(nameof(IsActive));
        }

        #endregion
    }
}
