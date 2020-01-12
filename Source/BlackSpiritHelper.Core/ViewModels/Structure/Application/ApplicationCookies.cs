using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Cookies are used to for setup application.
    /// Data you need immediately after application start and can be loaded without any other needs/references.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ApplicationCookies
    {
        #region Public Properties

        /// <summary>
        /// Last opened page during last data save event.
        /// ---
        /// <see cref="Update"/> only.
        /// </summary>
        public byte LastOpenedPage { get; set; } = 0;

        /// <summary>
        /// Command to run application As Administrator at startup.
        /// ---
        /// <see cref="Update"/> only.
        /// </summary>
        public bool ForceToRunAsAdministrator { get; set; } = false;

        /// <summary>
        /// Size of the MainWindow during last data save event.
        /// ---
        /// <see cref="Update"/> only.
        /// </summary>
        public Vector2Double MainWindowSize { get; set; } = new Vector2Double(803, 450);

        /// <summary>
        /// List of pending messages that cannot be send at the time of need.
        /// Handler: <see cref="PreferencesDataViewModel.Connection"/>.
        /// </summary>
        public List<string> PendingMessageList { get; set; } = new List<string>();

        /// <summary>
        /// Says, what is the latest time of patch notes review by the user.
        /// It is for routine which checks user's value with the one online.
        /// If the online values gets updated, it fires the event.
        /// </summary>
        [XmlIgnore]
        public DateTime PatchNotesLatestReviewDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// String version of <see cref="PatchNotesLatestReviewDate"/>.
        /// </summary>
        public string PatchNotesLatestReviewDateStr
        {
            get => PatchNotesLatestReviewDate.ToString("yyyy-MM-dd");
            set
            {
                DateTime date;
                DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                PatchNotesLatestReviewDate = date;
            }
        }

        /// <summary>
        /// Says, what is the latest time of news review by the user.
        /// It is for routine which checks user's value with the one online.
        /// If the online values gets updated, it fires the event.
        /// </summary>
        [XmlIgnore]
        public DateTime NewsLatestReviewDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// String version of <see cref="NewsLatestReviewDate"/>.
        /// </summary>
        public string NewsLatestReviewDateStr
        {
            get => NewsLatestReviewDate.ToString("yyyy-MM-dd");
            set
            {
                DateTime date;
                DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                NewsLatestReviewDate = date;
            }
        }

        /// <summary>
        /// Says, if the app has been already firstly launched or not.
        /// </summary>
        public bool AlreadyFirstlyLaunched { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApplicationCookies()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update all cookie data.
        /// </summary>
        public void Update()
        {
            LastOpenedPage = (byte)IoC.Application.CurrentPage;
            ForceToRunAsAdministrator = IoC.DataContent.PreferencesData.ForceToRunAsAdministrator;
            MainWindowSize = IoC.UI.GetMainWindowSize();
        }

        #endregion
    }
}
