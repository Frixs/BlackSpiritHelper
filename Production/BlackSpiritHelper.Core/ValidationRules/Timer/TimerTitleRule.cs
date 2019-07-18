using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemDataViewModel.Title"/>.
    /// </summary>
    public class TimerTitleRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check data type.
            if (val == null)
                return new ValidationResult(false, "Not a character sequence.");

            val = val.Trim();

            // Check conditions.
            if (val.Length < TimerItemDataViewModel.TitleAllowMinChar || val.Length > TimerItemDataViewModel.TitleAllowMaxChar)
                return new ValidationResult(false, $"Please enter a title in the length: {TimerItemDataViewModel.TitleAllowMinChar} - {TimerItemDataViewModel.TitleAllowMaxChar}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumericString(val, true, true))
                return new ValidationResult(false, "Illegal characters. Title can contain letters, numbers, underscores and spaces, only.");

            return ValidationResult.ValidResult;
        }
    }
}
