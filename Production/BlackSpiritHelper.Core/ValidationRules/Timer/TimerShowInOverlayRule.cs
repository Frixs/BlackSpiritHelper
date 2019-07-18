using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemViewModel.ShowInOverlay"/>.
    /// </summary>
    public class TimerShowInOverlayRule : BaseRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool val;
            object oVal = GetBoundValue(value);

            // Check data type.
            if (oVal.GetType() == typeof(bool))
                val = (bool)oVal;
            else
                return new ValidationResult(false, "Not a boolean.");

            // Check conditions.
            if (val)
            {
                // Counter.
                int c = 1;
                // Chech conditions.
                foreach (TimerGroupViewModel g in IoC.DataContent.TimerDesignModel.GroupList)
                    foreach (TimerItemViewModel t in g.TimerList)
                        if (t.ShowInOverlay)
                            if (++c > TimerItemViewModel.OverlayTimerLimitCount)
                                return new ValidationResult(false, $"Overlay is already full. Only {TimerItemViewModel.OverlayTimerLimitCount} timers can be visible in overlay.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
