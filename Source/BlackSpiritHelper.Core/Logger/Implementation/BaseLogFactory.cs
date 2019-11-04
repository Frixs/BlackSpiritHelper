using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The standard log factory.
    /// Logs details to the Debug by default.
    /// </summary>
    public class BaseLogFactory : ILogFactory
    {
        #region Protected Members

        /// <summary>
        /// The list of loggers in this factory.
        /// </summary>
        protected List<ILogger> mLoggers = new List<ILogger>();

        /// <summary>
        /// A lock for the logger list to keep it thread-safe.
        /// </summary>
        protected object mLoggersLock = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// The level of logging to output.
        /// </summary>
        public LogOutputLevel LogOutputLevel { get; set; }

        /// <summary>
        /// If true, includes the origin of where the log message was logged from such as the class name, line number and file name.
        /// </summary>
        public bool IncludeLogOriginDetails { get; set; } = true;

        #endregion

        #region Public Events

        /// <summary>
        /// Fires whenever a new log arrives.
        /// </summary>
        public event Action<(string Message, LogLevel Level)> NewLog = (details) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="loggers">The loggers to add to the factory, on top of the stock loggers already included.</param>
        public BaseLogFactory(ILogger[] loggers = null)
        {
            // Add logger.
            AddLogger(new ConsoleLogger());

            // Add any others passed in.
            if (loggers != null)
                foreach (var logger in loggers)
                    AddLogger(logger);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specific logger to this factory.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public void AddLogger(ILogger logger)
        {
            // Log the list so it is thread-safe
            lock (mLoggersLock)
            {
                // If the logger is not already in the list...
                if (!mLoggers.Contains(logger))
                    // Add the logger to the list
                    mLoggers.Add(logger);
            }
        }

        /// <summary>
        /// Removes the specified logger from this factory.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public void RemoveLogger(ILogger logger)
        {
            // Log the list so it is thread-safe.
            lock (mLoggersLock)
            {
                // If the logger is in the list...
                if (mLoggers.Contains(logger))
                    // Remove the logger from the list.
                    mLoggers.Remove(logger);
            }
        }

        /// <summary>
        /// Logs the specific message to all loggers in this factory.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The level of the message being logged.</param>
        /// <param name="origin">The method/function this message was logged in.</param>
        /// <param name="filePath">The code filename that this message was logged from.</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from.</param>
        public void Log(
            string message,
            LogLevel level = LogLevel.Info,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            // If we should not log the message as the level is too low...
            if ((int)level < (int)LogOutputLevel)
                return;

            // If the user wants to know where the log originated from...
            if (IncludeLogOriginDetails)
            {
                message = $"[{level.ToString().ToUpper()}] [{Path.GetFileName(filePath)} > {origin}() > Line {lineNumber}] {message}";
            }

            // Log to all loggers.
            mLoggers.ForEach(logger => logger.Log(message, level));

            // Inform listeners.
            NewLog.Invoke((message, level));
        }

        /// <summary>
        /// Get all log files.
        /// </summary>
        /// <returns></returns>
        public List<FileInfo> GetLogFiles()
        {
            var ret = new List<FileInfo>();

            foreach (ILogger logger in mLoggers)
            {
                if (!logger.GetType().Equals(typeof(FileLogger)))
                    continue;

                ret.Add(
                    new FileInfo(((FileLogger)logger).FilePath)
                    );
            }

            return ret;
        }

        /// <summary>
        /// Clean all log files.
        /// Clear very old log messages etc.
        /// Be very careful with using this method. It can take some time to process it.
        /// </summary>
        public void CleanLogFiles()
        {
            int maxAllowedLineCount = 5000;
            short timeout;

            foreach (FileInfo fi in GetLogFiles())
            {
                timeout = 10; // tries.
                while (IoC.File.IsInUse(fi))
                {
                    Thread.Sleep(1000); // Wait for some amount of time before another try.
                    if (timeout > 0)
                    {
                        timeout -= 1;
                    }
                    else
                    {
                        IoC.Logger.Log($"Cannot access log file '{fi.Name}'!", LogLevel.Error);
                        return;
                    }
                }

                // Count lines in the log file.
                var count = (int)IoC.File.LineCount(fi.FullName);

                // CHeck if the file should be cleaned.
                if (count > maxAllowedLineCount)
                {
                    var lines = IoC.File.ReadLines(fi.FullName);

                    using (var writer = (TextWriter)new StreamWriter(File.Open(fi.FullName, FileMode.Create)))
                    {
                        for (int i = count / 2; i < lines.Count; i++)
                            writer.WriteLine(lines[i]);
                    }
                }
            }
        }

        #endregion
    }
}
