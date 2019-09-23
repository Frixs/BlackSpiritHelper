using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Get corresponding margin size for Timer Overlay.
    /// </summary>
    public class OverlaySizeMarginValueConverter : BaseValueConverter<OverlaySizeMarginValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OverlaySize val = (OverlaySize)value;

            switch (val)
            {
                case OverlaySize.Small:
                    return 2;
                case OverlaySize.Normal:
                    return 5;
                case OverlaySize.Large:
                    return 10;
                default:
                    return 0;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
