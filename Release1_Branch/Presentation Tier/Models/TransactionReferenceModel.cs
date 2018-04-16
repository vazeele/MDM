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
    class TransactionReferenceModel
    {
        ObservableCollection<FIN_TxnReference> TransactionReferences; //These will be shown in the datagrid
        Transaction transaction; //Reference to Transaction Interface
        FIN_TxnReference EditingTransactionReference; //Stores what we're editing right now, Incase we need to cancel the edit
        public TransactionReferenceModel(Transaction transaction)
        {
            this.transaction = transaction;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            transaction.TRSearchControl.Search = Search;
            transaction.DocAttribTRDropDown.Search = DocumentAttributesModel.Search;

            //transaction.TRSearchControl.ResultsGrid.AutoGeneratingColumn += ResultsGrid_AutoGeneratingColumn;  
       
            transaction.TRSearchControl.ResultsGrid.SelectionChanged += SearchControl_SelectionChanged;
            transaction.TransactionReferenceGrid.SourceUpdated += EditGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(transaction.TRSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(transaction.TRSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            TransactionReferences = transaction.TRSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_TxnReference>;
            transaction.TransactionReferenceGrid.DataContext = TransactionReferences;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void ResultsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(FIN_TxnReference).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
     
        //When the user selects a row in the datagrid
        void SearchControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transaction.TRSearchControl.ResultsGrid.SelectedItem != null)
                EditingTransactionReference = (transaction.TRSearchControl.ResultsGrid.SelectedItem as FIN_TxnReference).Clone();
        }
        //If the user updates a record, then go to the Update mode automatically
        void EditGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (transaction.TransactionReferenceGrid.DataContext == TransactionReferences)
            {
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.BtnCancelUpdate.Visibility = Visibility.Visible;
                transaction.TRSearchControl.IsEnabled = false;
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
                var TransactionReferenceToSave = (FIN_TxnReference)CollectionViewSource.GetDefaultView(transaction.TransactionReferenceGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                transaction.txt_RefSeqTR .GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.txt_SeqNoTR.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.txt_ReferenceTextTR.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.DocAttribTRDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
               

                //Checking if all controls are in valid state
                if (!Helper.IsValid(transaction.TransactionReferenceGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (transaction.BtnAdd.IsChecked == true)
                {
                    
                    if (!client.TxnReferenceExists(TransactionReferenceToSave))
                    {
                        ApiAck ack = client.CreateTxnReference(TransactionReferenceToSave);
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
                    ApiAck ack = client.UpdateTxnReference(TransactionReferenceToSave);
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
                ObservableCollection<FIN_TxnReference>  newCollection = new ObservableCollection<FIN_TxnReference>() { new FIN_TxnReference()};
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.TransactionReferenceGrid.DataContext = newCollection;
                transaction.TRSearchControl.IsEnabled = false;
                transaction.btnAdd_text.Text = "Cancel Add";
                transaction.btnSave_text.Text = "Confirm Add";
                transaction.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in transaction.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                transaction.DocAttribTRDropDown.IsEnabled = true;
                transaction.txt_RefSeqTR.IsEnabled = true;
                     

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
            transaction.TransactionReferenceGrid.DataContext = TransactionReferences;              
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.TRSearchControl.IsEnabled = true;
            transaction.btnAdd_text.Text = "Add";
            transaction.BtnAdd.IsChecked = false;
            transaction.btnSave_text.Text = "Update";
            transaction.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in transaction.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            transaction.DocAttribTRDropDown.IsEnabled = false;
            transaction.txt_RefSeqTR.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            var query = new TransactionReferenceQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new TransactionReferenceQuery() { ReferenceText = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                short _RefSeq;
                if (short.TryParse(SearchControl.SearchTextBox.Text, out _RefSeq))
                {
                    query = new TransactionReferenceQuery() { RefSeq = _RefSeq };
                }
                else
                {
                   
                    SearchControl.ResultCount = 0;                          
                    SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_TxnReference>();
                    MessageBox.Show("RefSeq has to be a number ");
                    return;
                }
               
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new TransactionReferenceQuery() { DocCode = SearchControl.SearchTextBox.Text };
                   
            }
         
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryTxnReferenceAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_TxnReference>(response.Result.ToList<FIN_TxnReference>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= TransactionReferences.IndexOf(transaction.TRSearchControl.ResultsGrid.SelectedItem as FIN_TxnReference);
           TransactionReferences [index]= EditingTransactionReference;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            transaction.TRSearchControl.IsEnabled = true;
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
                var trtoremove = (FIN_TxnReference)CollectionViewSource.GetDefaultView(TransactionReferences).CurrentItem;

                if (client.TxnReferenceExists(trtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        TransactionReferences.Remove(trtoremove);
                        ApiAck ack = client.DeleteTxnReference(trtoremove);
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

