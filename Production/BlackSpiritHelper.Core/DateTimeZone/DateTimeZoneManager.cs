using System;

namespace BlackSpiritHelper.Core
{
    public class DateTimeZoneManager : IDateTimeZone
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DateTimeZoneManager()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the time zone into the date.
        /// It does not convert date, just set the time zone of the date. The date stays the same.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public void SetTimeZone(ref DateTimeOffset date, TimeZoneInfo timeZone)
        {
            // Get offsets.
            TimeSpan remoteOffset = timeZone.GetUtcOffset(date);
            TimeSpan localOffset = date.Offset;

            // Transform local date and pretend it as remote.
            // Transform it to UTC first without impact on date. We just want to change time zone offset of the date.
            date = date.ToOffset(TimeSpan.Zero);
            date += localOffset;
            // Transform it from UTC to remote zone (again, without impact on date).
            date = date.ToOffset(remoteOffset);
            date -= remoteOffset;
        }

        /// <summary>
        /// Get offset difference between two given <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="wanted">The one which we want to go.</param>
        /// <param name="current">The one where we are currently at.</param>
        /// <returns></returns>
        public int GetDayDifferenceOffset(int wanted, int current)
        {
            if (wanted == current)
                return 0;

            if (current == 0)
                return wanted;

            return wanted - current;
        }

        /// <summary>
        /// Get offset, difference between two offsets.
        /// </summary>
        /// <param name="from">Offset.</param>
        /// <param name="to">Offset.</param>
        /// <returns></returns>
        public TimeSpan GetTimeZoneOffsetDifference(TimeSpan from, TimeSpan to)
        {
            return to - from;
        }

        #endregion
    }
}
