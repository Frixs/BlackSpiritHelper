using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Wrapper with action settings on failure.
    /// </summary>
    public class WatchdogFailureRoutineDataViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Indicates command to turn computer into another state.
        /// </summary>
        private bool mIsComputerActionSelected = false;

        /// <summary>
        /// Indicates command to send message to user preffered social site.
        /// Only if user has settings set.
        /// </summary>
        private bool mIsSendMessageSelected = false;

        /// <summary>
        /// Indicates to play sound alert.
        /// </summary>
        private bool mIsSoundAlertSelected = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates command to turn computer into another state.
        /// Specified in <see cref="ComputerAction"/>.
        /// </summary>
        public bool IsComputerActionSelected
        {
            get => mIsComputerActionSelected;
            set
            {
                mIsComputerActionSelected = value;

                // Turn off sound alert. No point to play sound alert on computer action.
                if (value)
                    IsSoundAlertSelected = false;
            }
        }

        /// <summary>
        /// Specify computer action for <see cref="IsComputerActionSelected"/>.
        /// </summary>
        public WatchdogComputerAction ComputerAction { get; set; } = WatchdogComputerAction.LogOff;

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
        public bool IsSendMessageSelected
        {
            get => mIsSendMessageSelected;
            set => mIsSendMessageSelected = value;
        }

        /// <summary>
        /// Indicates to play sound alert.
        /// </summary>
        public bool IsSoundAlertSelected
        {
            get => mIsSoundAlertSelected;
            set => mIsSoundAlertSelected = value;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WatchdogFailureRoutineDataViewModel()
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

        #region Private Methods (Handle Methods)

        /// <summary>
        /// Handle computer action event of failure action.
        /// </summary>
        private void HandleComputerAction()
        {
            if (!IsComputerActionSelected)
                return;

            // Save userdata before.
            IoC.DataContent.SaveUserData();

            // Do.
            if (ComputerAction == WatchdogComputerAction.Shutdown)
                ComputerShutdown();
            if (ComputerAction == WatchdogComputerAction.Restart)
                ComputerRestart();
            else
                ComputerLogOff();
        }

        /// <summary>
        /// Handle sending message to user preferred platform.
        /// </summary>
        private void HandleMessageSending()
        {
            if (!IsSendMessageSelected)
                return;

            // Log it.
            IoC.DataContent.WatchdogData.Log("Sending message to user...");

            // Get current time to message.
            var currentTime = DateTimeOffset.UtcNow.ToString("MM-dd HH:mm UTC");
            string message = $"Connection lost! Accident time: {currentTime}";

            // Send message.
            int status = IoC.DataContent.PreferencesData.Connection.SendTextMessage(message);

            // Resolve sending status.
            if (status == 2)
            {
                IoC.DataContent.WatchdogData.Log("Message not sent! No active connection.");
            }
            else if (status == 1)
            {
                IoC.DataContent.WatchdogData.Log("Cannot send message at the moment.");
                IoC.DataContent.WatchdogData.Log("The message will be sent as soon as the connection is available.");
                IoC.DataContent.PreferencesData.Connection.AddNewPendingMessage(message);
            }
            else
            {
                IoC.DataContent.WatchdogData.Log("Message successfully sent!");
            }
        }

        /// <summary>
        /// Handle sound effect of failure action.
        /// </summary>
        private void HandleSoundAlert()
        {
            if (!IsSoundAlertSelected)
                return;

            IoC.Audio.Play(AudioType.Alert5, AudioPriorityBracket.Pack);
        }

        #endregion

        #region Private Methods (Computer Action)

        /// <summary>
        /// Force computer to log off user.
        /// </summary>
        private void ComputerLogOff()
        {
            // Log it.
            IoC.DataContent.WatchdogData.Log("Logging off...");

            // Check permissions.
            bool isElevated = IoC.Application.IsRunningAsAdministratorCheck;
            if (!isElevated)
            {
                IoC.DataContent.WatchdogData.Log("No permissions to perform the operation!");
                return;
            }

            // Run command.
            Process.Start("shutdown", "/l /f /t 0");
        }

        /// <summary>
        /// Force computer to restart.
        /// </summary>
        private void ComputerRestart()
        {
            // Log it.
            IoC.DataContent.WatchdogData.Log("Restarting computer...");

            // Check permissions.
            bool isElevated = IoC.Application.IsRunningAsAdministratorCheck;
            if (!isElevated)
            {
                IoC.DataContent.WatchdogData.Log("No permissions to perform the operation!");
                return;
            }

            // Run command.
            Process.Start("shutdown", "/r /f /t 0");
        }

        /// <summary>
        /// Force computer to shutdown.
        /// </summary>
        private void ComputerShutdown()
        {
            // Log it.
            IoC.DataContent.WatchdogData.Log("Shutting down computer...");

            // Check permissions.
            bool isElevated = IoC.Application.IsRunningAsAdministratorCheck;
            if (!isElevated)
            {
                IoC.DataContent.WatchdogData.Log("No permissions to perform the operation!");
                return;
            }

            // Run command.
            Process.Start("shutdown", "/s /f /t 0");
        }

        #endregion
    }
}
