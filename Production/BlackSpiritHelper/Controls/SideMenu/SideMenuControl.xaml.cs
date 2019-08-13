using BlackSpiritHelper.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace BlackSpiritHelper
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        #region Constructor

        public SideMenuControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void ShowOverlayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IoC.UI.OpenOverlay();
        }

        private void ShowOverlayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IoC.UI.CloseOverlay();
        }

        #endregion
    }
}
