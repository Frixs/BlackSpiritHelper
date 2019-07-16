using System;
using System.Configuration;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// User preferences of the application content.
    /// </summary>
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class PreferencesViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Run the application on startup.
        /// </summary>
        public bool RunOnStartup { get; set; } = false;

        /// <summary>
        /// User audio alert level.
        /// </summary>
        public AudioAlert AudioAlertLevel { get; set; } = AudioAlert.None;

        /// <summary>
        /// List of all types of audio alerts.
        /// </summary>
        [XmlIgnore]
        public AudioAlert[] AudioAlertList { get; private set; } = (AudioAlert[])Enum.GetValues(typeof(AudioAlert));

        #endregion

        #region Command

        /// <summary>
        /// The command to set run the application on startup.
        /// </summary>
        [XmlIgnore]
        public ICommand RunOnStartUpCheckboxCommand { get; set; }

        /// <summary>
        /// The command to reset overlay position.
        /// </summary>
        [XmlIgnore]
        public ICommand ResetOverlayPositionCommand { get; set; }

        /// <summary>
        /// The command to open author webpage.
        /// </summary>
        [XmlIgnore]
        public ICommand AuthorWebpageLinkCommand { get; set; }

        /// <summary>
        /// The command to open donation link.
        /// </summary>
        [XmlIgnore]
        public ICommand AuthorDonateLinkCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PreferencesViewModel()
        {
            System.Console.WriteLine("HEY");
            // Create commands.
            CreateCommands();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Create commands.
        /// </summary>
        private void CreateCommands()
        {
            RunOnStartUpCheckboxCommand = new RelayCommand(() => RunOnStartUpCheckboxCommandMethod());
            ResetOverlayPositionCommand = new RelayCommand(() => ResetOverlayPositionMethod());
            AuthorWebpageLinkCommand = new RelayCommand(() => AuthorWebpageLinkMethod());
            AuthorDonateLinkCommand = new RelayCommand(() => AuthorDonateLinkMethod());
        }

        /// <summary>
        /// Run the application on system startup.
        /// </summary>
        private void RunOnStartUpCheckboxCommandMethod()
        {
            // TODO runo on startup.
            System.Console.WriteLine(RunOnStartup);
        }

        /// <summary>
        /// Reset overlay position to defualt.
        /// </summary>
        private void ResetOverlayPositionMethod()
        {
            // TODO reset overlay position.
            System.Console.WriteLine("ResetOverlayPositionTrigger");
        }

        /// <summary>
        /// Open author webpage.
        /// </summary>
        private void AuthorWebpageLinkMethod()
        {
            // Open the webpage.
            System.Diagnostics.Process.Start("https://github.com/Frixs");
        }

        /// <summary>
        /// Open donation weblink.
        /// </summary>
        private void AuthorDonateLinkMethod()
        {
            // TODO donation link.
        }

        #endregion
    }
}
