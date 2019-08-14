using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Check if the string contains leading character according to passed as parameter.
    /// </summary>
    public class ContainsLeadingCharConverter : BaseValueConverter<ContainsLeadingCharConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value)[0] == char.Parse((string)parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
