using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SmartBiz.MDM.Presentation.Models
{
    internal class AccountSubTypeSearchModel
    {
        //Normal Search method has been made resuable
        public static async void AccountSubTypeSearch(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AccountSubTypeQuery query = new AccountSubTypeQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AccountSubTypeQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AccountSubTypeQuery() { AccountSubType = int.Parse(SearchControl.SearchTextBox.Text) };
            }

            SearchControl.Search = AccountSubTypeSearch;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAccountSubTypeAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<FIN_AccountSubType>();
            var AccountSubTypes = new ObservableCollection<FIN_AccountSubType>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = AccountSubTypes;
        }
    }
}