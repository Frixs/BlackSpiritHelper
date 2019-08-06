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
                await Task.Delay((int)(SlideSeconds * 1000));
                
                // Update UI thread.
                await Application.Current.Dispatcher.BeginInvoke((Action)(async () =>
                {
                    Calendar.Visibility = Visibility.Hidden;
                    await Task.Delay(500); // Delay some time to load (heavy) calendar in invisibility.

                    // Animation.
                    await Calendar.SlideAndFadeInFromBottom(8, 0.6f);
                    await Settings.SlideAndFadeInFromBottom(8, 0.6f);
                }));
            });
        }
    }
}
