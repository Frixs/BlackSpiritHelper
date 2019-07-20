﻿using BlackSpiritHelper.Core;
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

                case ApplicationPage.DailyCheck:
                    return new DailyCheckPage();

                case ApplicationPage.Preferences:
                    return new PreferencesPage();

                case ApplicationPage.TimerGroupSettingsForm:
                    return new TimerGroupSettingsFormPage(viewModel as TimerGroupSettingsFormPageViewModel);

                case ApplicationPage.TimerItemSettingsForm:
                    return new TimerItemSettingsFormPage(viewModel as TimerItemSettingsFormPageViewModel);

                default:
                    // Log it.
                    IoC.Logger.Log("A selected application page value is out of box!", LogLevel.Error);
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

            if (page is DailyCheckPage)
                return ApplicationPage.DailyCheck;

            if (page is PreferencesPage)
                return ApplicationPage.Preferences;

            if (page is TimerGroupSettingsFormPage)
                return ApplicationPage.TimerGroupSettingsForm;

            if (page is TimerItemSettingsFormPage)
                return ApplicationPage.TimerItemSettingsForm;

            // Log it.
            IoC.Logger.Log("A selected base page value is out of box!", LogLevel.Error);
            // Alert developer of issue.
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
