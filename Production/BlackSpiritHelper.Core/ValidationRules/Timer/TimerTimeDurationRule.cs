using System;
using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.TimeDuration"/>.
    /// </summary>
    public class TimerTimeDurationRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimeSpan val;
            object oVal = GetBoundValue(value);

            // Check data type.
            if (oVal.GetType() == typeof(TimeSpan))
                val = (TimeSpan)oVal;
            else
                return new ValidationResult(false, "Not a TimeSpan type.");

            // Check conditions.
            if (val.Ticks > TimerItemViewModel.TimeAllowMaxDuration.Ticks || val.Ticks < TimerItemViewModel.TimeAllowMinDuration.Ticks)
                return new ValidationResult(false, $"Please enter a time in the range: {TimerItemViewModel.TimeAllowMinDuration.ToString("dd':'hh':'mm':'ss")} - {TimerItemViewModel.TimeAllowMaxDuration.ToString("dd':'hh':'mm':'ss")}.");

            return ValidationResult.ValidResult;
        }
    }
}
