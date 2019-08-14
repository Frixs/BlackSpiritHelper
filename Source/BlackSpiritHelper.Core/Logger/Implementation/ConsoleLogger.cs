using System;

namespace BlackSpiritHelper.Core
{
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Logs the given message to the system Console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The leve of the message.</param>
        public void Log(string message, LogLevel level)
        {
            // Save old color.
            var consoleOldColor = Console.ForegroundColor;

            // Default log color value.
            var consoleColor = ConsoleColor.White;

            // Color console based on level.
            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case LogLevel.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            // Set the desired console color.
            Console.ForegroundColor = consoleColor;

            // Write message to console.
            Console.WriteLine(message);
        }
    }
}
