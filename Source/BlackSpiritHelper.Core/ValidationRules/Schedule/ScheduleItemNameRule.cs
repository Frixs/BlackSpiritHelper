using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="ScheduleItemDataViewModel.Name"/>.
    /// </summary>
    public class ScheduleItemNameRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check data type.
            if (val == null)
                return new ValidationResult(false, "Not a character sequence.");

            // Check conditions.
            if (val.Length < ScheduleViewModel.AllowedItemMinLength || val.Length > ScheduleViewModel.AllowedItemMaxLength)
                return new ValidationResult(false, $"Please enter a name in the length: {ScheduleViewModel.AllowedItemMinLength} - {ScheduleViewModel.AllowedItemMaxLength}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumeric(val, false, false, false))
                return new ValidationResult(false, "Illegal characters. Name can contain letters only.");

            return ValidationResult.ValidResult;
        }
    }
}
