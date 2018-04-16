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
    public partial class HRBenefits : UserControl
    {

        HRCashBenefitModel hrCashBeenfit;
        HRNonCashBenefitModel hrNonCashBeenfit;
        HREmpCashBenefitModel hrEmpCashBeenfit;
        HREmpNonCashBenefitModel hrEmpNonCashBeenfit;

        public HRBenefits()
        {
            InitializeComponent();
          
        }



        private void BtSave_Click(object sender, RoutedEventArgs e)
        {

            if (tb_CashBenefits.IsSelected)
            {
                hrCashBeenfit.Save();
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                hrNonCashBeenfit.Save();
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                hrEmpCashBeenfit.Save();
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                hrEmpNonCashBeenfit.Save();
            }
            


        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {

            if (tb_CashBenefits.IsSelected)
            {
                hrCashBeenfit.Add();
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                hrNonCashBeenfit.Add();
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                hrEmpCashBeenfit.Add();
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                hrEmpNonCashBeenfit.Add();
            }
            
        }
        private void BtnUpdateComplete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CashBenefits.IsSelected)
            {
                hrCashBeenfit.CompleteUpdate();
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                hrNonCashBeenfit.CompleteUpdate();
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                hrEmpCashBeenfit.CompleteUpdate();
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                hrEmpNonCashBeenfit.CompleteUpdate();

            }
            
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            if (tb_CashBenefits.IsSelected)
            {
                hrCashBeenfit.Delete();
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                hrNonCashBeenfit.Delete();
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                hrEmpCashBeenfit.Delete();
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                hrEmpNonCashBeenfit.Delete();
            }
            
        }
       
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_CashBenefits.IsSelected)
            {
                if (hrCashBeenfit == null)
                    hrCashBeenfit = new HRCashBenefitModel(this);
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                if (hrNonCashBeenfit == null)
                    hrNonCashBeenfit = new HRNonCashBenefitModel(this);
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                if (hrEmpCashBeenfit == null)
                    hrEmpCashBeenfit = new HREmpCashBenefitModel(this);
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                if (hrEmpNonCashBeenfit == null)
                    hrEmpNonCashBeenfit = new HREmpNonCashBenefitModel(this);
            }
           
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CashBenefits.IsSelected)
            {
                hrCashBeenfit.CancelUpdate();
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                hrNonCashBeenfit.CancelUpdate();
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                hrEmpCashBeenfit.CancelUpdate();
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                hrEmpNonCashBeenfit.CancelUpdate();
            }
            
        }

        



    }
}