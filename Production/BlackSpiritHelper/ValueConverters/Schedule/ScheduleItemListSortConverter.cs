using BlackSpiritHelper.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts item list to sort version of the list.
    /// </summary>
    public class ScheduleItemListSortConverter : BaseValueConverter<ScheduleItemListSortConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.GetType().Equals(typeof(ObservableCollection<ScheduleItemDataViewModel>)))
            {
                var val = (ObservableCollection<ScheduleItemDataViewModel>)value;
                return val.OrderBy(o => o.Name);
            }
            else if (value.GetType().Equals(typeof(List<ScheduleItemDataViewModel>)))
            {
                var val = (List<ScheduleItemDataViewModel>)value;
                return val.OrderBy(o => o.Name);
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
