using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.IconTitleShortcut"/>.
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
            if (val.Length < TimerItemViewModel.IconTitleAllowMinChar || val.Length > TimerItemViewModel.IconTitleAllowMaxChar)
                return new ValidationResult(false, $"Please enter a icon title shortcut in the length: {TimerItemViewModel.IconTitleAllowMinChar} - {TimerItemViewModel.IconTitleAllowMaxChar}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumericString(val, true, true))
                return new ValidationResult(false, "Illegal characters. Icon title shortcut can contain letters, numbers, underscores and spaces, only.");

            return ValidationResult.ValidResult;
        }
    }
}
