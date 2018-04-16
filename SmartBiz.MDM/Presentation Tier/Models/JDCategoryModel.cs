using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SmartBiz.MDM.Presentation
{
    internal class JDCategoryModel
    {
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            JDCategoryQuery query = new JDCategoryQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new JDCategoryQuery() { Code = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new JDCategoryQuery() { Name = SearchControl.SearchTextBox.Text };
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryJDCategoryAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_JD_CATEGORY>(response.Result.ToList<HR_JD_CATEGORY>()); ;
        }
    }
}