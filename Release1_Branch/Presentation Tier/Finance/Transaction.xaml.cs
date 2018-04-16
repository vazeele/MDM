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
    public partial class Transaction : UserControl
    {

      
        LastTransactionInfoModel l;
        TransactionReferenceModel tr;
        ControlledTransactionModel ct;
        FixedTxnAttributesModel fta;
        public Transaction()
        {
            InitializeComponent();
          
        }



        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (tb_LastTransactionInfo.IsSelected)
            {
                l.Save();
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                fta.Save();
            }
            else if (tb_TransactionReference.IsSelected)
            {
                tr.Save();
            }
            else if(tb_ControlledTransaction.IsSelected)
            {
                ct.Save();
            }


        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {

            if (tb_LastTransactionInfo.IsSelected)
            {
                l.Add();
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                fta.Add();
            }
            else if (tb_TransactionReference.IsSelected)
            {
                tr.Add();
            }
            else if (tb_ControlledTransaction.IsSelected)
            {
                ct.Add();
            }
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_LastTransactionInfo.IsSelected)
            {
                l.CancelUpdate();
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                fta.CancelUpdate();
            }
            else if (tb_TransactionReference.IsSelected)
            {
                tr.CancelUpdate();
            }
            else if (tb_ControlledTransaction.IsSelected)
            {
                ct.CancelUpdate();
            }
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
          if (tb_LastTransactionInfo.IsSelected)
            {
                l.Delete();
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                fta.Delete();
            }
            else if (tb_TransactionReference.IsSelected)
            {
                tr.Delete();
            }
          else if (tb_ControlledTransaction.IsSelected)
          {
              ct.Delete();
          }
        }
     
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_LastTransactionInfo.IsSelected)
            {
                if (l == null)
                    l = new LastTransactionInfoModel(this);
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                if (fta == null)
                    fta = new FixedTxnAttributesModel(this);
            }
            else if (tb_TransactionReference.IsSelected)
            {
                if (tr == null)
                    tr = new TransactionReferenceModel (this);
            }
            else if (tb_ControlledTransaction.IsSelected)
            {
                if (ct == null)
                    ct = new ControlledTransactionModel(this);
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_LastTransactionInfo.IsSelected)
            {
              LTISearchControl.ResetSearchControl();
                l = new LastTransactionInfoModel(this);
            }
            else if (tb_FixedTransactionAttrib.IsSelected)
            {
                FTASearchControl.ResetSearchControl();
                fta = new FixedTxnAttributesModel(this);
            }
            else if (tb_TransactionReference.IsSelected)
            {
                TRSearchControl.ResetSearchControl();
                tr = new TransactionReferenceModel(this);
            }
            else if (tb_ControlledTransaction.IsSelected)
            {
                CTSearchControl.ResetSearchControl();
                ct = new ControlledTransactionModel(this);
            }
        }

      


    }
}