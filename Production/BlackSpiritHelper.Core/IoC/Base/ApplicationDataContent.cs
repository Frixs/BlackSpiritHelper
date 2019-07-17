namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Subclass for <see cref="IoC"/>. There you define all application data structures.
    /// F.e. Timer structure is a bit complex with its groups <see cref="TimerGroupListViewModel"/>. You have to bind the same view model to multiple views.
    /// FOr this reason, you define these data structures here to easily access it as a singleton.
    /// </summary>
    public class ApplicationDataContent
    {
        #region Public Properties

        /// <summary>
        /// Data structure for the preferences.
        /// </summary>
        public PreferencesDesignModel PreferencesDesignModel { get; private set; } = IoC.SettingsStorage.PreferencesDesignModel == null
            ? PreferencesDesignModel.Instance
            : IoC.SettingsStorage.PreferencesDesignModel;

        /// <summary>
        /// Data structure for timers with its groups.
        /// </summary>
        public TimerGroupListDesignModel TimerGroupListDesignModel { get; private set; } = IoC.SettingsStorage.TimerGroupListDesignModel == null
            ? TimerGroupListDesignModel.Instance
            : IoC.SettingsStorage.TimerGroupListDesignModel;

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

            // Clear saved data commit.
            IoC.SettingsStorage.Save();

            // Save new data.
            SaveApplicationData();
            SaveNewPreferences();
            SaveNewTimerData();

            // Save commit.
            IoC.SettingsStorage.Save();

            // Log it.
            IoC.Logger.Log("User data saved!", LogLevel.Info);

            //
            //XmlSerializer mySerializer = new XmlSerializer(typeof(TimerGroupListDesignModel));
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
            foreach (TimerGroupViewModel g in TimerGroupListDesignModel.GroupList)
                foreach (TimerItemViewModel t in g.TimerList)
                    if (t.IsRunning)
                        t.TimerFreeze();

            // Save new data.
            IoC.SettingsStorage.TimerGroupListDesignModel = TimerGroupListDesignModel;
        }

        /// <summary>
        /// Clear saved Timer Page data.
        /// </summary>
        private void ClearSavedTimerData()
        {
            // Clear previous save.
            IoC.SettingsStorage.TimerGroupListDesignModel = null;
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

        #endregion
    }
}
