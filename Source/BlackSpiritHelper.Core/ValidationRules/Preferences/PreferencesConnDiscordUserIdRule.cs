using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="ScheduleItemDataViewModel.Name"/>.
    /// </summary>
    public class PreferencesConnDiscordUserIdRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = GetBoundValue(value) as string;

            // Check null.
            if (string.IsNullOrEmpty(val))
                return new ValidationResult(false, "Not a character sequence.");

            // Check conditions.
            if (val.Length > PreferencesConnDiscordDataViewModel.AllowedUsernameMaxLength)
                return new ValidationResult(false, $"Please enter a name in the maximum length of {PreferencesConnDiscordDataViewModel.AllowedUsernameMaxLength} characters.");

            // Check allowed characters.
            if (!StringUtils.CheckNumeric(val))
                return new ValidationResult(false, "Illegal characters. User ID can contain only digits.");

            return ValidationResult.ValidResult;
        }
    }
}
