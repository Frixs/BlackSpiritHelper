using System;
using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemDataViewModel.TimeDuration"/>.
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
            if (val.Ticks > TimerItemDataViewModel.TimeAllowMaxDuration.Ticks || val.Ticks < TimerItemDataViewModel.TimeAllowMinDuration.Ticks)
                return new ValidationResult(false, $"Please enter a time in the range: {TimerItemDataViewModel.TimeAllowMinDuration.ToString("dd':'hh':'mm':'ss")} - {TimerItemDataViewModel.TimeAllowMaxDuration.ToString("dd':'hh':'mm':'ss")}.");

            return ValidationResult.ValidResult;
        }
    }
}
