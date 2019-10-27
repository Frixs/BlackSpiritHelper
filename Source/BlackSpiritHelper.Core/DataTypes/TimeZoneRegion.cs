using System;
using System.ComponentModel;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Time zone IDs.
    /// It is <see cref="TimeZoneInfo"/> list with the only key time zones as a list of regions.
    /// Description contains <see cref="TimeZoneInfo.Id"/>.
    /// </summary>
    public enum TimeZoneRegion
    {
        /// <summary>
        /// The default value.
        /// If the searched value is not in this enum list, this is the default value.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// UTC: worldwide standard.
        /// </summary>
        [Description("UTC")]
        UTC = 1,

        /// <summary>
        /// Europe.
        /// </summary>
        [Description("Central Europe Standard Time")]
        EU = 10,

        /// <summary>
        /// North America.
        /// </summary>
        [Description("Pacific Standard Time")]
        NA = 11,

        /// <summary>
        /// Russia.
        /// </summary>
        [Description("Russian Standard Time")]
        RU = 12,

        /// <summary>
        /// Japan.
        /// </summary>
        [Description("Tokyo Standard Time")]
        JP = 13,

        /// <summary>
        /// Korea.
        /// </summary>
        [Description("Korea Standard Time")]
        KR = 14,

        /// <summary>
        /// Middle East and North Africa.
        /// </summary>
        [Description("Turkey Standard Time")]
        MENA = 15,

        /// <summary>
        /// South America.
        /// </summary>
        [Description("E. South America Standard Time")]
        SA = 16,

        /// <summary>
        /// South East Asia.
        /// </summary>
        [Description("Singapore Standard Time")]
        SEA = 17,

        /// <summary>
        /// Thailand.
        /// </summary>
        [Description("SE Asia Standard Time")]
        TH = 18,

        /// <summary>
        /// Thailand.
        /// </summary>
        [Description("Taipei Standard Time")]
        TW = 19,
    }
}
