using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Utilities
{
    public class Logger
    {
        #region Private Members

        private string loggerClassName;

        #endregion

        #region Constructor

        public Logger(string loggerClassName)
        {
            this.loggerClassName = loggerClassName;
        }

        #endregion

        /// <summary>
        /// Designates finer-grained informational events than the DEBUG.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Trace(string message)
        {
            string prefix = string.Format("[{0}] - TRACE: ", loggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates fine-grained informational events that are most useful to debug an application.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Debug(string message)
        {
            string prefix = string.Format("[{0}] - DEBUG: ", loggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Info(string message)
        {
            string prefix = string.Format("[{0}] - INFO: ", loggerClassName);
            Console.WriteLine(prefix + message);
        }

        /// <summary>
        /// Designates potentially harmful situations.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Warn(string message)
        {
            string prefix = string.Format("[{0}] - WARN: ", loggerClassName);
            WriteToLogFile(prefix + message);
        }

        /// <summary>
        /// Designates error events that might still allow the application to continue running.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Error(string message)
        {
            string prefix = string.Format("[{0}] - ERROR: ", loggerClassName);
            WriteToLogFile(prefix + message);
        }

        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Fatal(string message)
        {
            string prefix = string.Format("[{0}] - FATAL: ", loggerClassName);
            WriteToLogFile(prefix + message);
        }

        private void WriteToLogFile(string message)
        {
            if (!File.Exists(App.LogFilePath))
            {
                Directory.CreateDirectory(App.LogFileLocation);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(message);
            sb.Append(string.Format(" [{0:HH:mm:ss tt}]", DateTime.Now));
            sb.Append("\r\n");

            File.AppendAllText(App.LogFilePath, sb.ToString());
            sb.Clear();

            Console.WriteLine(message);
        }
    }
}
