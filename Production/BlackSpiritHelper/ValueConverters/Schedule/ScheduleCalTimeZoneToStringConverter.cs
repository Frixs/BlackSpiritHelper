using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts time zone of schedule to string.
    /// </summary>
    public class ScheduleCalTimeZoneToStringConverter : BaseValueConverter<ScheduleCalTimeZoneToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (
                !value.GetType().Equals(typeof(RegionTimeZone))
                )
            {
                return "";
            }

            if (parameter == null)
                return TimeZoneInfo.FindSystemTimeZoneById(((RegionTimeZone)value).GetDescription()).ToString();
            return TimeZoneInfo.Local.ToString();

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
