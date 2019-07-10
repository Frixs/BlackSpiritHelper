using BlackSpiritHelper.Core;
using System;
using System.Diagnostics;
using System.Globalization;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page.
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page.
            switch ((ApplicationPage) value)
            {
                case ApplicationPage.Home:
                    return new HomePage();

                case ApplicationPage.Timer:
                    return new TimerPage();

                case ApplicationPage.Watchdog:
                    return new WatchdogPage();

                case ApplicationPage.Boss:
                    return new BossPage();

                case ApplicationPage.TimerGroupSettingsForm:
                    return new TimerGroupSettingsFormPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
