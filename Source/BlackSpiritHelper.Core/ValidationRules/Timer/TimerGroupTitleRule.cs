using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerGroupDataViewModel.Title"/>.
    /// </summary>
    public class TimerGroupTitleRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check data type.
            if (val == null)
                return new ValidationResult(false, "Not a character sequence.");

            // Check conditions.
            if (val.Length < TimerGroupDataViewModel.TitleAllowMinChar || val.Length > TimerGroupDataViewModel.TitleAllowMaxChar)
                return new ValidationResult(false, $"Please enter a group title in the length: {TimerGroupDataViewModel.TitleAllowMinChar} - {TimerGroupDataViewModel.TitleAllowMaxChar}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumeric(val, true, true))
                return new ValidationResult(false, "Illegal characters. Group title can contain letters, numbers, underscores and spaces, only.");

            return ValidationResult.ValidResult;
        }
    }
}
