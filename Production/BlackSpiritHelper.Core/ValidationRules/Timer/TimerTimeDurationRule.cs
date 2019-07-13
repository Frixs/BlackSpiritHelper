using System;
using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.TimeDuration"/>.
    /// </summary>
    public class TimerTimeDurationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimeSpan val;

            // Get correct type.
            if (value.GetType() == typeof(TimeSpan))
                val = (TimeSpan)value;
            else
                return new ValidationResult(false, "Not a TimeSpan type.");

            // Check conditions.
            if (val.Ticks > TimerItemViewModel.TimeAllowMaxDuration.Ticks || val.Ticks < TimerItemViewModel.TimeAllowMinDuration.Ticks)
                return new ValidationResult(false, $"Please enter a time in the range: {TimerItemViewModel.TimeAllowMinDuration.ToString("dd':'hh':'mm':'ss")} - {TimerItemViewModel.TimeAllowMaxDuration.ToString("dd':'hh':'mm':'ss")}.");

            return ValidationResult.ValidResult;
        }
    }
}
