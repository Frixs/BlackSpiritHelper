using BlackSpiritHelper.Core;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts enum to boolean for RadioButton.
    /// </summary>
    public class RadioEnumBooleanConverter : BaseValueConverter<RadioEnumBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBlock par = parameter as TextBlock;
            if (par == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), par.Text);

            return parameterValue.Equals(value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBlock par = parameter as TextBlock;
            if (par == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, par.Text);
        }
    }
}
