using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule to check <see cref="TimeZoneRegion"/> only valid values of <see cref="ScheduleTemplateDataViewModel.TimeZoneRegion"/>.
    /// </summary>
    public class ScheduleTimeZoneRegionRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimeZoneRegion val;
            object oVal = GetBoundValue(value);

            // Check data type.
            if (oVal.GetType() == typeof(TimeZoneRegion))
                val = (TimeZoneRegion)oVal;
            else
                return new ValidationResult(false, "Not a TimeZoneRegion.");

            // Check conditions.
            if (val == TimeZoneRegion.None)
                return new ValidationResult(false, $"Not a valid value.");
            // While changing rules, do not forget to change list in ScheduleTemplateSettingsFormPage.

            return ValidationResult.ValidResult;
        }
    }

}
