using BlackSpiritHelper.Core;
using System;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Get corresponding margin size for Timer Overlay.
    /// </summary>
    public class BaseOverlaySizeMarginValueConverter : BaseValueConverter<BaseOverlaySizeMarginValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BaseOverlaySize val = (BaseOverlaySize)value;

            switch (val)
            {
                case BaseOverlaySize.Small:
                    return 2;
                case BaseOverlaySize.Normal:
                    return 5;
                case BaseOverlaySize.Large:
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
