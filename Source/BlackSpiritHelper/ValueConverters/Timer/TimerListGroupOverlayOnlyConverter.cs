using BlackSpiritHelper.Core;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Selects only appropriate groups <see cref="TimerGroupDataViewModel.IgnoreInOverlay"/> == false
    /// </summary>
    public class TimerListGroupOverlayOnlyConverter : BaseValueConverter<TimerListGroupOverlayOnlyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (
                !value.GetType().Equals(typeof(ObservableCollection<TimerGroupDataViewModel>))
                )
            {
                return null;
            }

            return ((ObservableCollection<TimerGroupDataViewModel>)value).Where(o => o.IgnoreInOverlay == false);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
