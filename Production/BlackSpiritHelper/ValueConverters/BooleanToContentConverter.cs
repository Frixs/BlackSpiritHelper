using System;
using System.Globalization;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts boolean to content that will be displayed according to paramter.
    /// Parameter is separated into 2 values with '|'.
    /// True = 1st parameter, False = 2nd parameter.
    /// </summary>
    public class BooleanToContentConverter : BaseValueConverter<IoCConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parameters = (parameter as string).Split('|');

            return (bool)value ? parameters[0] : parameters[1];
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
