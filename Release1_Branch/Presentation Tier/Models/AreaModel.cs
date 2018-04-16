using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApiAck = SmartBiz.MDM.Presentation.ServiceReference.ApiAck;
using EApiCallStatus = SmartBiz.MDM.Presentation.ServiceReference.EApiCallStatus;
using Service = SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDMAPI.Common.Entities;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Specialized;
using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.ServiceReference;
namespace SmartBiz.MDM.Presentation

{
    class AreaModel
    {
       
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AreaQuery query = new AreaQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new Service.AreaQuery() { AreaCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new Service.AreaQuery() { AreaName = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new Service.AreaQuery() { RegionCode = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAreaAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_Area>(response.Result.ToList<FIN_Area>()); ;         
   
        }
        
    
    
    }
}