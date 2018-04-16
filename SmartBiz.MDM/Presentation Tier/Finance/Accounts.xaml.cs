using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Accounts : UserControl
    {
        private AccountTypeModel at;
        private AccountSubTypeModel ast;
        private AccountSubTypeCategoryModel astc;
        private ProfitLossModel plt;
        private SpecialAccountTypeModel sat;

        public Accounts()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AccountType.IsSelected)
            {
                at.Save();
            }
            else if (tb_AccountSubType.IsSelected)
            {
                ast.Save();
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                astc.Save();
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                plt.Save();
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                sat.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AccountType.IsSelected)
            {
                at.Add();
            }
            else if (tb_AccountSubType.IsSelected)
            {
                ast.Add();
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                astc.Add();
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                plt.Add();
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                sat.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AccountType.IsSelected)
            {
                at.Delete();
            }
            else if (tb_AccountSubType.IsSelected)
            {
                ast.Delete();
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                astc.Delete();
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                plt.Delete();
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                sat.Delete();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_AccountType.IsSelected)
            {
                if (at == null)
                    at = new AccountTypeModel(this);
            }
            else if (tb_AccountSubType.IsSelected)
            {
                if (ast == null)
                    ast = new AccountSubTypeModel(this);
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                if (astc == null)
                    astc = new AccountSubTypeCategoryModel(this);
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                if (plt == null)
                    plt = new ProfitLossModel(this);
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                if (sat == null)
                    sat = new SpecialAccountTypeModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AccountType.IsSelected)
            {
                at.CancelUpdate();
            }
            else if (tb_AccountSubType.IsSelected)
            {
                ast.CancelUpdate();
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                astc.CancelUpdate();
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                plt.CancelUpdate();
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                sat.CancelUpdate();
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_AccountType.IsSelected)
            {
                AccountTypeSearchControl.ResetSearchControl();
                at = new AccountTypeModel(this);
            }
            else if (tb_AccountSubType.IsSelected)
            {
                AccountSubTypeSearchControl.ResetSearchControl();
                ast = new AccountSubTypeModel(this);
            }
            else if (tb_AccountSubTypeCategory.IsSelected)
            {
                AccountSubTypeCatSearchControl.ResetSearchControl();
                astc = new AccountSubTypeCategoryModel(this);
            }
            else if (Tb_ProfitLostType.IsSelected)
            {
                ProfitLossSearchControl.ResetSearchControl();
                plt = new ProfitLossModel(this);
            }
            else if (Tb_SpecialAccountType.IsSelected)
            {
                SpecialAccountTypeSearchControl.ResetSearchControl();
                sat = new SpecialAccountTypeModel(this);
            }
        }
    }
}