using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemDataViewModel.IconTitleShortcut"/>.
    /// </summary>
    public class TimerIconTitleShortcutRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check data type.
            if (val == null)
                return new ValidationResult(false, "Not a character sequence.");

            // Check conditions.
            if (val.Length < TimerItemDataViewModel.IconTitleAllowMinChar || val.Length > TimerItemDataViewModel.IconTitleAllowMaxChar)
                return new ValidationResult(false, $"Please enter a icon title shortcut in the length: {TimerItemDataViewModel.IconTitleAllowMinChar} - {TimerItemDataViewModel.IconTitleAllowMaxChar}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumeric(val, true, true))
                return new ValidationResult(false, "Illegal characters. Icon title shortcut can contain letters, numbers, underscores and spaces, only.");

            return ValidationResult.ValidResult;
        }
    }
}
