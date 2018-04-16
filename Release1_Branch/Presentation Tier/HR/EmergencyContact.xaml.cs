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
    public partial class EmergencyContact : UserControl
    {

        EmergencyContactModel EmergencyContactModel;

        public EmergencyContact()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {            
            if (tb_EmergencyContact.IsSelected)
            {
                EmergencyContactModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_EmergencyContact.IsSelected)
            {
                EmergencyContactModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_EmergencyContact.IsSelected)
            {
                EmergencyContactModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_EmergencyContact.IsSelected)
            {
                EmergencyContactModel.CancelUpdate();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_EmergencyContact.IsSelected)
            {
                if (EmergencyContactModel == null)
                    EmergencyContactModel = new EmergencyContactModel(this);
            }
        }
              
    }
}