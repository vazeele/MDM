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
    public partial class TransportDetail : UserControl
    {

        TransportDetailModel TransportDetailModel;

        public TransportDetail()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_TransportDetail.IsSelected)
            {
                TransportDetailModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_TransportDetail.IsSelected)
            {
                TransportDetailModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_TransportDetail.IsSelected)
            {
                TransportDetailModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_TransportDetail.IsSelected)
            {
                TransportDetailModel.CancelUpdate();
            }
        }        

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_TransportDetail.IsSelected)
            {
                if (TransportDetailModel == null)
                    TransportDetailModel = new TransportDetailModel(this);
            }
        }
       
    }
}