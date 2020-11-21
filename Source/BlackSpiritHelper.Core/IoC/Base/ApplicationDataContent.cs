namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Subclass for <see cref="IoC"/>. There you define all application data structures.
    /// F.e. Timer structure is a bit complex with its groups <see cref="TimerDataViewModel"/>. You have to bind the same view model to multiple views.
    /// For this reason, you define these data structures here to easily access it as a singleton.
    /// </summary>
    public class ApplicationDataContent
    {
        #region Public Properties

        /// <summary>
        /// Data structure for the preferences.
        /// </summary>
        public PreferencesDataViewModel PreferencesData { get; private set; }

        /// <summary>
        /// Data structure for APM Calculator.
        /// </summary>
        public ApmCalculatorDataViewModel ApmCalculatorData { get; private set; }

        /// <summary>
        /// Data structure for schedule.
        /// </summary>
        public ScheduleDataViewModel ScheduleData { get; private set; }

        /// <summary>
        /// Data structure for timers with its groups.
        /// </summary>
        public TimerDataViewModel TimerData { get; private set; }

        /// <summary>
        /// Data structure for schedule.
        /// </summary>
        public WatchdogDataViewModel WatchdogData { get; private set; }

        /// <summary>
        /// Data structure for the overlay.
        /// </summary>
        public OverlayDataViewModel OverlayData { get; private set; }

        #endregion

        #region Constructor & Setup

        /// <summary>
        /// Default constructor.
        /// Call <see cref="Setup"/> method right after creating the instance.
        /// </summary>
        public ApplicationDataContent()
        {
        }

        /// <summary>
        /// Option to load user data on application load in order you want.
        /// </summary>
        /// <remarks>
        ///     Try to get data of each section... 
        ///     if the data cannot be loaded, create a new data instance with triggered <see cref="ADataContentBaseViewModel.SetDefaults"/> method.
        ///     The method should be called only during initialization here!
        /// </remarks>
        public void Setup()
        {
            // Preferences.
            PreferencesData = IoC.SettingsStorage.PreferencesData ?? PreferencesDataViewModel.NewDataInstance;
            PreferencesData.Init();
            PreferencesData.SetDefaults();

            // APM Calculator
            ApmCalculatorData = IoC.SettingsStorage.ApmCalculatorData ?? ApmCalculatorDataViewModel.NewDataInstance;
            ApmCalculatorData.Init();
            ApmCalculatorData.SetDefaults();

            // Timer
            TimerData = IoC.SettingsStorage.TimerData ?? TimerDataViewModel.NewDataInstance;
            TimerData.Init();
            TimerData.SetDefaults();

            // Schedule
            ScheduleData = IoC.SettingsStorage.ScheduleData ?? ScheduleDataViewModel.NewDataInstance;
            ScheduleData.Init();
            ScheduleData.SetDefaults();

            // Watchdog
            WatchdogData = IoC.SettingsStorage.WatchdogData ?? WatchdogDataViewModel.NewDataInstance;
            WatchdogData.Init();
            WatchdogData.SetDefaults();

            // Overlay
            OverlayData = IoC.SettingsStorage.OverlayData ?? OverlayDataViewModel.NewDataInstance;
            OverlayData.Init();
            OverlayData.SetDefaults();
        }

        /// <summary>
        /// Unset application's data.
        /// "Prepare to die."
        /// </summary>
        public void Unset()
        {
            // Preferences.
            PreferencesData.Dispose();

            // APM Calculator.
            ApmCalculatorData.Dispose();

            // Timer.
            TimerData.Dispose();

            // Schedule.
            ScheduleData.Dispose();

            // Watchdog.
            WatchdogData.Dispose();

            // Overlay.
            OverlayData.Dispose();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Save all user data (on application exit).
        /// </summary>
        public void SaveUserData()
        {
            // Clear previously saved data, first.
            ClearSavedAppData();
            ClearSavedPreferencesData();
            ClearSavedApmCalculatorData();
            ClearSavedScheduleData();
            ClearSavedTimerData();
            ClearSavedWatchdogData();
            ClearSavedOverlayData();

            // Clear saved data commit.
            IoC.SettingsStorage.Save();

            // Save new data.
            SaveNewAppData();
            SaveNewPreferencesData();
            SaveNewApmCalculatorData();
            SaveNewScheduleData();
            SaveNewTimerData();
            SaveNewWatchdogData();
            SaveNewOverlayData();

            // Save commit.
            IoC.SettingsStorage.Save();

            // Log it.
            IoC.Logger.Log("User data saved!", LogLevel.Info);

            //
            //XmlSerializer mySerializer = new XmlSerializer(typeof(TimerDataViewModel));
            //StreamWriter myWriter = new StreamWriter("prefs.xml");
            //mySerializer.Serialize(myWriter, xb);
            //myWriter.Close();
            //
        }

        #endregion

        #region Private Methods - Save

        #region Application Data

        /// <summary>
        /// Save application data.
        /// </summary>
        private void SaveNewAppData()
        {
            IoC.Application.Cookies.Update();
            IoC.SettingsStorage.ApplicationCookies = IoC.Application.Cookies;
        }

        /// <summary>
        /// Clear saved user preferences.
        /// </summary>
        private void ClearSavedAppData()
        {
            // Clear previous save.
            IoC.SettingsStorage.ApplicationCookies = null;
        }

        #endregion

        #region Preferences

        /// <summary>
        /// Save new user preferences.
        /// </summary>
        private void SaveNewPreferencesData()
        {
            // Save new data.
            IoC.SettingsStorage.PreferencesData = PreferencesData;
        }

        /// <summary>
        /// Clear saved user preferences.
        /// </summary>
        private void ClearSavedPreferencesData()
        {
            // Clear previous save.
            IoC.SettingsStorage.PreferencesData = null;
        }

        #endregion

        #region APM Calculator

        /// <summary>
        /// Save new APM Calculator data.
        /// </summary>
        private void SaveNewApmCalculatorData()
        {
            // Save new data.
            IoC.SettingsStorage.ApmCalculatorData = ApmCalculatorData;
        }

        /// <summary>
        /// Clear saved APM Calculator data.
        /// </summary>
        private void ClearSavedApmCalculatorData()
        {
            // Clear previous save.
            IoC.SettingsStorage.ApmCalculatorData = null;
        }

        #endregion

        #region Timer

        /// <summary>
        /// Save new Timer Page data.
        /// </summary>
        private void SaveNewTimerData()
        {
            // Freeze all running timers, first.
            foreach (TimerGroupDataViewModel g in TimerData.GroupList)
                foreach (TimerItemDataViewModel t in g.TimerList)
                    if (t.IsRunning)
                        t.TimerFreeze();

            // Save new data.
            IoC.SettingsStorage.TimerData = TimerData;
        }

        /// <summary>
        /// Clear saved Timer Page data.
        /// </summary>
        private void ClearSavedTimerData()
        {
            // Clear previous save.
            IoC.SettingsStorage.TimerData = null;
        }

        #endregion

        #region Schedule

        /// <summary>
        /// Save new overlay settings.
        /// </summary>
        private void SaveNewScheduleData()
        {
            // Save new data.
            IoC.SettingsStorage.ScheduleData = ScheduleData;
        }

        /// <summary>
        /// Clear saved Schedule settings.
        /// </summary>
        private void ClearSavedScheduleData()
        {
            // Clear previous save.
            IoC.SettingsStorage.ScheduleData = null;
        }

        #endregion

        #region Watchdog

        /// <summary>
        /// Save new Watchdog settings.
        /// </summary>
        private void SaveNewWatchdogData()
        {
            // Save new data.
            IoC.SettingsStorage.WatchdogData = WatchdogData;
        }

        /// <summary>
        /// Clear saved Watchdog settings.
        /// </summary>
        private void ClearSavedWatchdogData()
        {
            // Clear previous save.
            IoC.SettingsStorage.WatchdogData = null;
        }

        #endregion

        #region Overlay

        /// <summary>
        /// Save new overlay settings.
        /// </summary>
        private void SaveNewOverlayData()
        {
            // Save new data.
            IoC.SettingsStorage.OverlayData = OverlayData;
        }

        /// <summary>
        /// Clear saved overlay settings.
        /// </summary>
        private void ClearSavedOverlayData()
        {
            // Clear previous save.
            IoC.SettingsStorage.OverlayData = null;
        }

        #endregion

        #endregion
    }
}
