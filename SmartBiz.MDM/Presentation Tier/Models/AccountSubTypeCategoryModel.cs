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
    internal class AccountSubTypeCategoryModel
    {
        private ObservableCollection<FIN_AccountSubTypeCategory> LastAccountInformation; //These will be shown in the datagrid
        private Accounts account; //Reference to Transaction Interface
        private FIN_AccountSubTypeCategory EditingAccountInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public AccountSubTypeCategoryModel(Accounts account)
        {
            this.account = account;

            account.AccountSubTypeCatDropDown.Search = AccountSubTypeSearchModel.AccountSubTypeSearch;

            account.AccountSubTypeCatSearchControl.ResultsGrid.SelectedCellsChanged += dgv_AccountSubTypeCat_SelectionChanged;
            account.AccountSubTypeCatGrid.SourceUpdated += AccountSubTypeCatGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(account.AccountSubTypeCatSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(account.AccountSubTypeCatSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastAccountInformation = account.AccountSubTypeCatSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_AccountSubTypeCategory>;
            account.AccountSubTypeCatGrid.DataContext = LastAccountInformation;
            if (LastAccountInformation.Count>0) { 
                EditingAccountInfo = LastAccountInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_AccountSubTypeCat_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (account.AccountSubTypeCatSearchControl.ResultsGrid.SelectedItem != null)
                EditingAccountInfo = (account.AccountSubTypeCatSearchControl.ResultsGrid.SelectedItem as FIN_AccountSubTypeCategory).Clone() as FIN_AccountSubTypeCategory;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void AccountSubTypeCatGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (account.AccountSubTypeCatGrid.DataContext == LastAccountInformation)
            {
                account.BtnSave.Visibility = Visibility.Visible;
                account.BtnCancelUpdate.Visibility = Visibility.Visible;
                account.AccountSubTypeCatSearchControl.IsEnabled = false;
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
                var LastAccountToSave = (FIN_AccountSubTypeCategory)CollectionViewSource.GetDefaultView(account.AccountSubTypeCatGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                account.txt_AccountSubTypeCat_AccountSubTypeCat.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                account.txt_AccountSubTypeCat_AccountSubTypeDesc.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(account.AccountSubTypeCatGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (account.BtnAdd.IsChecked == true)
                {
                    var astctosave = (LastAccountToSave);
                    if (!client.AccountSubTypeCategoryExists(astctosave))
                    {
                        ApiAck ack = client.CreateAccountSubTypeCategory(astctosave);
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
                    ApiAck ack = client.UpdateAccountSubTypeCategory(LastAccountToSave);
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
                ObservableCollection<FIN_AccountSubTypeCategory> newCollection = new ObservableCollection<FIN_AccountSubTypeCategory>() { new FIN_AccountSubTypeCategory() };
                account.BtnSave.Visibility = Visibility.Visible;
                account.AccountSubTypeCatGrid.DataContext = newCollection;
                account.AccountSubTypeCatSearchControl.IsEnabled = false;
                account.btnAdd_text.Text = "Cancel Add";
                account.btnSave_text.Text = "Confirm Add";
                account.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in account.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                account.AccountSubTypeCatDropDown.IsEnabled = true;
                account.txt_AccountSubType_AccountSubType.IsEnabled = true;
                account.txt_AccountSubType_AccountSubTypeDesc.IsEnabled = true;
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
            account.AccountSubTypeCatGrid.DataContext = LastAccountInformation;
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.AccountSubTypeCatSearchControl.IsEnabled = true;
            account.btnAdd_text.Text = "Add";
            account.BtnAdd.IsChecked = false;
            account.btnSave_text.Text = "Update";
            account.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in account.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            account.AccountSubTypeCatDropDown.IsEnabled = false;
            account.txt_AccountSubTypeCat_AccountSubTypeCat.IsEnabled = false;
            account.txt_AccountSubTypeCat_AccountSubTypeDesc.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AccountSubTypeCategoryQuery query = new AccountSubTypeCategoryQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AccountSubTypeCategoryQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AccountSubTypeCategoryQuery() { AccountSubType = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AccountSubTypeCategoryQuery() { AccountSubTypeCategory = int.Parse(SearchControl.SearchTextBox.Text) };
            }

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAccountSubTypeCategoryAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_AccountSubTypeCategory>(response.Result.ToList<FIN_AccountSubTypeCategory>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastAccountInformation.IndexOf(account.AccountSubTypeCatSearchControl.ResultsGrid.SelectedItem as FIN_AccountSubTypeCategory);
            LastAccountInformation[index] = EditingAccountInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            account.AccountSubTypeCatSearchControl.IsEnabled = true;
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
                var asttoremove = (FIN_AccountSubTypeCategory)CollectionViewSource.GetDefaultView(LastAccountInformation).CurrentItem;

                if (client.AccountSubTypeCategoryExists(asttoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastAccountInformation.Remove(asttoremove);
                        ApiAck ack = client.DeleteAccountSubTypeCategory(asttoremove);
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