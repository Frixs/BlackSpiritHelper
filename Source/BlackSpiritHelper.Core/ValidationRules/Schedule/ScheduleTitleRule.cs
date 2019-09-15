using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="ScheduleTemplateDataViewModel.Title"/>.
    /// </summary>
    public class ScheduleTitleRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check data type.
            if (val == null)
                return new ValidationResult(false, "Not a character sequence.");

            val = val.Trim();

            // Check conditions.
            if (val.Length < ScheduleTemplateDataViewModel.AllowedTitleMinLength || val.Length > ScheduleTemplateDataViewModel.AllowedTitleMaxLength)
                return new ValidationResult(false, $"Please enter a title in the length: {ScheduleTemplateDataViewModel.AllowedTitleMinLength} - {ScheduleTemplateDataViewModel.AllowedTitleMaxLength}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumeric(val, false, false, true))
                return new ValidationResult(false, "Illegal characters. Title can contain letters, numbers and dashes, only.");

            return ValidationResult.ValidResult;
        }
    }
}
