using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for EMPBANK.xaml
    /// </summary>
    public partial class HREmployeeBank : UserControl
    {
        private HREmpBankModel hrEmpModel;
        public HREmployeeBank()
        {
            InitializeComponent();
            hrEmpModel = new HREmpBankModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            hrEmpModel.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            hrEmpModel.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            hrEmpModel.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            hrEmpModel.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            EMPBANKSearchControl.ResetSearchControl();
            hrEmpModel = new HREmpBankModel(this);
        }
    }
}