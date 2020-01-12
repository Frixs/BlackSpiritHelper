namespace BlackSpiritHelper.Core
{
    public interface IAppAssembly
    {
        /// <summary>
        /// Restart the application.
        /// </summary>
        /// <param name="args">Arguments in string form to pass for a new start of the app</param>
        void Restart(string args = "");
    }
}
