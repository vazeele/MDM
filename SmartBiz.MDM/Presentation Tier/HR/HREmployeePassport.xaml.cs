using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for EMPPASSPORT.xaml
    /// </summary>
    public partial class HREmployeePassport : UserControl
    {
        private HREmpPassportModel hrEmpPassport;
        public HREmployeePassport()
        {
            InitializeComponent();
            hrEmpPassport = new HREmpPassportModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            hrEmpPassport.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            hrEmpPassport.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            hrEmpPassport.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            hrEmpPassport.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            EMPPASSPORTSearchControl.ResetSearchControl();
            hrEmpPassport = new HREmpPassportModel(this);
        }
    }
}