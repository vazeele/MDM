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
    public partial class Currency : UserControl
    {

        CurrencyModel CurrencyModel;
        CurrencyExchangeRateModel CurrencyExchangeRateModel;        

        public Currency()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Currency.IsSelected)
            {
                CurrencyModel.Save();
            }
            if (tb_CurrencyExchangeRate.IsSelected)
            {
                CurrencyExchangeRateModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Currency.IsSelected)
            {
                CurrencyModel.Add();
            }
            if (tb_CurrencyExchangeRate.IsSelected)
            {
                CurrencyExchangeRateModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            if (tb_Currency.IsSelected)
            {
                CurrencyModel.Delete();
            }
            if (tb_CurrencyExchangeRate.IsSelected)
            {
                CurrencyExchangeRateModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Currency.IsSelected)
            {
                CurrencyModel.CancelUpdate();
            }
            if (tb_CurrencyExchangeRate.IsSelected)
            {
                CurrencyExchangeRateModel.CancelUpdate();
            }
        }        

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Currency.IsSelected)
            {
                if (CurrencyModel == null)
                    CurrencyModel = new CurrencyModel(this);
            }
            if (tb_CurrencyExchangeRate.IsSelected)
            {
                if (CurrencyExchangeRateModel == null)
                    CurrencyExchangeRateModel = new CurrencyExchangeRateModel(this);
            }
        }
       
    }
}