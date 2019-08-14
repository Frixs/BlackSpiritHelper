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
        /// Get UTC offset with possibility to count DST into it.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="allowDST">Do you want to dynamically calculate DST into it depending on current date? TRUE=yes, FALSE=count default offset of the zone only.</param>
        /// <returns></returns>
        TimeSpan GetUtcOffset(TimeZoneInfo timeZone, bool allowDST = false);

        /// <summary>
        /// Get UTC offset with possibility to count DST into it.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="date"></param>
        /// <param name="allowDST">Do you want to dynamically calculate DST into it depending on current date? TRUE=yes, FALSE=count default offset of the zone only.</param>
        /// <returns></returns>
        TimeSpan GetUtcOffset(TimeZoneInfo timeZone, DateTimeOffset date, bool allowDST = false);

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

        /// <summary>
        /// Convert <see cref="TimeZoneInfo"/> into string representation.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="showUtcOffset"></param>
        /// <param name="allowDST"></param>
        /// <returns></returns>
        string TimeZoneToString(TimeZoneInfo timeZone, bool showUtcOffset = false, bool allowDST = false);

        /// <summary>
        /// Convert <see cref="TimeZoneInfo"/> into string representation.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="date"></param>
        /// <param name="showUtcOffset"></param>
        /// <param name="allowDST"></param>
        /// <returns></returns>
        string TimeZoneToString(TimeZoneInfo timeZone, DateTimeOffset date, bool showUtcOffset = false, bool allowDST = false);
    }
}
