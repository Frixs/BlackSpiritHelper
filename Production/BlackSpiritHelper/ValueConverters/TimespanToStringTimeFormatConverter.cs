using System;
using System.Globalization;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts boolean to visibility - Visible/Hidden.
    /// </summary>
    public class TimespanToStringTimeFormatConverter : BaseValueConverter<TimespanToStringTimeFormatConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan t = TimeSpan.FromSeconds((double)value);

            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds
                );
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = "";

            try
            {
                ret = DateTime.ParseExact(((TimeSpan)value).ToString(), "HH:mm:ss", null).TimeOfDay.TotalSeconds.ToString();
            }
            catch (FormatException ex)
            {
                return null;
            }
            
            return ret;
        }
    }
}
