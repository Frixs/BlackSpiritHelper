using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.IconTitleShortcut"/>.
    /// </summary>
    public class TimerIconTitleShortcutRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val;

            // Get correct type.
            if (value.GetType() == typeof(string))
                val = (string)value;
            else
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
