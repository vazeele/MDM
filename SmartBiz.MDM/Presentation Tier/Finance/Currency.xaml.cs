using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Currency : UserControl
    {
        private CurrencyModel CurrencyModel;
        private CurrencyExchangeRateModel CurrencyExchangeRateModel;

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

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Currency.IsSelected)
            {
                CurrencySearchControl.ResetSearchControl();
                CurrencyModel = new CurrencyModel(this);
            }
            else if (tb_CurrencyExchangeRate.IsSelected)
            {
                CurrencyExchangeRateSearchControl.ResetSearchControl();
                CurrencyExchangeRateModel = new CurrencyExchangeRateModel(this);
            }
        }
    }
}