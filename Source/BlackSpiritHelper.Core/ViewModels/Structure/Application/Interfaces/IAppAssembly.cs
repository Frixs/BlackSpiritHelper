namespace BlackSpiritHelper.Core
{
    public interface IAppAssembly
    {
        /// <summary>
        /// Restart the application.
        /// </summary>
        /// <param name="args">Arguments in string form to pass for a new start of the app</param>
        /// <param name="saveSettings">Indicates if the settings should be saved on app restart or not</param>
        void Restart(string args = "", bool saveSettings = false);

        /// <summary>
        /// Update active user counter. Anonymous approach.
        /// </summary>
        void UpdateActiveUserCounter();
    }
}
