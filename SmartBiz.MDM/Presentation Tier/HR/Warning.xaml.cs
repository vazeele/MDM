using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Warning : UserControl
    {
        private WarningModel WarningModel;

        public Warning()
        {
            InitializeComponent();
            WarningModel = new WarningModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            WarningModel.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            WarningModel.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            WarningModel.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            WarningModel.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            WarningSearchControl.ResetSearchControl();
            WarningModel = new WarningModel(this);
        }
    }
}