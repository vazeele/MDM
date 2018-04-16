using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class TransportDetail : UserControl
    {
        private TransportDetailModel TransportDetailModel;

        public TransportDetail()
        {
            InitializeComponent();
            TransportDetailModel = new TransportDetailModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            TransportDetailModel.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            TransportDetailModel.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            TransportDetailModel.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            TransportDetailModel.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            TransportDetailSearchControl.ResetSearchControl();
            TransportDetailModel = new TransportDetailModel(this);
        }
    }
}