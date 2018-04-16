using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class AgentBrokerSalesMan : UserControl
    {
        private AgentBrokerSalesManModel absm;

        public AgentBrokerSalesMan()
        {
            InitializeComponent();
            absm = new AgentBrokerSalesManModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            absm.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            absm.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            absm.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            absm.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            ABSMSearchControl.ResetSearchControl();
            absm = new AgentBrokerSalesManModel(this);
        }
    }
}