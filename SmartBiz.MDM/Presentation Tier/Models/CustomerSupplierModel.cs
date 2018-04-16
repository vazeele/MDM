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
    internal class CustomerSupplierModel
    {
        private ObservableCollection<FIN_CustomerSupplier_Info> LastCusomerSupplierInformation; //These will be shown in the datagrid
        private Customer CustomerSupplier; //Reference to Customer Supplier Interface
        private FIN_CustomerSupplier_Info EditingCusSupInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public CustomerSupplierModel(Customer CustomerSupplier)
        {
            this.CustomerSupplier = CustomerSupplier;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            CustomerSupplier.CusSupInfoSearchControl.Search = Search;

            CustomerSupplier.cmb_CusSupInfo_Flag.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "CustSupFlag");
            CustomerSupplier.cmb_CusSupInfo_ForeignLocalFlag.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "ForeignLocalFlag");
            CustomerSupplier.cmb_CusSupInfo_Status.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "CustSupStatus");
            CustomerSupplier.cmb_CusSupInfo_TaxFlag.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "TaxFlag");
            CustomerSupplier.cmb_CusSupInfo_Type.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "CustSupType");
            CustomerSupplier.cmb_CusSupInfo_PaymentMode.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "PaymentMode");
            CustomerSupplier.cmb_CusSupInfo_CreditPeriodUnit.ItemsSource = Helper.getItemSource(typeof(FIN_CustomerSupplier_Info), "CreditPeriodUnit");




            CustomerSupplier.CusSupInfoSearchControl.ResultsGrid.SelectedCellsChanged += dgv_CustomerSupplier_SelectionChanged;
            CustomerSupplier.CusSupInfoGrid.SourceUpdated += CusSupInfoGrid_SourceUpdated;


            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(CustomerSupplier.CusSupInfoSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(CustomerSupplier.CusSupInfoSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastCusomerSupplierInformation = CustomerSupplier.CusSupInfoSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_CustomerSupplier_Info>;
            CustomerSupplier.CusSupInfoGrid.DataContext = LastCusomerSupplierInformation;
            if (LastCusomerSupplierInformation.Count>0)
            {
                EditingCusSupInfo = LastCusomerSupplierInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_CustomerSupplier_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (CustomerSupplier.CusSupInfoSearchControl.ResultsGrid.SelectedItem != null)
                EditingCusSupInfo = (CustomerSupplier.CusSupInfoSearchControl.ResultsGrid.SelectedItem as FIN_CustomerSupplier_Info).Clone() as FIN_CustomerSupplier_Info;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void CusSupInfoGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (CustomerSupplier.CusSupInfoGrid.DataContext == LastCusomerSupplierInformation)
            {
                CustomerSupplier.BtnSave.Visibility = Visibility.Visible;
                CustomerSupplier.BtnCancelUpdate.Visibility = Visibility.Visible;
                CustomerSupplier.CusSupInfoSearchControl.IsEnabled = false;
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
                var LastCusSupInfoToSave = (FIN_CustomerSupplier_Info)CollectionViewSource.GetDefaultView(CustomerSupplier.CusSupInfoGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                //CustomerSupplier.txt_AccountType_AccountType.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //CustomerSupplier.txt_AccountType_AccountDesc.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(CustomerSupplier.CusSupInfoGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (CustomerSupplier.BtnAdd.IsChecked == true)
                {
                    var CusSupInfotosave = (LastCusSupInfoToSave);
                    if (!client.CustomerSupplierInfoExists(CusSupInfotosave))
                    {
                        ApiAck ack = client.CreateCustomerSupplierInfo(CusSupInfotosave);
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
                    ApiAck ack = client.UpdateCustomerSupplierInfo(LastCusSupInfoToSave);
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
                ObservableCollection<FIN_CustomerSupplier_Info> newCollection = new ObservableCollection<FIN_CustomerSupplier_Info>() { new FIN_CustomerSupplier_Info() };
                CustomerSupplier.BtnSave.Visibility = Visibility.Visible;
                CustomerSupplier.CusSupInfoGrid.DataContext = newCollection;
                CustomerSupplier.CusSupInfoSearchControl.IsEnabled = false;
                CustomerSupplier.cmb_CusSupInfo_Flag.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_ForeignLocalFlag.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_Status.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_TaxFlag.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_Type.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_PaymentMode.SelectedIndex = 0;
                CustomerSupplier.cmb_CusSupInfo_CreditPeriodUnit.SelectedIndex = 0;
                CustomerSupplier.btnAdd_text.Text = "Cancel Add";
                CustomerSupplier.btnSave_text.Text = "Confirm Add";
                CustomerSupplier.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in CustomerSupplier.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                CustomerSupplier.txt_CusSupInfo_Code.IsEnabled = true;
                
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
            CustomerSupplier.CusSupInfoGrid.DataContext = LastCusomerSupplierInformation;
            CustomerSupplier.BtnSave.Visibility = Visibility.Collapsed;
            CustomerSupplier.CusSupInfoSearchControl.IsEnabled = true;
            CustomerSupplier.btnAdd_text.Text = "Add";
            CustomerSupplier.BtnAdd.IsChecked = false;
            CustomerSupplier.btnSave_text.Text = "Update";
            CustomerSupplier.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in CustomerSupplier.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            CustomerSupplier.txt_CusSupInfo_Code.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CustomerSupplierQuery query = new CustomerSupplierQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new CustomerSupplierQuery() { CusSupFlag = int.Parse(SearchControl.SearchTextBox.Text) };

            if (SearchControl.OptionTwo.IsChecked == true)
                query = new CustomerSupplierQuery() { CusSupCode = SearchControl.SearchTextBox.Text };

            if (SearchControl.OptionTwo.IsChecked == true)
                query = new CustomerSupplierQuery() { Name = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCustomerSupplierAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_CustomerSupplier_Info>(response.Result.ToList<FIN_CustomerSupplier_Info>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastCusomerSupplierInformation.IndexOf(CustomerSupplier.CusSupInfoSearchControl.ResultsGrid.SelectedItem as FIN_CustomerSupplier_Info);
            LastCusomerSupplierInformation[index] = EditingCusSupInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            CustomerSupplier.BtnSave.Visibility = Visibility.Collapsed;
            CustomerSupplier.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            CustomerSupplier.CusSupInfoSearchControl.IsEnabled = true;
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
                var CusSupInfoToRemove = (FIN_CustomerSupplier_Info)CollectionViewSource.GetDefaultView(LastCusomerSupplierInformation).CurrentItem;

                if (client.CustomerSupplierInfoExists(CusSupInfoToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastCusomerSupplierInformation.Remove(CusSupInfoToRemove);
                        ApiAck ack = client.DeleteCustomerSupplierInfo(CusSupInfoToRemove);
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