using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SmartBiz.MDMAPI.Common.Entities;
using System.Windows.Data;
using System.Windows.Controls;
using System.Runtime.Serialization;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDM.Presentation.Models;
using SmartBiz.MDM.Presentation.CustomControls;
using System.ComponentModel;
namespace SmartBiz.MDM.Presentation
{
    class FixedTxnAttributesModel
    {
        ObservableCollection<ERP_FixedTxnAttributes> FixedTxnAttributes; //These will be shown in the datagrid
        Transaction transaction; //Reference to Transaction Interface
        ERP_FixedTxnAttributes EditingFixedTxnAttribute; //Stores what we're editing right now, Incase we need to cancel the edit
        public FixedTxnAttributesModel(Transaction transaction)
        {
            this.transaction = transaction;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            transaction.FTASearchControl.Search = Search;
            transaction.DocAttribFTADropDown.Search = DocumentAttributesModel.Search;
            transaction.CostCrdCurrencyFTADropDown.Search = CurrencyModel.Search;
            transaction.CostDbtCurrencyFTADropDown.Search = CurrencyModel.Search;
            transaction.SalesDbtCurrencyFTADropDown.Search = CurrencyModel.Search;
            transaction.SalesCrdCurrencyDropDown.Search = CurrencyModel.Search;
            transaction.CostCrdCostCenterFTADropDown.Search = CostCenterModel.Search;
            transaction.CostDbtCostCenterFTADropDown.Search = CostCenterModel.Search;
            transaction.SalesCrdCostCenterFTADropDown.Search = CostCenterModel.Search;
            transaction.SalesDbtCostCenterFTADropDown.Search = CostCenterModel.Search;
            transaction.GLFTADropDown.Search = GeneralLedgerAccountModel.Search;
            transaction.CostCrdGLFTADropDown.Search = GeneralLedgerAccountModel.Search;
            transaction.CostDbtGLFTADropDown.Search = GeneralLedgerAccountModel.Search;
            transaction.SalesCrdGLFTADropDown.Search = GeneralLedgerAccountModel.Search;
            transaction.SalesDbtGLFTADropDown.Search = GeneralLedgerAccountModel.Search;


         //   transaction.FTASearchControl.ResultsGrid.AutoGeneratingColumn += dgv_FixedTxnAttributes_AutoGeneratingColumn;            
            transaction.FTASearchControl.ResultsGrid.SelectionChanged += dgv_FixedTxnAttributes_SelectionChanged;

            transaction.FixedTransactionAttribGrid.SourceUpdated += FixedTransactionAttribGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(transaction.FTASearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(transaction.FTASearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            FixedTxnAttributes = transaction.FTASearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_FixedTxnAttributes>;
            transaction.FixedTransactionAttribGrid.DataContext = FixedTxnAttributes;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
      
        //When the user selects a row in the datagrid
        void dgv_FixedTxnAttributes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transaction.FTASearchControl.ResultsGrid.SelectedItem != null)
                EditingFixedTxnAttribute = (transaction.FTASearchControl.ResultsGrid.SelectedItem as ERP_FixedTxnAttributes).Clone() as ERP_FixedTxnAttributes;
        }
        //If the user updates a record, then go to the Update mode automatically
        void FixedTransactionAttribGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (transaction.FixedTransactionAttribGrid.DataContext == FixedTxnAttributes)
            {
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.BtnCancelUpdate.Visibility = Visibility.Visible;
                transaction.FTASearchControl.IsEnabled = false;
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
                var FixedTxnAttributeToSave = (ERP_FixedTxnAttributes)CollectionViewSource.GetDefaultView(transaction.FixedTransactionAttribGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                transaction.DocAttribFTADropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                transaction.GLFTADropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                 

                //Checking if all controls are in valid state
                if (!Helper.IsValid(transaction.FixedTransactionAttribGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (transaction.BtnAdd.IsChecked == true)
                {
                    
                 
                    if (!client.FixedTxnAttributesExists(FixedTxnAttributeToSave))
                    {
                        ApiAck ack = client.CreateFixedTxnAttributes(FixedTxnAttributeToSave);
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
                    ApiAck ack = client.UpdateFixedTxnAttributes(FixedTxnAttributeToSave);
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
                ObservableCollection<ERP_FixedTxnAttributes>  newCollection = new ObservableCollection<ERP_FixedTxnAttributes>() { new ERP_FixedTxnAttributes()};
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.FixedTransactionAttribGrid.DataContext = newCollection;
                transaction.FTASearchControl.IsEnabled = false;
                transaction.btnAdd_text.Text = "Cancel Add";
                transaction.btnSave_text.Text = "Confirm Add";
                transaction.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in transaction.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                transaction.DocAttribFTADropDown.IsEnabled = true;
                transaction.GLFTADropDown.IsEnabled = true;
             

            }
            //If user cancels the add  , renable textboxes,grid etc;
            else
            {
                CompleteAdd();
            }
        }
        //Code for renabling tabs,textboxes etc; when an add operation finishes
        private void CompleteAdd()
        {   //Bind the original collection when the Add operation is complete       
            transaction.FixedTransactionAttribGrid.DataContext = FixedTxnAttributes;              
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.FTASearchControl.IsEnabled = true;
            transaction.btnAdd_text.Text = "Add";
            transaction.BtnAdd.IsChecked = false;
            transaction.btnSave_text.Text = "Update";
            transaction.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in transaction.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            transaction.DocAttribFTADropDown.IsEnabled = false;
            transaction.GLFTADropDown.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            FixedTxnAttributesQuery query = new FixedTxnAttributesQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new FixedTxnAttributesQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new FixedTxnAttributesQuery() { TxCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new FixedTxnAttributesQuery() { GLCode = SearchControl.SearchTextBox.Text };
               
            }
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryFixedTransactionAttributesAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_FixedTxnAttributes>(response.Result.ToList<ERP_FixedTxnAttributes>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= FixedTxnAttributes.IndexOf(transaction.FTASearchControl.ResultsGrid.SelectedItem as ERP_FixedTxnAttributes);
           FixedTxnAttributes [index]= EditingFixedTxnAttribute;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            transaction.FTASearchControl.IsEnabled = true;
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
                var ftatoremove = (ERP_FixedTxnAttributes)CollectionViewSource.GetDefaultView(FixedTxnAttributes).CurrentItem;        

                if (client.FixedTxnAttributesExists(ftatoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        FixedTxnAttributes.Remove(ftatoremove);
                        ApiAck ack = client.DeleteFixedTxnAttributes(ftatoremove);
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

