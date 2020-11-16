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

                case ApplicationPage.Schedule:
                    return new SchedulePage();

                case ApplicationPage.ApmCalculator:
                    return new ApmCalculatorPage();

                case ApplicationPage.Preferences:
                    return new PreferencesPage();

                case ApplicationPage.TimerGroupSettingsForm:
                    return new TimerGroupSettingsFormPage(viewModel as TimerGroupSettingsFormPageViewModel);

                case ApplicationPage.TimerItemSettingsForm:
                    return new TimerItemSettingsFormPage(viewModel as TimerItemSettingsFormPageViewModel);

                case ApplicationPage.ScheduleTemplateSettingsForm:
                    return new ScheduleTemplateSettingsFormPage(viewModel as ScheduleTemplateSettingsFormPageViewModel);

                case ApplicationPage.ScheduleItemControlForm:
                    return new ScheduleItemControlFormPage(viewModel as ScheduleItemControlFormPageViewModel);

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

            if (page is SchedulePage)
                return ApplicationPage.Schedule;

            if (page is ApmCalculatorPage)
                return ApplicationPage.ApmCalculator;

            if (page is PreferencesPage)
                return ApplicationPage.Preferences;

            if (page is TimerGroupSettingsFormPage)
                return ApplicationPage.TimerGroupSettingsForm;

            if (page is TimerItemSettingsFormPage)
                return ApplicationPage.TimerItemSettingsForm;

            if (page is ScheduleTemplateSettingsFormPage)
                return ApplicationPage.ScheduleTemplateSettingsForm;

            if (page is ScheduleItemControlFormPage)
                return ApplicationPage.ScheduleItemControlForm;

            // Log it.
            IoC.Logger.Log("A selected base page value is out of box!", LogLevel.Error);
            // Alert developer of issue.
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
