using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class JobSpecification : UserControl
    {
        private JobSpecificationModel JobSpecificationModel;
        public JobSpecification()
        {
            InitializeComponent();
            JobSpecificationModel = new JobSpecificationModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            JobSpecificationModel.Save();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            JobSpecificationModel.CancelUpdate();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            JobSpecificationModel.Delete();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            JobSpecificationModel.Add();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            JobSpecificationSearchControl.ResetSearchControl();
            JobSpecificationModel = new JobSpecificationModel(this);
        }
    }
}