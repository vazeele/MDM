using System.Linq;
using SmartBiz.MDMAPI.Common.Entities;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDM.Presentation.CustomControls;
using System.Windows;
using System.Collections.ObjectModel;
namespace SmartBiz.MDM.Presentation.Models
{
    class GeneralLedgerAccountModel
    {
        //Normal Search method has been made resuable
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            GeneralLedgerAccountQuery query = new GeneralLedgerAccountQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new GeneralLedgerAccountQuery() { AccountNo = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new GeneralLedgerAccountQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new GeneralLedgerAccountQuery() { AccountSubtype = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryGeneralLedgerAccountAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<FIN_GeneralLedgerAccount>();
            var GeneralLedgerAccounts = new ObservableCollection<FIN_GeneralLedgerAccount>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = GeneralLedgerAccounts;
        }
    }
}
