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
            OverlaySize val;

            if (value.GetType() == typeof(OverlaySize))
                val = (OverlaySize)value;
            else
            {
                IoC.Logger.Log("The target must be a OverlaySize!", LogLevel.Fatal);
                throw new InvalidOperationException("The target must be a OverlaySize!");
            }

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
