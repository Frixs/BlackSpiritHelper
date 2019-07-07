using System;
using System.Globalization;
using System.Windows.Media;

namespace BlackSpiritHelper
{
    /// <summary>
    /// A converter that takes in an RGB string such as FF00FF and converts it to a WPF brush.
    /// </summary>
    public class StringRGBToBrushConverter : BaseValueConverter<StringRGBToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string)value))
                return (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));

            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{value}"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
