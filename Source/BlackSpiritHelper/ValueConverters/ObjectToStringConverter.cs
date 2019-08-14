using System;
using System.Globalization;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts object to string.
    /// </summary>
    public class ObjectToStringConverter : BaseValueConverter<ObjectToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
