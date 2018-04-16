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
    public partial class Production : UserControl
    {

        ProcessControlModel ProcessControlModel;
        PeriodModel PeriodModel;

        public Production()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Save();
            }
            if (tb_Period.IsSelected)
            {
               PeriodModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Add();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Delete();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.Delete();
            }

        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.CancelUpdate();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.CancelUpdate();
            }
        }
        
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_ProcessControl.IsSelected)
            {
                if (ProcessControlModel == null)
                    ProcessControlModel = new ProcessControlModel(this);
            }
            if (tb_Period.IsSelected)
            {
                if (PeriodModel == null)
                    PeriodModel = new PeriodModel(this);
            }
        }
       
    }
}