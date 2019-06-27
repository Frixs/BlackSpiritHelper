using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BlackSpiritHelper.Core
{
    public class Logger
    {
        #region Singleton

        /// <summary>
        /// Singleton instance for the logger.
        /// </summary>
        public static Logger Instance { get; private set; } = new Logger();

        #endregion

        #region Private Members

        private const string mLogFileLocation = "Log/";
        private const string mLogFileName = "Log.txt";
        private const string mLogFilePath = mLogFileLocation + mLogFileName;

        #endregion

        #region Constructor

        public Logger()
        {
        }

        #endregion

        /// <summary>
        /// Designates finer-grained informational events than the DEBUG.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Trace(string message)
        {
            Console.WriteLine(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Designates fine-grained informational events that are most useful to debug an application.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Debug(string message)
        {
            Console.WriteLine(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Info(string message)
        {
            Console.WriteLine(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Designates potentially harmful situations.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Warn(string message)
        {
            WriteToLogFile(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Designates error events that might still allow the application to continue running.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Error(string message)
        {
            WriteToLogFile(GetMessagePrefix() + message);
        }

        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Fatal(string message)
        {
            WriteToLogFile(GetMessagePrefix() + message);
        }

        #region Helpers

        private void WriteToLogFile(string message)
        {
            if (!File.Exists(mLogFilePath))
            {
                Directory.CreateDirectory(mLogFileLocation);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(message);
            sb.Append(string.Format(" [{0:HH:mm:ss tt}]", DateTime.Now));
            sb.Append("\r\n");

            File.AppendAllText(mLogFilePath, sb.ToString());
            sb.Clear();

            Console.WriteLine(message);
        }

        /// <summary>
        /// Get prefix for the log message.
        /// </summary>
        /// <returns></returns>
        private string GetMessagePrefix()
        {
            var st = new StackTrace();

            return string.Format(
                "[{0}] - " + st.GetFrame(1).GetMethod().Name + ": ",
                st.GetFrame(2).GetMethod().ReflectedType.FullName + ":" + st.GetFrame(2).GetMethod().Name
                );
        }

        #endregion
    }
}
