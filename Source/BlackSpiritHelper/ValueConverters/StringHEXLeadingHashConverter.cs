using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// A converter that adds '#' at the beginning of string.
    /// </summary>
    public class StringHEXLeadingHashConverter : BaseValueConverter<StringHEXLeadingHashConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "#" + (string)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString();
            return val.ToHexStringWithoutHashmark();
        }
    }
}
