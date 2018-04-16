using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmartBiz.MDMAPI.Common.Entities;
using Service = SmartBiz.MDM.Presentation.ServiceReference;
using ApiAck = SmartBiz.MDM.Presentation.ServiceReference.ApiAck;
using EApiCallStatus = SmartBiz.MDM.Presentation.ServiceReference.EApiCallStatus;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class CostCenter : UserControl
    {
        CostCenterModel cc;
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
        }
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.Add();
            }
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.Delete();
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
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CostCenter.IsSelected)
            {
                cc.CancelUpdate();
            }
        }
        private void CCSearchControl_SearchClick(object sender, RoutedEventArgs e)
        {
            CCSearchControl.ResetPager();
            AgentBrokerSalesManModel.Search(CCSearchControl);
        }
    }
}