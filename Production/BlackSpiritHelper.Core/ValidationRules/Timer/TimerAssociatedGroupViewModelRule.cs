using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule to check free space in group... to be able to change timer's group - <see cref="TimerItemViewModel.GroupID"/>.
    /// </summary>
    public class TimerAssociatedGroupViewModelRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimerGroupViewModel val;

            // Get correct type.
            if (value.GetType() == typeof(TimerGroupViewModel))
                val = (TimerGroupViewModel)value;
            else
                return new ValidationResult(false, "Not a timer group model.");

            // Check conditions.
            if (!val.CanCreateNewTimer)
                return new ValidationResult(false, $"The group is already full. Maximal number of timers in 1 group is {TimerGroupViewModel.AllowedMaxNoOfTimers}.");

            return ValidationResult.ValidResult;
        }
    }
}
