using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for Bank.xaml
    /// </summary>

    public partial class Bank : UserControl
    {
        private BankModel bankmodel;
        private BankBranchModel bankbranchmodel;
        private BankAccountModel bankaccountmodel;

        public Bank()
        {
            InitializeComponent();
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Bank.IsSelected)
            {
                if (bankmodel == null)
                    bankmodel = new BankModel(this);
            }
            else if (tb_BankBranch.IsSelected)
            {
                if (bankbranchmodel == null)
                    bankbranchmodel = new BankBranchModel(this);
            }
            else if (tb_BankAccount.IsSelected)
            {
                if (bankaccountmodel == null)
                    bankaccountmodel = new BankAccountModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Bank.IsSelected)
            {
                bankmodel.CancelUpdate();
            }
            else if (tb_BankBranch.IsSelected)
            {
                bankbranchmodel.CancelUpdate();
            }
            else if (tb_BankAccount.IsSelected)
            {
                bankaccountmodel.CancelUpdate();
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Bank.IsSelected)
            {
                BankSearchControl.ResetSearchControl();
                bankmodel = new BankModel(this);
            }
            else if (tb_BankBranch.IsSelected)
            {
                BankBranchSearchControl.ResetSearchControl();
                bankbranchmodel = new BankBranchModel(this);
            }
            else if (tb_BankAccount.IsSelected)
            {
                BankAccountSearchControl.ResetSearchControl();
                bankaccountmodel = new BankAccountModel(this);
            }
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Bank.IsSelected)
            {
                bankmodel.Save();
            }
            else if (tb_BankBranch.IsSelected)
            {
                bankbranchmodel.Save();
            }
            else if (tb_BankAccount.IsSelected)
            {
                bankaccountmodel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Bank.IsSelected)
            {
                bankmodel.Add();
            }
            else if (tb_BankBranch.IsSelected)
            {
                bankbranchmodel.Add();
            }
            else if (tb_BankAccount.IsSelected)
            {
                bankaccountmodel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Bank.IsSelected)
            {
                bankmodel.Delete();
            }
            else if (tb_BankBranch.IsSelected)
            {
                bankbranchmodel.Delete();
            }
            else if (tb_BankAccount.IsSelected)
            {
                bankaccountmodel.Delete();
            }
        }
    }
}