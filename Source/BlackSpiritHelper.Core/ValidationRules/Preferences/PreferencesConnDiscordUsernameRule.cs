using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="ScheduleItemDataViewModel.Name"/>.
    /// </summary>
    public class PreferencesConnDiscordUsernameRule : BaseRule
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
            if (!StringUtils.CheckAlphanumeric(val, true, false, true, "#"))
                return new ValidationResult(false, "Illegal characters. Username can contain only letters, numbers, '-', '_' and '#'.");

            return ValidationResult.ValidResult;
        }
    }
}
