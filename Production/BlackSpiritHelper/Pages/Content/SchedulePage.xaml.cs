using BlackSpiritHelper.Core;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : BasePage<SchedulePageViewModel>
    {
        public SchedulePage()
        {
            InitializeComponent();

            // Lazy load task.
            IoC.Task.Run(async () => 
            {
                // Lazy load delay.
                //await Task.Delay((int)(SlideSeconds * 1000 * 3)); // Delay is 3 times the loading duration.
                // TODO lazy load.
                // Update UI thread.
                await Application.Current.Dispatcher.BeginInvoke((Action)(async () =>
                {
                    // Animation.
                    await Calendar.SlideAndFadeInFromBottom(10, 1.2f);
                }));
            });
        }
    }
}
