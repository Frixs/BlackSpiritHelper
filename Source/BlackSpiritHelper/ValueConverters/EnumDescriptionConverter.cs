using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts enum to its description string shape.
    /// </summary>
    public class EnumDescriptionConverter : BaseValueConverter<EnumDescriptionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Enum)value).GetDescription();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
