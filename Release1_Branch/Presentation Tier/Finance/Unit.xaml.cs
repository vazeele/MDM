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
    public partial class Unit : UserControl
    {
        UnitDefinitionModel ud;
        public Unit()
        {
            InitializeComponent();  
        }
        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Save();
            }
        }
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Add();
            }
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Delete();
            }
        }
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_UnitDefinition.IsSelected)
            {
                if (ud == null)
                    ud = new UnitDefinitionModel(this);
            }
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.CancelUpdate();
            }
        }
        private void ABSMSearchControl_SearchClick(object sender, RoutedEventArgs e)
        {
            UDSearchControl.ResetPager();
            AgentBrokerSalesManModel.Search(UDSearchControl);
        }
    }
}