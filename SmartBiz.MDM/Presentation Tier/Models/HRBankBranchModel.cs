using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SmartBiz.MDM.Presentation
{
    internal class HRBankModel
    {
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HRBankQuery query = new HRBankQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new HRBankQuery() { Code = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new HRBankQuery() { Name = SearchControl.SearchTextBox.Text };
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryHRBankAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_BANK>(response.Result.ToList<HR_BANK>()); ;
        }
    }
}