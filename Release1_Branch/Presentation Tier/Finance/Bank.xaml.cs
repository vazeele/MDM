using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for Bank.xaml
    /// </summary>
    
   
    public partial class Bank : UserControl
    {
        BankModel bankmodel;
        BankBranchModel bankbranchmodel;
        BankAccountModel bankaccountmodel;
        public Bank()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
    }
}
