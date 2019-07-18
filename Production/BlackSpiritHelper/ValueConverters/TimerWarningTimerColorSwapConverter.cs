using BlackSpiritHelper.Core;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts <see cref="TimerItemDataViewModel.IsWarningTime"/> into correct color.
    /// </summary>
    public class TimerWarningTimerColorSwapConverter : BaseValueConverter<TimerWarningTimerColorSwapConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val;
            Color par;

            // Check data type.
            if (value.GetType() == typeof(bool) && parameter.GetType() == typeof(Color))
            {
                val = (bool)value;
                par = (Color)parameter;
            }
            else
                return new SolidColorBrush(Colors.Wheat);

            // Swap colors.
            if (val)
                return new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(Application.Current.Resources["TimerListItemWarningColor"].ToString())
                    );
            else
                return new SolidColorBrush(par);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
