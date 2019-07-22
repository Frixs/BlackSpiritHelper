using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Invert boolean.
    /// </summary>
    public class InvertBooleanConverter : BaseValueConverter<InvertBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                IoC.Logger.Log("The target must be a boolean!", LogLevel.Fatal);
                throw new InvalidOperationException("The target must be a boolean!");
            }

            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
