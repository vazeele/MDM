using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Customer : UserControl
    {
        private CustomerSupplierModel cusSupModel;
        private CustomerSupplierBankModel cusSupBank;

        public Customer()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CusSupInfo.IsSelected)
            {
                cusSupModel.Save();
            }
            else if (tb_CusSupBank.IsSelected)
            {
                cusSupBank.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CusSupInfo.IsSelected)
            {
                cusSupModel.Add();
            }
            else if (tb_CusSupBank.IsSelected)
            {
                cusSupBank.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CusSupInfo.IsSelected)
            {
                cusSupModel.Delete();
            }
            else if (tb_CusSupBank.IsSelected)
            {
                cusSupBank.Delete();
            }
        }
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }
            else if (tb_CusSupInfo.IsSelected)
            {
                if (cusSupModel == null)
                    cusSupModel = new CustomerSupplierModel(this);
            }
            else if (tb_CusSupBank.IsSelected)
            {
                if (cusSupBank == null)
                    cusSupBank = new CustomerSupplierBankModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CusSupInfo.IsSelected)
            {
                cusSupModel.CancelUpdate();
            }
            else if (tb_CusSupBank.IsSelected)
            {
                cusSupBank.CancelUpdate();
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_CusSupInfo.IsSelected)
            {
                CusSupInfoSearchControl.ResetSearchControl();
                cusSupModel = new CustomerSupplierModel(this);
            }
            else if (tb_CusSupBank.IsSelected)
            {
                CusSupBankSearchControl.ResetSearchControl();
                cusSupBank = new CustomerSupplierBankModel(this);
            }
        }
    }
}