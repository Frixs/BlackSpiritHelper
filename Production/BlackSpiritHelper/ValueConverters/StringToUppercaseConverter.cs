using System;
using System.Globalization;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts string to uppercase string.
    /// </summary>
    public class StringToUppercaseConverter : BaseValueConverter<StringToUppercaseConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).ToUpper();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
