using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Reporting : UserControl
    {
        private ReportingModel ReportingModel;
        public Reporting()
        {
            InitializeComponent();
            ReportingModel = new ReportingModel(this);
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            ReportingModel.Save();
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            ReportingModel.CancelUpdate();
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            ReportingModel.Delete();
        }
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            ReportingModel.Add();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            ReportingSearchControl.ResetSearchControl();
            ReportingModel = new ReportingModel(this);
        }

    }
}