using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.Models;
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
    internal class AccountSubTypeModel
    {
        private ObservableCollection<FIN_AccountSubType> LastAccountInformation; //These will be shown in the datagrid
        private Accounts account; //Reference to Transaction Interface
        private FIN_AccountSubType EditingAccountInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public AccountSubTypeModel(Accounts account)
        {
            this.account = account;

            account.AccountSubTypeDropDown.Search = AccountTypeSearchModel.AccountTypeSearch;

            account.AccountSubTypeSearchControl.ResultsGrid.SelectedCellsChanged += dgv_AccountSubType_SelectionChanged;
            account.AccountSubTypeGrid.SourceUpdated += AccountSubTypeGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(account.AccountSubTypeSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(account.AccountSubTypeSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastAccountInformation = account.AccountSubTypeSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_AccountSubType>;
            account.AccountSubTypeGrid.DataContext = LastAccountInformation;
            if (LastAccountInformation.Count>0)
            {
                EditingAccountInfo = LastAccountInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_AccountSubType_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (account.AccountSubTypeSearchControl.ResultsGrid.SelectedItem != null)
                EditingAccountInfo = (account.AccountSubTypeSearchControl.ResultsGrid.SelectedItem as FIN_AccountSubType).Clone() as FIN_AccountSubType;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void AccountSubTypeGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (account.AccountSubTypeGrid.DataContext == LastAccountInformation)
            {
                account.BtnSave.Visibility = Visibility.Visible;
                account.BtnCancelUpdate.Visibility = Visibility.Visible;
                account.AccountSubTypeSearchControl.IsEnabled = false;
                account.BtnAdd.Visibility = Visibility.Collapsed;
                account.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in account.TbPage.Items)
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
                var LastAccountToSave = (FIN_AccountSubType)CollectionViewSource.GetDefaultView(account.AccountSubTypeGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                account.txt_AccountSubType_AccountSubType.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                account.txt_AccountSubType_AccountSubTypeDesc.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(account.AccountSubTypeGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (account.BtnAdd.IsChecked == true)
                {
                    var asttosave = (LastAccountToSave);
                    if (!client.AccountSubTypeExists(asttosave))
                    {
                        ApiAck ack = client.CreateAccountSubType(asttosave);
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
                    ApiAck ack = client.UpdateAccountSubType(LastAccountToSave);
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
            if (account.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_AccountSubType> newCollection = new ObservableCollection<FIN_AccountSubType>() { new FIN_AccountSubType() };
                account.BtnSave.Visibility = Visibility.Visible;
                account.AccountSubTypeGrid.DataContext = newCollection;
                account.AccountSubTypeSearchControl.IsEnabled = false;
                account.btnAdd_text.Text = "Cancel Add";
                account.btnSave_text.Text = "Confirm Add";
                account.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in account.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                account.AccountSubTypeDropDown.IsEnabled = true;
                account.txt_AccountSubType_AccountSubType.IsEnabled = true;
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
            account.AccountSubTypeGrid.DataContext = LastAccountInformation;
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.AccountSubTypeSearchControl.IsEnabled = true;
            account.btnAdd_text.Text = "Add";
            account.BtnAdd.IsChecked = false;
            account.btnSave_text.Text = "Update";
            account.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in account.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            account.AccountSubTypeDropDown.IsEnabled = false;
            account.txt_AccountSubType_AccountSubType.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AccountSubTypeQuery query = new AccountSubTypeQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AccountSubTypeQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AccountSubTypeQuery() { AccountSubType = int.Parse(SearchControl.SearchTextBox.Text) };
            }

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAccountSubTypeAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_AccountSubType>(response.Result.ToList<FIN_AccountSubType>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastAccountInformation.IndexOf(account.AccountSubTypeSearchControl.ResultsGrid.SelectedItem as FIN_AccountSubType);
            LastAccountInformation[index] = EditingAccountInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            account.AccountSubTypeSearchControl.IsEnabled = true;
            account.BtnAdd.Visibility = Visibility.Visible;
            account.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in account.TbPage.Items)
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
                var asttoremove = (FIN_AccountSubType)CollectionViewSource.GetDefaultView(LastAccountInformation).CurrentItem;

                if (client.AccountSubTypeExists(asttoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastAccountInformation.Remove(asttoremove);
                        ApiAck ack = client.DeleteAccountSubType(asttoremove);
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