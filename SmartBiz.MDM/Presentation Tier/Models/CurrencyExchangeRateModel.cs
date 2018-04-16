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
    internal class CurrencyExchangeRateModel
    {
        private ObservableCollection<FIN_CurrencyExchangeRate> CurrencyExchangeRateInformation; //These will be shown in the datagrid
        private Currency Currency; //Reference to Transaction Interface
        private FIN_CurrencyExchangeRate EditingCurrencyExchangeRateInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public CurrencyExchangeRateModel(Currency Currency)
        {
            this.Currency = Currency;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Currency.CurrencyExchangeRateSearchControl.Search = Search;
            Currency.CurrencyCodeDropDown.Search = CurrencyModel.Search;
            Currency.CurrencyExchangeRateSearchControl.ResultsGrid.SelectedCellsChanged += dgv_CurrencyExchangeRate_SelectionChanged;

            Currency.CurrencyExchangeRateGrid.SourceUpdated += CurrencyExchangeRateGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(Currency.CurrencyExchangeRateSearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(Currency.CurrencyExchangeRateSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            CurrencyExchangeRateInformation = Currency.CurrencyExchangeRateSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_CurrencyExchangeRate>;
            Currency.CurrencyExchangeRateGrid.DataContext = CurrencyExchangeRateInformation;
            if (CurrencyExchangeRateInformation.Count>0)
            {
                EditingCurrencyExchangeRateInfomation = CurrencyExchangeRateInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_CurrencyExchangeRate_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Currency.CurrencyExchangeRateSearchControl.ResultsGrid.SelectedItem != null)
                EditingCurrencyExchangeRateInfomation = (Currency.CurrencyExchangeRateSearchControl.ResultsGrid.SelectedItem as FIN_CurrencyExchangeRate).Clone() as FIN_CurrencyExchangeRate;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void CurrencyExchangeRateGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Currency.CurrencyExchangeRateGrid.DataContext == CurrencyExchangeRateInformation)
            {
                Currency.BtnSave.Visibility = Visibility.Visible;
                Currency.BtnCancelUpdate.Visibility = Visibility.Visible;
                Currency.CurrencyExchangeRateSearchControl.IsEnabled = false;
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
                var SaveCurrencyExchangeRateInformation = (FIN_CurrencyExchangeRate)CollectionViewSource.GetDefaultView(Currency.CurrencyExchangeRateGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(Currency.CurrencyExchangeRateGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Currency.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistCurrencyExchangeRate(SaveCurrencyExchangeRateInformation))
                    {
                        ApiAck ack = client.CreateCurrencyExchangeRate(SaveCurrencyExchangeRateInformation);
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
                    ApiAck ack = client.UpdateCurrencyExchangeRate(SaveCurrencyExchangeRateInformation);
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
                ObservableCollection<FIN_CurrencyExchangeRate> newCollection = new ObservableCollection<FIN_CurrencyExchangeRate>() { new FIN_CurrencyExchangeRate() };
                Currency.BtnSave.Visibility = Visibility.Visible;
                Currency.CurrencyExchangeRateGrid.DataContext = newCollection;
                Currency.CurrencyExchangeRateSearchControl.IsEnabled = false;
                Currency.btnAdd_text.Text = "Cancel Add";
                Currency.btnSave_text.Text = "Confirm Add";
                Currency.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Currency.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Currency.CurrencyCodeDropDown.IsEnabled = true;
                Currency.txt_CalenderMonthCER.IsEnabled = true;
                Currency.txt_CalenderYearCER.IsEnabled = true;
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
            Currency.CurrencyExchangeRateGrid.DataContext = CurrencyExchangeRateInformation;
            Currency.BtnSave.Visibility = Visibility.Collapsed;
            Currency.CurrencyExchangeRateSearchControl.IsEnabled = true;
            Currency.btnAdd_text.Text = "Add";
            Currency.BtnAdd.IsChecked = false;
            Currency.btnSave_text.Text = "Update";
            Currency.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Currency.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            Currency.CurrencyCodeDropDown.IsEnabled = false;
            Currency.txt_CalenderMonthCER.IsEnabled = false;
            Currency.txt_CalenderYearCER.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CurrencyExchangeRateQuery query = new CurrencyExchangeRateQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new CurrencyExchangeRateQuery() { CurrencyCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new CurrencyExchangeRateQuery() { CalenderYear = Int16.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new CurrencyExchangeRateQuery() { CalenderMonth = byte.Parse(SearchControl.SearchTextBox.Text) };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCurrencyExchangeRateAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_CurrencyExchangeRate>(response.Result.ToList<FIN_CurrencyExchangeRate>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = CurrencyExchangeRateInformation.IndexOf(Currency.CurrencyExchangeRateSearchControl.ResultsGrid.SelectedItem as FIN_CurrencyExchangeRate);
            CurrencyExchangeRateInformation[index] = EditingCurrencyExchangeRateInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            Currency.BtnSave.Visibility = Visibility.Collapsed;
            Currency.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Currency.CurrencyExchangeRateSearchControl.IsEnabled = true;
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
                var RemoveCurrencyExchangeRateInformation = (FIN_CurrencyExchangeRate)CollectionViewSource.GetDefaultView(Currency.CurrencyExchangeRateGrid.DataContext).CurrentItem;

                if (client.ExistCurrencyExchangeRate(RemoveCurrencyExchangeRateInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        CurrencyExchangeRateInformation.Remove(RemoveCurrencyExchangeRateInformation);
                        ApiAck ack = client.DeleteCurrencyExchangeRate(RemoveCurrencyExchangeRateInformation);
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