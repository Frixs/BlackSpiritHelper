using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Check for null
    /// </summary>
    public class IsNullConverter : BaseValueConverter<IsNullConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
