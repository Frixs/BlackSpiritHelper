namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// The severity of the log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Developer specific.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Lot all information except debug information.
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// General information
        /// </summary>
        Info = 3,

        /// <summary>
        /// Designates potentially harmful situations..
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Designates error events that might still allow the application to continue running.
        /// </summary>
        Error = 5,

        /// <summary>
        /// Designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        Fatal = 6,

        /// <summary>
        /// A success.
        /// </summary>
        Success = 7,
    }
}
