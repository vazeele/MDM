using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class HRBenefits : UserControl
    {
        private HRCashBenefitModel hrCashBeenfit;
        private HRNonCashBenefitModel hrNonCashBeenfit;
        private HREmpCashBenefitModel hrEmpCashBeenfit;
        private HREmpNonCashBenefitModel hrEmpNonCashBeenfit;

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

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CashBenefits.IsSelected)
            {
                CashBenefitSearchControl.ResetSearchControl();
                hrCashBeenfit = new HRCashBenefitModel(this);
            }
            else if (tb_NonCashBenefits.IsSelected)
            {
                NonCashBenefitSearchControl.ResetSearchControl();
                hrNonCashBeenfit = new HRNonCashBenefitModel(this);
            }
            else if (tb_EmpCashBenefits.IsSelected)
            {
                EmpCashBenefitSearchControl.ResetSearchControl();
                hrEmpCashBeenfit = new HREmpCashBenefitModel(this);
            }
            else if (Tb_EmpNonCashBenefits.IsSelected)
            {
                EmpNonCashBenefitSearchControl.ResetSearchControl();
                hrEmpNonCashBeenfit = new HREmpNonCashBenefitModel(this);
            }
        }
    }
}