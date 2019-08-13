using System.Windows;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        #region Static Members

        /// <summary>
        /// Active progess window instance.
        /// </summary>
        public static ProgressWindow Window = null;

        #endregion

        #region Constructor

        public ProgressWindow()
        {
            InitializeComponent();

            DataContext = new ProgressWindowViewModel(this);
        } 

        #endregion
    }
}
