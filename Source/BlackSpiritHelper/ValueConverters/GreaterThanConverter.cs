using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Returns bool based on if the value is greater than parameter value
    /// </summary>
    public class GreaterThanConverter : BaseValueConverter<GreaterThanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.GetType();

            if (type == typeof(int))
                return (int)value > int.Parse(parameter.ToString());
            else if (type == typeof(uint))
                return (uint)value > uint.Parse(parameter.ToString());
            else if (type == typeof(float))
                return (float)value > float.Parse(parameter.ToString());
            else if (type == typeof(double))
                return (double)value > double.Parse(parameter.ToString());
            else
                throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
