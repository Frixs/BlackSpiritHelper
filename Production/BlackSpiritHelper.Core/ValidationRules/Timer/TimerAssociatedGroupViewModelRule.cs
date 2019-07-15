using System.Globalization;
using System.Windows.Controls;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule to check free space in group... to be able to change timer's group - <see cref="TimerItemViewModel.GroupID"/>.
    /// </summary>
    public class TimerAssociatedGroupViewModelRule : BaseRule
    {
        private sbyte mCurrentGroupID = -1;

        public sbyte CurrentGroupID
        {
            get => mCurrentGroupID;
            set => mCurrentGroupID = value;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimerGroupViewModel val;
            object oVal = GetBoundValue(value);

            // Check data type.
            if (oVal.GetType() == typeof(TimerGroupViewModel))
                val = (TimerGroupViewModel)oVal;
            else
                return new ValidationResult(false, "Not a timer group model.");

            // Check conditions.
            if (!val.CanCreateNewTimer && val.ID != CurrentGroupID)
                return new ValidationResult(false, $"The group is already full. Maximal number of timers in one group is {TimerGroupViewModel.AllowedMaxNoOfTimers}.");

            return ValidationResult.ValidResult;
        }
    }

}
