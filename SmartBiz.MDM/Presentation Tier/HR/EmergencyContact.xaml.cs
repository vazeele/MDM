using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class EmergencyContact : UserControl
    {
        private EmergencyContactModel EmergencyContactModel;

        public EmergencyContact()
        {
            InitializeComponent();
            EmergencyContactModel = new EmergencyContactModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactModel.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactModel.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactModel.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactModel.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactSearchControl.ResetSearchControl();
            EmergencyContactModel = new EmergencyContactModel(this);
        }
    }
}