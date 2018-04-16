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
    public partial class Customer : UserControl
    {

       
        LastTransactionInfoModel l;
        public Customer()
        {
            InitializeComponent();
          
        }



        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (tb_PrimaryTransaction.IsSelected)
            {
                
            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
         

        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {

            if (tb_PrimaryTransaction.IsSelected)
            {
               

            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
          
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_PrimaryTransaction.IsSelected)
            {

               
            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
          
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            if (tb_PrimaryTransaction.IsSelected)
            {
               
               
            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
        
        }
        private void btn_SearchPT_Click(object sender, RoutedEventArgs e)
        {

        }
        private  void btn_SearchST_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_Search_LTI_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            if (!(e.Source is TabControl))
            {
                return;
            }

         
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
            else if (tb_SecondaryTransaction.IsSelected)
            {

            }
           
           
        }

        



    }
}