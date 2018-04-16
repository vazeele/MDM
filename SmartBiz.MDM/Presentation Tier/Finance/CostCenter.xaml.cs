using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class CostCenter : UserControl
    {
        private CostCenterModel cc;
        private CostCenterwiseConfigurationModel ccwc;
        public CostCenter()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.Save();
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                ccwc.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.Add();
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                ccwc.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.Delete();
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                ccwc.Delete();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_CostCenter.IsSelected)
            {
                if (cc == null)
                    cc = new CostCenterModel(this);
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                if (ccwc == null)
                    ccwc = new CostCenterwiseConfigurationModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.CancelUpdate();
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                ccwc.CancelUpdate();
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                CCSearchControl.ResetSearchControl();
                cc = new CostCenterModel(this);
            }
            else if (tb_CostCenterwiseConfiguration.IsSelected)
            {
                CCwCSearchControl.ResetSearchControl();
                ccwc = new CostCenterwiseConfigurationModel(this);
            }
        }
    }
}