using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Multiply number
    /// </summary>
    public class MultiplyConverter : BaseValueConverter<MultiplyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = double.Parse(value.ToString());
            double mp = double.Parse(parameter.ToString());

            if (value.GetType() == typeof(int))
                return (int)(val * mp);
            else if (value.GetType() == typeof(double))
                return (double)(val * mp);
            else if (value.GetType() == typeof(float))
                return (float)(val * mp);

            throw new Exception("Converter multiply error!");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
