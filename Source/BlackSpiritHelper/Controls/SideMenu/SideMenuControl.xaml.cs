using BlackSpiritHelper.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

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

        private void AvailableWindowsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items.Count > 0)
                ((SideMenuControlViewModel)DataContext).SelectScreenShareWindowCommand.Execute(items[0]);
        }

        private void AvailableWindowsCombo_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadAvailableWindowsIntoCombo(sender);
        }

        private void AvailableWindowsCombo_DropDownOpened(object sender, System.EventArgs e)
        {
            LoadAvailableWindowsIntoCombo(sender);
        }

        private void LoadAvailableWindowsIntoCombo(object sender)
        {
            var data = new List<Process>();
            data.Add(null);
            data.AddRange(IoC.WInfo.EnumProcessesWithWindows());

            ((ComboBox)sender).ItemsSource = data;
            ((ComboBox)sender).SelectedIndex = 0;
        }
    }
}
