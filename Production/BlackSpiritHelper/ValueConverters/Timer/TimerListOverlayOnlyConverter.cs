using BlackSpiritHelper.Core;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Selects only appropriate timers from <see cref="TimerGroupDataViewModel.TimerList"/> to show in Overlay.
    /// </summary>
    public class TimerListOverlayOnlyConverter : BaseValueConverter<TimerListOverlayOnlyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (
                !value.GetType().Equals(typeof(ObservableCollection<TimerItemDataViewModel>))
                )
            {
                return null;
            }

            return ((ObservableCollection<TimerItemDataViewModel>)value).Where(o => o.ShowInOverlay == true);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
