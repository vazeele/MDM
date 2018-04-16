using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for HelpControl.xaml
    /// </summary>
    public partial class HelpControl : UserControl
    {
        public HelpControl()
        {
            InitializeComponent();
        }

        private void BtnUserManual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"..\..\Documentation\User Guide.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("User Guide not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAPI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"..\..\Documentation\Documentation.chm");
            }
            catch (Exception ex)
            {
                MessageBox.Show("API Reference not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}