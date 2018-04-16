using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                MessageBox.Show("API Reference not found","Not found",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }

    }
}
