using BlackSpiritHelper.Core;
using System;
using System.Globalization;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts boolean to visibility - Visible/Hidden.
    /// </summary>
    public class BooleanToVisibilityGoneConverter : BaseValueConverter<BooleanToVisibilityGoneConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val;
            // Get value.
            if (value.GetType().Equals(typeof(bool)))
            {
                val = (bool)value;
            }
            else
            {
                IoC.Logger.Log($"The target is in invalid type ({value.GetType().ToString()})!", LogLevel.Fatal);
                throw new InvalidOperationException($"The target is in invalid type ({value.GetType().ToString()})!");
            }

            // Solve.
            if (parameter == null)
                return val ? Visibility.Visible : Visibility.Collapsed;
            else
                return val ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
