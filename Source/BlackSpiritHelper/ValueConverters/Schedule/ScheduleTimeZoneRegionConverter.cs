using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts enum to boolean for RadioButton.
    /// </summary>
    public class ScheduleTimeZoneRegionConverter : BaseValueConverter<ScheduleTimeZoneRegionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Prevention against Type erros.
            if (!value.GetType().Equals(typeof(TimeZoneRegion)))
                return "";

            return ((Enum)value).ToString() + " (" + ((Enum)value).GetDescription() + ")";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
