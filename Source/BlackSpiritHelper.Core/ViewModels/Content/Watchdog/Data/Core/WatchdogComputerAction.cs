using System.ComponentModel;

namespace BlackSpiritHelper.Core
{
    public enum WatchdogComputerAction
    {
        /// <summary>
        /// Indicates, computer should log off currently logged in user.
        /// </summary>
        [Description("Log off")]
        LogOff = 0,

        /// <summary>
        /// Indicates, computer should restart.
        /// </summary>
        [Description("Restart")]
        Restart = 1,

        /// <summary>
        /// Indicates, computer will shutown.
        /// </summary>
        [Description("Shutdown")]
        Shutdown = 2,
    }
}
