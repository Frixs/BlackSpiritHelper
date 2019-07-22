namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Subclass for <see cref="IoC"/>. There you define all application data structures.
    /// F.e. Timer structure is a bit complex with its groups <see cref="TimerViewModel"/>. You have to bind the same view model to multiple views.
    /// FOr this reason, you define these data structures here to easily access it as a singleton.
    /// </summary>
    public class ApplicationDataContent
    {
        #region Public Properties

        /// <summary>
        /// Data structure for the preferences.
        /// </summary>
        public PreferencesDesignModel PreferencesDesignModel { get; private set; }

        /// <summary>
        /// Data structure for the overlay.
        /// </summary>
        public OverlayDesignModel OverlayDesignModel { get; private set; }

        /// <summary>
        /// Data structure for timers with its groups.
        /// </summary>
        public TimerDesignModel TimerDesignModel { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApplicationDataContent()
        {
        }

        /// <summary>
        /// Option to load user data on application load in order you want.
        /// </summary>
        public void Setup()
        {
            // Preferences.
            PreferencesDesignModel = IoC.SettingsStorage.PreferencesDesignModel ?? PreferencesDesignModel.Instance;

            // Timer.
            TimerDesignModel = IoC.SettingsStorage.TimerDesignModel ?? TimerDesignModel.Instance;

            // Overlay.
            OverlayDesignModel = IoC.SettingsStorage.OverlayDesignModel ?? OverlayDesignModel.Instance;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Save all user data (on application exit).
        /// </summary>
        public void SaveUserData()
        {
            // Clear previously saved data, first.
            ClearSavedPreferences();
            ClearSavedTimerData();
            ClearSavedOverlaySettings();

            // Clear saved data commit.
            IoC.SettingsStorage.Save();

            // Save new data.
            SaveApplicationData();
            SaveNewPreferences();
            SaveNewTimerData();
            SaveNewOverlaySettings();

            // Save commit.
            IoC.SettingsStorage.Save();

            // Log it.
            IoC.Logger.Log("User data saved!", LogLevel.Info);

            //
            //XmlSerializer mySerializer = new XmlSerializer(typeof(TimerDesignModel));
            //StreamWriter myWriter = new StreamWriter("prefs.xml");
            //mySerializer.Serialize(myWriter, xb);
            //myWriter.Close();
            //
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Save application data.
        /// </summary>
        private void SaveApplicationData()
        {
            // Save last opened page.
            IoC.SettingsStorage.LastOpenedPage = (byte)IoC.Application.CurrentPage;
        }

        /// <summary>
        /// Save new Timer Page data.
        /// </summary>
        private void SaveNewTimerData()
        {
            // Freeze all running timers, first.
            foreach (TimerGroupDataViewModel g in TimerDesignModel.GroupList)
                foreach (TimerItemDataViewModel t in g.TimerList)
                    if (t.IsRunning)
                        t.TimerFreeze();

            // Save new data.
            IoC.SettingsStorage.TimerDesignModel = TimerDesignModel;
        }

        /// <summary>
        /// Clear saved Timer Page data.
        /// </summary>
        private void ClearSavedTimerData()
        {
            // Clear previous save.
            IoC.SettingsStorage.TimerDesignModel = null;
        }

        /// <summary>
        /// Save new user preferences.
        /// </summary>
        private void SaveNewPreferences()
        {
            // Save new data.
            IoC.SettingsStorage.PreferencesDesignModel = PreferencesDesignModel;
        }

        /// <summary>
        /// Clear saved user preferences.
        /// </summary>
        private void ClearSavedPreferences()
        {
            // Clear previous save.
            IoC.SettingsStorage.PreferencesDesignModel = null;
        }

        /// <summary>
        /// Save new overlay settings.
        /// </summary>
        private void SaveNewOverlaySettings()
        {
            // Save new data.
            IoC.SettingsStorage.OverlayDesignModel = OverlayDesignModel;
        }

        /// <summary>
        /// Clear saved overlay settings.
        /// </summary>
        private void ClearSavedOverlaySettings()
        {
            // Clear previous save.
            IoC.SettingsStorage.OverlayDesignModel = null;
        }

        #endregion
    }
}
