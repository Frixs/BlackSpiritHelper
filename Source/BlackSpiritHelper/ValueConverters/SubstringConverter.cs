using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Substring converter.
    /// </summary>
    public class SubstringConverter : BaseValueConverter<SubstringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Substring(int.Parse((string)parameter));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
