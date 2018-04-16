﻿using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SmartBiz.MDM.Presentation.Models
{
    internal class ReligionModel
    {
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            ReligionQuery query = new ReligionQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new ReligionQuery() { Code = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new ReligionQuery() { Name = SearchControl.SearchTextBox.Text };
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryReligionAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_RELIGION>(response.Result.ToList<HR_RELIGION>()); ;
        }
    }
}