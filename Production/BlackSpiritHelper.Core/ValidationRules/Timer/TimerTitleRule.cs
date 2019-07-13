using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.Title"/>.
    /// </summary>
    public class TimerTitleRule : ValidationRule
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
            if (val.Length < TimerItemViewModel.TitleAllowMinChar || val.Length > TimerItemViewModel.TitleAllowMaxChar)
                return new ValidationResult(false, $"Please enter a title in the length: {TimerItemViewModel.TitleAllowMinChar} - {TimerItemViewModel.TitleAllowMaxChar}.");

            // Check allowed characters.
            if (!StringUtils.CheckAlphanumericString(val, true, true))
                return new ValidationResult(false, "Illegal characters. Title can contain letters, numbers, underscores and spaces, only.");

            return ValidationResult.ValidResult;
        }
    }
}
