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
    internal class CurrencyModel
    {
        private ObservableCollection<FIN_Currency> CurrencyInformation; //These will be shown in the datagrid
        private Currency Currency; //Reference to Transaction Interface
        private FIN_Currency EditingCurrencyInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public CurrencyModel(Currency Currency)
        {
            this.Currency = Currency;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Currency.CurrencySearchControl.Search = Search;
            Currency.CurrencySearchControl.ResultsGrid.SelectedCellsChanged += dgv_Currency_SelectionChanged;
            Currency.CurrencyGrid.SourceUpdated += CurrencyGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(Currency.CurrencySearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(Currency.CurrencySearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            CurrencyInformation = Currency.CurrencySearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_Currency>;
            Currency.CurrencyGrid.DataContext = CurrencyInformation;
            if (CurrencyInformation[0] != null)
            {
                EditingCurrencyInfomation = CurrencyInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Currency_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Currency.CurrencySearchControl.ResultsGrid.SelectedItem != null)
                EditingCurrencyInfomation = (Currency.CurrencySearchControl.ResultsGrid.SelectedItem as FIN_Currency).Clone() as FIN_Currency;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void CurrencyGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Currency.CurrencyGrid.DataContext == CurrencyInformation)
            {
                Currency.BtnSave.Visibility = Visibility.Visible;
                Currency.BtnCancelUpdate.Visibility = Visibility.Visible;
                Currency.CurrencySearchControl.IsEnabled = false;
                Currency.BtnAdd.Visibility = Visibility.Collapsed;
                Currency.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in Currency.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var SaveCurrencyInformation = (FIN_Currency)CollectionViewSource.GetDefaultView(Currency.CurrencyGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(Currency.CurrencyGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Currency.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistCurrency(SaveCurrencyInformation))
                    {
                        ApiAck ack = client.CreateCurrency(SaveCurrencyInformation);
                        if (ack.CallStatus == EApiCallStatus.Success)
                        {
                            MessageBox.Show("Record Added Successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            //Complete the Add operation. i.e.; Re-enable tabs,textboxes and datagrid
                            CompleteAdd();
                        }
                        else
                        {
                            MessageBox.Show(ack.ReturnedMessage, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Record Already Exists", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                //Else the user is trying to update a record
                else
                {
                    ApiAck ack = client.UpdateCurrency(SaveCurrencyInformation);
                    if (ack.CallStatus == EApiCallStatus.Success)
                    {
                        MessageBox.Show("Record Updated Successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        CompleteUpdate();
                    }
                    else
                    {
                        MessageBox.Show(ack.ReturnedMessage, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to Update", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        //User clicks add button; The program has to go to the adding state
        public void Add()
        {
            //In adding state Tabs,Textboxes,Grid has to be disabled
            if (Currency.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_Currency> newCollection = new ObservableCollection<FIN_Currency>() { new FIN_Currency() };
                Currency.BtnSave.Visibility = Visibility.Visible;
                Currency.CurrencyGrid.DataContext = newCollection;
                Currency.CurrencySearchControl.IsEnabled = false;
                Currency.btnAdd_text.Text = "Cancel Add";
                Currency.btnSave_text.Text = "Confirm Add";
                Currency.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Currency.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Currency.txt_CurrencyCodeC.IsEnabled = true;
            }
            //If user cancels the add  , renable textboxes,grid etc
            else
            {
                CompleteAdd();
            }
        }

        //Code for renabling tabs,textboxes etc; when an add operation finishes
        private void CompleteAdd()
        {   //Bind the original collection when the Add operation is complete
            Currency.CurrencyGrid.DataContext = CurrencyInformation;
            Currency.BtnSave.Visibility = Visibility.Collapsed;
            Currency.CurrencySearchControl.IsEnabled = true;
            Currency.btnAdd_text.Text = "Add";
            Currency.BtnAdd.IsChecked = false;
            Currency.btnSave_text.Text = "Update";
            Currency.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Currency.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            Currency.txt_CurrencyCodeC.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CurrencyQuery query = new CurrencyQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new CurrencyQuery() { CurrencyCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new CurrencyQuery() { CurrencyName = SearchControl.SearchTextBox.Text };
            }
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCurrencyAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_Currency>(response.Result.ToList<FIN_Currency>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = CurrencyInformation.IndexOf(Currency.CurrencySearchControl.ResultsGrid.SelectedItem as FIN_Currency);
            CurrencyInformation[index] = EditingCurrencyInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            Currency.BtnSave.Visibility = Visibility.Collapsed;
            Currency.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Currency.CurrencySearchControl.IsEnabled = true;
            Currency.BtnAdd.Visibility = Visibility.Visible;
            Currency.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in Currency.TbPage.Items)
            {
                t.IsEnabled = true;
            }
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var RemoveCurrencyInformation = (FIN_Currency)CollectionViewSource.GetDefaultView(Currency.CurrencyGrid.DataContext).CurrentItem;

                if (client.ExistCurrency(RemoveCurrencyInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        CurrencyInformation.Remove(RemoveCurrencyInformation);
                        ApiAck ack = client.DeleteCurrency(RemoveCurrencyInformation);
                        if (ack.CallStatus == EApiCallStatus.Success)
                        {
                            MessageBox.Show("Record Deleted Successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show(ack.ReturnedMessage, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No such record found to delete", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}