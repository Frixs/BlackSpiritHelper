using System;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper with action settings on failure.
    /// </summary>
    public class WatchdogFailureActionDataViewModel
    {
        #region Public Properties

        /// <summary>
        /// Indicates command to turn computer into another state.
        /// Specified in <see cref="ComputerAction"/>.
        /// </summary>
        public bool IsComputerActionSelected { get; set; } = false;

        /// <summary>
        /// Specify computer action for <see cref="IsComputerActionSelected"/>.
        /// </summary>
        public WatchdogComputerAction ComputerAction { get; set; } = WatchdogComputerAction.Restart;

        /// <summary>
        /// List of all possile types of computer action.
        /// Presenter for UI.
        /// </summary>
        [XmlIgnore]
        public WatchdogComputerAction[] ComputerActionList { get; set; } = (WatchdogComputerAction[])Enum.GetValues(typeof(WatchdogComputerAction));

        /// <summary>
        /// Indicates command to send message to user preffered social site.
        /// Only if user has settings set.
        /// </summary>
        public bool IsSendMessageSelected { get; set; } = false;

        /// <summary>
        /// Indicates to play sound alert.
        /// </summary>
        public bool IsSoundAlertSelected { get; set; } = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogFailureActionDataViewModel()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Do all the failure actions.
        /// </summary>
        public void Do()
        {
            HandleSoundAlert();
            HandleMessageSending();
            HandleComputerAction();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handle computer action event of failure action.
        /// </summary>
        private void HandleComputerAction()
        {
            // TODO
        }

        /// <summary>
        /// Handle sending message to user preferred platform.
        /// </summary>
        private void HandleMessageSending()
        {
            // TODO
        }

        /// <summary>
        /// Handle sound effect of failure action.
        /// </summary>
        private void HandleSoundAlert()
        {
            // TODO
        }

        #endregion
    }
}
