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
    public partial class Warning : UserControl
    {

        WarningModel WarningModel;

        public Warning()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Warning.IsSelected)
            {
                WarningModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Warning.IsSelected)
            {
                WarningModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Warning.IsSelected)
            {
                WarningModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Warning.IsSelected)
            {
                WarningModel.CancelUpdate();
            }
        }
        

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Warning.IsSelected)
            {
                if (WarningModel == null)
                    WarningModel = new WarningModel(this);
            }
        }
       
    }
}