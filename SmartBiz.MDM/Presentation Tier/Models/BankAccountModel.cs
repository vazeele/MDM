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
    internal class BankAccountModel
    {
        private ObservableCollection<FIN_BankAccount> BankAccounts; //These will be shown in the datagrid
        private Bank bank; //Reference to Transaction Interface
        private FIN_BankAccount EditingBankAccount; //Stores what we're editing right now, Incase we need to cancel the edit

        public BankAccountModel(Bank bank)
        {
            this.bank = bank;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            bank.BankAccountSearchControl.Search = Search;
            bank.BankAccountBankBranchDropDown.Search = BankBranchModel.Search;
            bank.BankAccountCurrencyDropDown.Search = CurrencyModel.Search;
            bank.BankAccountGLAccountDropDown.Search = GeneralLedgerAccountModel.Search;
            bank.BankAccountCostCenterDropDown.Search = CostCenterModel.Search;

            bank.BankAccountSearchControl.ResultsGrid.SelectedCellsChanged += SearchControl_SelectionChanged;
            bank.BankAccountGrid.SourceUpdated += EditGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(bank.BankAccountSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(bank.BankAccountSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            BankAccounts = bank.BankAccountSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_BankAccount>;
            bank.BankAccountGrid.DataContext = BankAccounts;
            if (BankAccounts.Count>0)
            {
                EditingBankAccount = BankAccounts[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void SearchControl_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (bank.BankAccountSearchControl.ResultsGrid.SelectedItem != null)
                EditingBankAccount = (bank.BankAccountSearchControl.ResultsGrid.SelectedItem as FIN_BankAccount).Clone();
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EditGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (bank.BankAccountGrid.DataContext == BankAccounts)
            {
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.BtnCancelUpdate.Visibility = Visibility.Visible;
                bank.BankAccountSearchControl.IsEnabled = false;
                bank.BtnAdd.Visibility = Visibility.Collapsed;
                bank.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in bank.TbPage.Items)
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
                var BankAccountToSave = (FIN_BankAccount)CollectionViewSource.GetDefaultView(bank.BankAccountGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                bank.BankAccountBankBranchDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                bank.txt_BankAccount_AccountSEQNo.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(bank.BankAccountGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (bank.BtnAdd.IsChecked == true)
                {
                    if (!client.BankAccountExists(BankAccountToSave))
                    {
                        ApiAck ack = client.CreateBankAccount(BankAccountToSave);
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
                    ApiAck ack = client.UpdateBankAccount(BankAccountToSave);
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
            if (bank.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_BankAccount> newCollection = new ObservableCollection<FIN_BankAccount>() { new FIN_BankAccount() };
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.BankAccountGrid.DataContext = newCollection;
                bank.BankAccountSearchControl.IsEnabled = false;
                bank.btnAdd_text.Text = "Cancel Add";
                bank.btnSave_text.Text = "Confirm Add";
                bank.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in bank.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }

                bank.BankAccountBankBranchDropDown.IsEnabled = true;
                bank.txt_BankAccount_AccountSEQNo.IsEnabled = true;
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
            bank.BankAccountGrid.DataContext = BankAccounts;
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.BankAccountSearchControl.IsEnabled = true;
            bank.btnAdd_text.Text = "Add";
            bank.BtnAdd.IsChecked = false;
            bank.btnSave_text.Text = "Update";
            bank.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in bank.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            bank.BankAccountBankBranchDropDown.IsEnabled = false;
            bank.txt_BankAccount_AccountSEQNo.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            var query = new BankAccountQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new BankAccountQuery() { BankCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new BankAccountQuery() { BankBranchCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                int _AccountSEQNo;
                if (int.TryParse(SearchControl.SearchTextBox.Text, out _AccountSEQNo))
                {
                    query = new BankAccountQuery() { AccountSEQNo = _AccountSEQNo };
                }
                else
                {
                    SearchControl.ResultCount = 0;
                    SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_TxnReference>();
                    MessageBox.Show("AccountSEQNo has to be a number ");
                    return;
                }
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryBankAccountAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_BankAccount>(response.Result.ToList<FIN_BankAccount>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = BankAccounts.IndexOf(bank.BankAccountSearchControl.ResultsGrid.SelectedItem as FIN_BankAccount);
            BankAccounts[index] = EditingBankAccount;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            bank.BankAccountSearchControl.IsEnabled = true;
            bank.BtnAdd.Visibility = Visibility.Visible;
            bank.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in bank.TbPage.Items)
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
                var bankbranchtoremove = (FIN_BankAccount)CollectionViewSource.GetDefaultView(BankAccounts).CurrentItem;

                if (client.BankAccountExists(bankbranchtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        BankAccounts.Remove(bankbranchtoremove);
                        ApiAck ack = client.DeleteBankAccount(bankbranchtoremove);
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