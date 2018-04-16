using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReportViewer : UserControl
    {
        public ReportViewer()
        {
            InitializeComponent();
         
            reportViewer.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl = new Uri(@"http://haritha-pc/ReportServer_SQLEXPRESS");
            RefreshReports();
        }

        private void RefreshReports()
        {
            var service = Helper.getServiceClient();
            var list = service.GetReportNames();
            listbox.Items.Clear();
            SearchBox.Clear();
            foreach (string s in list)
            {
                listbox.Items.Add(s);
            }

            CountLabel.Content = listbox.Items.Count.ToString() + " Items";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            listbox.Items.Filter = s => s.ToString().ContainsIgnoreCase(SearchBox.Text);
            CountLabel.Content = listbox.Items.Count.ToString()+ " Items";
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                reportViewer.ServerReport.ReportPath = "/" + listbox.SelectedItem.ToString();
                reportViewer.RefreshReport();
            }
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshReports();
        }
      
    }
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
