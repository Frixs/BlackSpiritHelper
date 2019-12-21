using System.Configuration;

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
