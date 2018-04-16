using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SmartBiz.MDM.Presentation.Models
{
    internal class AccountTypeSearchModel
    {
        //Normal Search method has been made resuable
        public static async void AccountTypeSearch(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AccountTypeQuery query = new AccountTypeQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AccountTypeQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            }

            SearchControl.Search = AccountTypeSearch;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAccountTypeAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<FIN_AccountType>();
            var AccountTypes = new ObservableCollection<FIN_AccountType>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = AccountTypes;
        }
    }
}