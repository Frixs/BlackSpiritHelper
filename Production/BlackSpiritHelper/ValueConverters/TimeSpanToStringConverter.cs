using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts boolean to visibility - Visible/Hidden.
    /// </summary>
    public class TimeSpanToStringConverter : BaseValueConverter<TimeSpanToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan ts;

            if (value.GetType() == typeof(TimeSpan))
                ts = (TimeSpan)value;
            else
                return "--:--";

            // Parameter to select time format.
            if (parameter.Equals("hms"))
            {
                return ts.ToString("hh':'mm':'ss");
            }
            else if (parameter.Equals("h*ms"))
            {
                double n = Math.Floor(ts.TotalMinutes);
                if (n < 100)
                    return string.Format("{0}:{1:D2}",
                    n,
                    ts.Seconds
                    );

                return string.Format("{0}h", Math.Ceiling(ts.TotalHours));
            }
            else
            {
                return "--:--";
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
