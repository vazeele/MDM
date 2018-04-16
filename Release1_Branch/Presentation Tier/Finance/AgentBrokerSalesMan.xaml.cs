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
    public partial class AgentBrokerSalesMan : UserControl
    {
        AgentBrokerSalesManModel absm;
        public AgentBrokerSalesMan()
        {
            InitializeComponent();  
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AgentBrokerSalesMan.IsSelected)
            {
                absm.Save();
            }
        }
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AgentBrokerSalesMan.IsSelected)
            {
                absm.Add();
            }
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AgentBrokerSalesMan.IsSelected)
            {
                absm.Delete();
            }
        }
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_AgentBrokerSalesMan.IsSelected)
            {
                if (absm == null)
                    absm = new AgentBrokerSalesManModel(this);
            }
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AgentBrokerSalesMan.IsSelected)
            {
                absm.CancelUpdate();
            }
        }
        private void ABSMSearchControl_SearchClick(object sender, RoutedEventArgs e)
        {
            ABSMSearchControl.ResetPager();
            AgentBrokerSalesManModel.Search(ABSMSearchControl);
        }
    }
}