using System;
using System.Diagnostics;
using System.IO;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// List of all directory paths that are needed to run this application.
    /// </summary>
    public class SettingsConfiguration
    {
        /// <summary>
        /// Uniform application name for the whole application.
        /// It is used for name of settings directory e.g. etc.
        /// </summary>
        public static readonly string ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name;

        /// <summary>
        /// The user settings (config) directory location path.
        /// </summary>
        public static readonly string DataDirPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            ApplicationName.Split('.')[0]
            );

        /// <summary>
        /// The user settings (config) file path.
        /// </summary>
        public static readonly string UserConfigFilePath = Path.Combine(
            DataDirPath,
            Debugger.IsAttached ? "user.debug.config" : "user.config"
            );

        /// <summary>
        /// The user APM Calculator section archive file path.
        /// </summary>
        public static readonly string ApmCalculatorArchiveFilePath = Path.Combine(
            DataDirPath,
            Debugger.IsAttached ? "apm_archive.debug.csv" : "apm_archive.csv"
            );

        /// <summary>
        /// Application data directory location path (downloadable data)
        /// ---
        /// If we are in debug mode, we have relative path in execution directory.
        /// </summary>
        public static readonly string ApplicationDataDirPath = Debugger.IsAttached
            ? "Data/"
            : Path.Combine(
                DataDirPath,
                "Data/"
                );

        /// <summary>
        /// Remote data directory relative path.
        /// </summary>
        public static readonly string RemoteDataDirRelPath = @"Source/BlackSpiritHelper.Core/Data/";

        /// <summary>
        /// Remote data directory location path.
        /// </summary>
        public static readonly string RemoteDataDirPath = Path.Combine(
            @"https://api.github.com/repos/Frixs/BlackSpiritHelper/contents/",
            RemoteDataDirRelPath
            );

        /// <summary>
        /// Remote patch notes URL file path.
        /// </summary>
        public static readonly string RemotePatchNotesFilePath = @"https://raw.githubusercontent.com/Frixs/BlackSpiritHelper/master/Release/patch_notes.md";

        /// <summary>
        /// Remote news URL file path.
        /// </summary>
        public static readonly string RemoteNewsFilePath = @"https://raw.githubusercontent.com/Frixs/BlackSpiritHelper/master/Release/news.md";

        /// <summary>
        /// Update active user conter URL.
        /// </summary>
        public static readonly string UpdateActiveUserCounterURL = @"https://us-central1-blackspirithelper.cloudfunctions.net/registerConnection";
    }
}
