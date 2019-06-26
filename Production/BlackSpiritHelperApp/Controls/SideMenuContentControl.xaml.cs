using BlackSpiritHelper.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
