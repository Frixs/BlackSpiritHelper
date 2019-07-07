using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;

namespace BlackSpiritHelper
{
    /// <summary>
    /// A converter that takes in an RGB string such as FF00FF and converts it to a WPF brush.
    /// </summary>
    public class StringRGBToLightOrDarkBrushConverter : BaseValueConverter<StringRGBToLightOrDarkBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string)value))
                value = "FFFFFF";

            string[] parameters = (parameter as string).Split('|');

            // Get RGB component.
            int rComponent = int.Parse(((string)value).Substring(0, 2), NumberStyles.HexNumber);
            int gComponent = int.Parse(((string)value).Substring(2, 2), NumberStyles.HexNumber);
            int bComponent = int.Parse(((string)value).Substring(4, 2), NumberStyles.HexNumber);

            // Count components together.
            int count = rComponent + gComponent + bComponent;

            // Get edge to change color.
            int limit = (255 * 3) / 2;

            string finalColor;
            if (count < limit)
                finalColor = parameters[0]; // Light.
            else
                finalColor = parameters[1]; // Dark.

            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{finalColor}"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
