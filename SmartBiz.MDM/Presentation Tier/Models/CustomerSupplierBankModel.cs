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
    internal class CustomerSupplierBankModel
    {
        private ObservableCollection<FIN_CustomerSupplierBank> CusomerSupplierBankInformation; //These will be shown in the datagrid
        private Customer CustomerSupplier; //Reference to Customer Supplier Interface
        private FIN_CustomerSupplierBank EditingCusSupBank; //Stores what we're editing right now, Incase we need to cancel the edit

        public CustomerSupplierBankModel(Customer CustomerSupplier)
        {
            this.CustomerSupplier = CustomerSupplier;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            CustomerSupplier.CusSupBankSearchControl.Search = Search;

            CustomerSupplier.CustSupFlagDD.Search = CustomerSupplierModel.Search;
            CustomerSupplier.BankCodeDD.Search = BankBranchModel.Search;
          
            CustomerSupplier.CusSupBankSearchControl.ResultsGrid.SelectedCellsChanged += dgv_CustomerSupplier_SelectionChanged;
            CustomerSupplier.CusSupBankGrid.SourceUpdated += CusSupBankGrid_SourceUpdated;


            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(CustomerSupplier.CusSupBankSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(CustomerSupplier.CusSupBankSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            CusomerSupplierBankInformation = CustomerSupplier.CusSupBankSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_CustomerSupplierBank>;
            CustomerSupplier.CusSupBankGrid.DataContext = CusomerSupplierBankInformation;
            if (CusomerSupplierBankInformation.Count>0)
            {
                EditingCusSupBank = CusomerSupplierBankInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_CustomerSupplier_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (CustomerSupplier.CusSupBankSearchControl.ResultsGrid.SelectedItem != null)
                EditingCusSupBank = (CustomerSupplier.CusSupBankSearchControl.ResultsGrid.SelectedItem as FIN_CustomerSupplierBank).Clone() as FIN_CustomerSupplierBank;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void CusSupBankGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (CustomerSupplier.CusSupBankGrid.DataContext == CusomerSupplierBankInformation)
            {
                CustomerSupplier.BtnSave.Visibility = Visibility.Visible;
                CustomerSupplier.BtnCancelUpdate.Visibility = Visibility.Visible;
                CustomerSupplier.CusSupBankSearchControl.IsEnabled = false;
                CustomerSupplier.BtnAdd.Visibility = Visibility.Collapsed;
                CustomerSupplier.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in CustomerSupplier.TbPage.Items)
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
                var LastCusSupBankToSave = (FIN_CustomerSupplierBank)CollectionViewSource.GetDefaultView(CustomerSupplier.CusSupBankGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                //CustomerSupplier.txt_AccountType_AccountType.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //CustomerSupplier.txt_AccountType_AccountDesc.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(CustomerSupplier.CusSupBankGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (CustomerSupplier.BtnAdd.IsChecked == true)
                {
                    var CusSupBankToSave = (LastCusSupBankToSave);
                    if (!client.CustomerSupplierBankExists(CusSupBankToSave))
                    {
                        ApiAck ack = client.CreateCustomerSupplierBank(CusSupBankToSave);
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
                    ApiAck ack = client.UpdateCustomerSupplierBank(LastCusSupBankToSave);
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
            if (CustomerSupplier.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_CustomerSupplierBank> newCollection = new ObservableCollection<FIN_CustomerSupplierBank>() { new FIN_CustomerSupplierBank() };
                CustomerSupplier.BtnSave.Visibility = Visibility.Visible;
                CustomerSupplier.CusSupBankGrid.DataContext = newCollection;
                CustomerSupplier.CusSupBankSearchControl.IsEnabled = false;          
                CustomerSupplier.btnAdd_text.Text = "Cancel Add";
                CustomerSupplier.btnSave_text.Text = "Confirm Add";
                CustomerSupplier.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in CustomerSupplier.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                CustomerSupplier.CustSupFlagDD.IsEnabled = true;
                CustomerSupplier.BankCodeDD.IsEnabled = true;
                
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
            CustomerSupplier.CusSupBankGrid.DataContext = CusomerSupplierBankInformation;
            CustomerSupplier.BtnSave.Visibility = Visibility.Collapsed;
            CustomerSupplier.CusSupBankSearchControl.IsEnabled = true;
            CustomerSupplier.btnAdd_text.Text = "Add";
            CustomerSupplier.BtnAdd.IsChecked = false;
            CustomerSupplier.btnSave_text.Text = "Update";
            CustomerSupplier.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in CustomerSupplier.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            CustomerSupplier.CustSupFlagDD.IsEnabled = false;
            CustomerSupplier.BankCodeDD.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CustomerSupplierBankQuery query = new CustomerSupplierBankQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new CustomerSupplierBankQuery() { CusSupCode = SearchControl.SearchTextBox.Text };

            if (SearchControl.OptionTwo.IsChecked == true)
                query = new CustomerSupplierBankQuery() { BankCode = SearchControl.SearchTextBox.Text };

            if (SearchControl.OptionTwo.IsChecked == true)
                query = new CustomerSupplierBankQuery() { BranchCode = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCustomerSupplierBankAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_CustomerSupplierBank>(response.Result.ToList<FIN_CustomerSupplierBank>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = CusomerSupplierBankInformation.IndexOf(CustomerSupplier.CusSupBankSearchControl.ResultsGrid.SelectedItem as FIN_CustomerSupplierBank);
            CusomerSupplierBankInformation[index] = EditingCusSupBank;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            CustomerSupplier.BtnSave.Visibility = Visibility.Collapsed;
            CustomerSupplier.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            CustomerSupplier.CusSupBankSearchControl.IsEnabled = true;
            CustomerSupplier.BtnAdd.Visibility = Visibility.Visible;
            CustomerSupplier.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in CustomerSupplier.TbPage.Items)
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
                var CusSupBankToRemove = (FIN_CustomerSupplierBank)CollectionViewSource.GetDefaultView(CusomerSupplierBankInformation).CurrentItem;

                if (client.CustomerSupplierBankExists(CusSupBankToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        CusomerSupplierBankInformation.Remove(CusSupBankToRemove);
                        ApiAck ack = client.DeleteCustomerSupplierBank(CusSupBankToRemove);
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