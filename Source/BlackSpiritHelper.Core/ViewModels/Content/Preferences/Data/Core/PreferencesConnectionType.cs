using System.ComponentModel;

namespace BlackSpiritHelper.Core
{
    public enum PreferencesConnectionType
    {
        /// <summary>
        /// None - no method selected.
        /// </summary>
        [Description("NONE")]
        None = 0,

        /// <summary>
        /// Discord connection.
        /// </summary>
        [Description("DISCORD")]
        Discord = 1,
    }
}
