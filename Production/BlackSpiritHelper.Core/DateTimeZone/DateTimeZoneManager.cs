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

        #endregion
    }
}
