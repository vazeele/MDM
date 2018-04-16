using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class BasicSalary : UserControl
    {
        private BasicSalaryModel BasicSalaryModel;

        public BasicSalary()
        {
            InitializeComponent();
            BasicSalaryModel = new BasicSalaryModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            BasicSalaryModel.Save();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            BasicSalaryModel.CancelUpdate();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            BasicSalaryModel.Delete();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            BasicSalaryModel.Add();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            BasicSalarySearchControl.ResetSearchControl();
            BasicSalaryModel = new BasicSalaryModel(this);
        }
    }
}