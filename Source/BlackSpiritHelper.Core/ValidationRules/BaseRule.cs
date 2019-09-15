using System.Windows.Controls;
using System.Windows.Data;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Rule for the property <see cref="TimerItemDataViewModel.Title"/>.
    /// </summary>
    public abstract class BaseRule : ValidationRule
    {
        /// <summary>
        /// Extract value from the rule handler.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                // ValidationStep was UpdatedValue or CommittedValue (Validate after setting).
                // Need to pull the value out of the BindingExpression..
                BindingExpression binding = (BindingExpression)value;

                // Get the bound object and name of the property.
                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;

                // Extract the value of the property.
                object propertyValue = dataItem.GetType().GetProperty(propertyName).GetValue(dataItem, null);

                // This is what we want.
                return propertyValue;
            }
            else
            {
                // ValidationStep was RawProposedValue or ConvertedProposedValue.
                // The argument is already what we want!
                return value;
            }
        }
    }
}
