using System;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Managing date with time zones.
    /// </summary>
    public interface IDateTimeZone
    {
        /// <summary>
        /// Set the time zone into the date.
        /// It does not convert date, just set the time zone of the date. The date stays the same.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZone"></param>
        void SetTimeZone(ref DateTimeOffset date, TimeZoneInfo timeZone);
    }
}
