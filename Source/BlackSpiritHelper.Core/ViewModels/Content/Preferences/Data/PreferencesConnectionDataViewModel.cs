using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    public class PreferencesConnectionDataViewModel : BaseViewModel
    {
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
        public PreferencesConnectionType ActiveMethodIdentifier { get; set; } = PreferencesConnectionType.None;

        /// <summary>
        /// Active (activated) method that user uses.
        /// </summary>
        [XmlIgnore]
        public APreferencesConnBaseDataViewModel ActiveMethod { get; set; } = null;

        /// <summary>
        /// List of all available connections for user.
        /// Use <seealso cref="Init"/> for initialization.
        /// </summary>
        [XmlIgnore]
        public List<APreferencesConnBaseDataViewModel> MethodList { get; set; } //; Init - Constructor

        /// <summary>
        /// Says if <see cref="Init"/> has been fired or not.
        /// </summary>
        [XmlIgnore]
        public bool IsInitialized { get; private set; } = false;

        #endregion

        #region Public Properties (Methods)

        /// <summary>
        /// Connection method: Discord
        /// Use <seealso cref="Init"/> for initialization.
        /// </summary>
        public PreferencesConnDiscordDataViewModel MethodDiscord { get; set; } = new PreferencesConnDiscordDataViewModel();

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

        /// <summary>
        /// This method should be called only once at the beggining for initializing this section.
        /// Use <see cref="PreferencesDataViewModel.SetupMethod"/>.
        /// </summary>
        public void Init()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            // Build list of all methods.
            MethodList = new List<APreferencesConnBaseDataViewModel>()
            {
                MethodDiscord,
            };

            // Activate user preferred method.
            ActivateMethod(ActiveMethodIdentifier);
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
            ActiveMethod = MethodList.FirstOrDefault(o => o.Identifier.Equals(identifier));
            ActiveMethodIdentifier = ActiveMethod == null ? PreferencesConnectionType.None : ActiveMethod.Identifier;
            OnPropertyChanged(nameof(IsActive));
        }

        /// <summary>
        /// Deactivate connection method which user uses.
        /// </summary>
        /// <param name="identifier"></param>
        public void DeactivateMethod()
        {
            ActiveMethod = null;
            ActiveMethodIdentifier = PreferencesConnectionType.None;
            OnPropertyChanged(nameof(IsActive));
        }

        #endregion
    }
}
