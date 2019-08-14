using System;
using System.Text;

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
            TimeSpan remoteOffset = GetUtcOffset(timeZone, date, true);
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
        /// Get UTC offset with possibility to count DST into it.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="allowDST">Do you want to dynamically calculate DST into it depending on current date? TRUE=yes, FALSE=count default offset of the zone only.</param>
        /// <returns></returns>
        public TimeSpan GetUtcOffset(TimeZoneInfo timeZone, bool allowDST = false)
        {
            return GetUtcOffset(timeZone, new DateTimeOffset(DateTime.Now), allowDST);
        }

        /// <summary>
        /// Get UTC offset with possibility to count DST into it.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="date"></param>
        /// <param name="allowDST">Do you want to dynamically calculate DST into it depending on current date? TRUE=yes, FALSE=count default offset of the zone only.</param>
        /// <returns></returns>
        public TimeSpan GetUtcOffset(TimeZoneInfo timeZone, DateTimeOffset date, bool allowDST = false)
        {
            if (!allowDST)
                return timeZone.BaseUtcOffset;

            // Allow to calculate offset with DST possibility.
            return timeZone.GetUtcOffset(date);
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

        /// <summary>
        /// Convert <see cref="TimeZoneInfo"/> into string representation.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="showUtcOffset"></param>
        /// <param name="allowDST"></param>
        /// <returns></returns>
        public string TimeZoneToString(TimeZoneInfo timeZone, bool showUtcOffset = false, bool allowDST = false)
        {
            return TimeZoneToString(timeZone, new DateTimeOffset(DateTime.Now), showUtcOffset, allowDST);
        }

        /// <summary>
        /// Convert <see cref="TimeZoneInfo"/> into string representation.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="date"></param>
        /// <param name="showUtcOffset"></param>
        /// <param name="allowDST"></param>
        /// <returns></returns>
        public string TimeZoneToString(TimeZoneInfo timeZone, DateTimeOffset date, bool showUtcOffset = false, bool allowDST = false)
        {
            string utcString, nameString;

            // Show UTC.
            if (showUtcOffset)
            {
                var offset = GetUtcOffset(timeZone, date, allowDST);
                if (offset >= TimeSpan.Zero)
                {
                    utcString = $"(UTC+{offset.ToString(@"hh\:mm")}) ";
                }
                else
                {
                    utcString = $"(UTC-{offset.ToString(@"hh\:mm")}) ";
                }
            }
            else
            {
                utcString = "";
            }

            // Allow DST.
            if (allowDST)
            {
                nameString = timeZone.IsDaylightSavingTime(date) ? timeZone.DaylightName : timeZone.StandardName;
            }
            else
            {
                nameString = timeZone.StandardName;
            }
            
            // Final string.
            return $"{utcString}{nameString}";
        }

        #endregion
    }
}
