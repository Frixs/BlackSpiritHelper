using BlackSpiritTimerApp.Utilities;
using System;
using System.Windows;

namespace BlackSpiritTimerApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log manager instance.
        /// </summary>
        private Logger logger;

        /// <summary>
        /// Currently active overlay window instance.
        /// </summary>
        private OverlayWindow overlayWindow = null;

        public MainWindow()
        {
            logger = new Logger(this.GetType().ToString());
            InitializeComponent();
        }

        private void ShowOverlayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            overlayWindow = new OverlayWindow();
            overlayWindow.Show();
        }

        private void ShowOverlayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            overlayWindow.Close();
            overlayWindow = null;
            // TODO: GS collect.
        }
    }
}
