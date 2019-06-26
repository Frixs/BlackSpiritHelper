using BlackSpiritHelper.Core;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SideMenuContentControl.xaml
    /// </summary>
    public partial class SideMenuContentControl : UserControl
    {
        #region Commands

        /// <summary>
        /// The command to open the content page.
        /// </summary>
        public ICommand OpenPage0 { get; set; }

        /// <summary>
        /// The command to open the content page.
        /// </summary>
        public ICommand OpenPage1 { get; set; }

        /// <summary>
        /// The command to open the content page.
        /// </summary>
        public ICommand OpenPage2 { get; set; }

        /// <summary>
        /// The command to open the content page.
        /// </summary>
        public ICommand OpenPage3 { get; set; }

        #endregion

        #region Constructor

        public SideMenuContentControl()
        {
            InitializeComponent();

            CreateCommands();
        }

        #endregion

        /// <summary>
        /// Create Windows commands.
        /// </summary>
        private void CreateCommands()
        {
            OpenPage0 = new RelayCommand(() =>
            {
                // TODO;
            });
        }
    }
}
