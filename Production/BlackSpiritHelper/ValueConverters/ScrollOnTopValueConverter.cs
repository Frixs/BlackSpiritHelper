using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts the scroll height to bool, False = scroll is on top, True = else.
    /// </summary>
    public class IsScrollOnTopByOffsetValueConverter : BaseValueConverter<IsScrollOnTopByOffsetValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((double)value > 0);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
