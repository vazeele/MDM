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
    internal class ControlledTransactionModel
    {
        private ObservableCollection<FIN_ControledTransaction> ControlledTransactions; //These will be shown in the datagrid
        private Transaction transaction; //Reference to Transaction Interface
        private FIN_ControledTransaction EditingControlledTransaction; //Stores what we're editing right now, Incase we need to cancel the edit

        public ControlledTransactionModel(Transaction transaction)
        {
            this.transaction = transaction;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            transaction.CTSearchControl.Search = Search;
            transaction.DocAttribCTDropDown.Search = DocumentAttributesModel.Search;
            transaction.CurrencyCTDropDown.Search = CurrencyModel.Search;
            transaction.CostCenterCTRDropDown.Search = CostCenterModel.Search;
            transaction.GLAccountCTDropDown.Search = GeneralLedgerAccountModel.Search;
            transaction.CTSearchControl.ResultsGrid.SelectedCellsChanged += dgv_ControlledTransaction_SelectionChanged;
            transaction.ControlledTransactionGrid.SourceUpdated += ControlledTransactionGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(transaction.CTSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(transaction.CTSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            ControlledTransactions = transaction.CTSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_ControledTransaction>;
            transaction.ControlledTransactionGrid.DataContext = ControlledTransactions;
            if (ControlledTransactions.Count>0)
            {
                EditingControlledTransaction = ControlledTransactions[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_ControlledTransaction_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (transaction.CTSearchControl.ResultsGrid.SelectedItem != null)
                EditingControlledTransaction = (transaction.CTSearchControl.ResultsGrid.SelectedItem as FIN_ControledTransaction).Clone() as FIN_ControledTransaction;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void ControlledTransactionGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (transaction.ControlledTransactionGrid.DataContext == ControlledTransactions)
            {
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.BtnCancelUpdate.Visibility = Visibility.Visible;
                transaction.CTSearchControl.IsEnabled = false;
                transaction.BtnAdd.Visibility = Visibility.Collapsed;
                transaction.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in transaction.TbPage.Items)
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
                var ControlledTransactionToSave = (FIN_ControledTransaction)CollectionViewSource.GetDefaultView(transaction.ControlledTransactionGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                transaction.DocAttribCTDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                transaction.CurrencyCTDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                transaction.GLAccountCTDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                transaction.CostCenterCTRDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(transaction.ControlledTransactionGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (transaction.BtnAdd.IsChecked == true)
                {
                    if (!client.ControledTransactionExists(ControlledTransactionToSave))
                    {
                        ApiAck ack = client.CreateControledTransaction(ControlledTransactionToSave);
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
                    ApiAck ack = client.UpdateControledTransaction(ControlledTransactionToSave);
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
            if (transaction.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_ControledTransaction> newCollection = new ObservableCollection<FIN_ControledTransaction>() { new FIN_ControledTransaction() };
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.ControlledTransactionGrid.DataContext = newCollection;
                transaction.CTSearchControl.IsEnabled = false;
                transaction.btnAdd_text.Text = "Cancel Add";
                transaction.btnSave_text.Text = "Confirm Add";
                transaction.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in transaction.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                transaction.DocAttribCTDropDown.IsEnabled = true;
                transaction.CostCenterCTRDropDown.IsEnabled = true;
                transaction.GLAccountCTDropDown.IsEnabled = true;
                transaction.CurrencyCTDropDown.IsEnabled = true;
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
            transaction.ControlledTransactionGrid.DataContext = ControlledTransactions;
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.CTSearchControl.IsEnabled = true;
            transaction.btnAdd_text.Text = "Add";
            transaction.BtnAdd.IsChecked = false;
            transaction.btnSave_text.Text = "Update";
            transaction.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in transaction.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            transaction.DocAttribCTDropDown.IsEnabled = false;
            transaction.CostCenterCTRDropDown.IsEnabled = false;
            transaction.GLAccountCTDropDown.IsEnabled = false;
            transaction.CurrencyCTDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            ControlledTransactionQuery query = new ControlledTransactionQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new ControlledTransactionQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new ControlledTransactionQuery() { TxnCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new ControlledTransactionQuery() { GLAccountCode = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryControlledTransactionAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_ControledTransaction>(response.Result.ToList<FIN_ControledTransaction>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = ControlledTransactions.IndexOf(transaction.CTSearchControl.ResultsGrid.SelectedItem as FIN_ControledTransaction);
            ControlledTransactions[index] = EditingControlledTransaction;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            transaction.CTSearchControl.IsEnabled = true;
            transaction.BtnAdd.Visibility = Visibility.Visible;
            transaction.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in transaction.TbPage.Items)
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
                var cttoremove = (FIN_ControledTransaction)CollectionViewSource.GetDefaultView(ControlledTransactions).CurrentItem;

                if (client.ControledTransactionExists(cttoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        ControlledTransactions.Remove(cttoremove);
                        ApiAck ack = client.DeleteControledTransaction(cttoremove);
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