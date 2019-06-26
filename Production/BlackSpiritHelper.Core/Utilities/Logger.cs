using System;
using System.IO;
using System.Text;

namespace BlackSpiritHelper.Core
{
    public class Logger
    {
        #region Private Members

        private string mLoggerClassName;
        private const string mLogFileLocation = "Log/";
        private const string mLogFileName = "Log.txt";
        private const string mLogFilePath = mLogFileLocation + mLogFileName;

        #endregion

        #region Constructor

        public Logger(string loggerClassName)
        {
            this.mLoggerClassName = loggerClassName;
        }

        #endregion

        /// <summary>
        /// Designates finer-grained informational events than the DEBUG.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Trace(string message)
        {
            string prefix = string.Format("[{0}] - TRACE: ", mLoggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates fine-grained informational events that are most useful to debug an application.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Debug(string message)
        {
            string prefix = string.Format("[{0}] - DEBUG: ", mLoggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Info(string message)
        {
            string prefix = string.Format("[{0}] - INFO: ", mLoggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates potentially harmful situations.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Warn(string message)
        {
            string prefix = string.Format("[{0}] - WARN: ", mLoggerClassName);
            WriteToLogFile(prefix + message);
        }

        /// <summary>
        /// Designates error events that might still allow the application to continue running.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Error(string message)
        {
            string prefix = string.Format("[{0}] - ERROR: ", mLoggerClassName);
            WriteToLogFile(prefix + message);
        }

        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Fatal(string message)
        {
            string prefix = string.Format("[{0}] - FATAL: ", mLoggerClassName);
            WriteToLogFile(prefix + message);
        }

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
    }
}
