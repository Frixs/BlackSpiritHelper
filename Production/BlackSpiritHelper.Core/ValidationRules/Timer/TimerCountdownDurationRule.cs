using System;
using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.CountdownDuration"/>.
    /// Enter TotalSeconds of TimeSpan.
    /// </summary>
    public class TimerCountdownDurationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val;

            // Get correct type.
            if (value.GetType() == typeof(double))
                val = (double)value;
            else
                return new ValidationResult(false, "Not a double type.");

            // Check conditions.
            if (val > TimerItemViewModel.CountdownAllowMaxDuration.TotalSeconds || val < 0)
                return new ValidationResult(false, $"Please enter a time in the range: 0 - {TimerItemViewModel.CountdownAllowMaxDuration.ToString("dd':'hh':'mm':'ss")}.");

            return ValidationResult.ValidResult;
        }
    }
}
