using BlackSpiritHelper.Core;
using System.Diagnostics;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page.
    /// </summary>
    public static class ApplicationPageHelpers
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page.
            switch (page)
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
                    return new TimerGroupSettingsFormPage(viewModel as TimerGroupSettingsFormViewModel);

                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that patches the base page.
            if (page is HomePage)
                return ApplicationPage.Home;

            if (page is TimerPage)
                return ApplicationPage.Timer;

            if (page is WatchdogPage)
                return ApplicationPage.Watchdog;

            if (page is BossPage)
                return ApplicationPage.Boss;

            if (page is TimerGroupSettingsFormPage)
                return ApplicationPage.TimerGroupSettingsForm;

            // Alert developer of issue.
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
