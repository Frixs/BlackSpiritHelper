using System;
using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemDataViewModel.CountdownDuration"/>.
    /// Enter TotalSeconds of TimeSpan.
    /// </summary>
    public class TimerCountdownDurationRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val;
            object oVal = GetBoundValue(value);

            // Check data type.
            if (oVal.GetType() == typeof(double))
                val = (double)oVal;
            else
                return new ValidationResult(false, "Not a double type.");

            // Check conditions.
            if (val > TimerItemDataViewModel.CountdownAllowMaxDuration.TotalSeconds || val < 0)
                return new ValidationResult(false, $"Please enter a time in the range: 0 - {TimerItemDataViewModel.CountdownAllowMaxDuration.ToString("dd':'hh':'mm':'ss")}.");

            return ValidationResult.ValidResult;
        }
    }
}
