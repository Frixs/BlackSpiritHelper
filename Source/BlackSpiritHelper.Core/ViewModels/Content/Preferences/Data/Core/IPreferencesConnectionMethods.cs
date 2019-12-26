using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    public interface IPreferencesConnectionMethods
    {
        /// <summary>
        /// Send message to the user's connection.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>
        /// Status code:
        ///     - 0 = OK
        ///     - 1 = Unexpected error occurred - no internet connection
        ///     - 2 = Not set active connection
        /// </returns>
        int SendTextMessage(string message);

        /// <summary>
        /// Async version of <see cref="SendTextMessage(string)"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<int> SendTextMessageAsync(string message);
    }
}
