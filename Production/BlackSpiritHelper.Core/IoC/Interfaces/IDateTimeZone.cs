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

        /// <summary>
        /// Get offset difference between two given <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="wanted">The one which we want to go.</param>
        /// <param name="current">The one where we are currently at.</param>
        /// <returns></returns>
        int GetDayDifferenceOffset(int wanted, int current);

        /// <summary>
        /// Get offset, difference between two offsets.
        /// </summary>
        /// <param name="from">Offset.</param>
        /// <param name="to">Offset.</param>
        /// <returns></returns>
        TimeSpan GetTimeZoneOffsetDifference(TimeSpan from, TimeSpan to);
    }
}
