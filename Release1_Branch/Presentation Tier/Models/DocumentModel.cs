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
    class DocumentModel
    {
        ObservableCollection<ERP_Document> DocumentInformation; //These will be shown in the datagrid
        Document document; //Reference to Transaction Interface
        ERP_Document EditingDocument; //Stores what we're editing right now, Incase we need to cancel the edit
        public DocumentModel(Document document)
        {
            this.document = document;
            document.DSearchControl.Search = Search;
            //document.DSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_Document_AutoGeneratingColumn;
            document.DSearchControl.ResultsGrid.SelectionChanged += dgv_Document_SelectionChanged;
            document.DocumentGrid.SourceUpdated += DocumentGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(document.DSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(document.DSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            DocumentInformation = document.DSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_Document>;
            document.DocumentGrid.DataContext = DocumentInformation;
        }
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_Document_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(ERP_Document).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
        }

        //When the user selects a row in the datagrid
        void dgv_Document_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (document.DSearchControl.ResultsGrid.SelectedItem != null)
                EditingDocument = (document.DSearchControl.ResultsGrid.SelectedItem as ERP_Document).Clone() as ERP_Document;
        }
        //If the user updates a record, then go to the Update mode automatically
        void DocumentGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (document.DocumentGrid.DataContext == DocumentInformation)
            {
                document.BtnSave.Visibility = Visibility.Visible;
                document.BtnCancelUpdate.Visibility = Visibility.Visible;
                document.DSearchControl.IsEnabled = false;
                document.BtnAdd.Visibility = Visibility.Collapsed;
                document.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in document.TbPage.Items)
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
                var DocumentToSave = (ERP_Document)CollectionViewSource.GetDefaultView(document.DocumentGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                document.txt_DocCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //document.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
               // document.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //document.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(document.DocumentGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (document.BtnAdd.IsChecked == true)
                {

                    var dtosave = (DocumentToSave);
                    if (!client.DocumentExists(dtosave))
                    {
                        ApiAck ack = client.CreateDocument(dtosave);
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
                    ApiAck ack = client.UpdateDocument(DocumentToSave);
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
            if (document.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<ERP_Document> newCollection = new ObservableCollection<ERP_Document>() { new ERP_Document() };
                document.BtnSave.Visibility = Visibility.Visible;
                document.DocumentGrid.DataContext = newCollection;
                document.DSearchControl.IsEnabled = false;
                document.btnAdd_text.Text = "Cancel Add";
                document.btnSave_text.Text = "Confirm Add";
                document.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in document.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
               // document.DocAttribLTIDropDown.IsEnabled = true;
                document.txt_DocCode.IsEnabled = true;
                //document.txt_ProductCodeLTI.IsEnabled = true;

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
            document.DocumentGrid.DataContext = DocumentInformation;
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.DSearchControl.IsEnabled = true;
            document.btnAdd_text.Text = "Add";
            document.BtnAdd.IsChecked = false;
            document.btnSave_text.Text = "Update";
            document.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in document.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            //document.DocAttribLTIDropDown.IsEnabled = false;
            document.txt_DocCode.IsEnabled = false;
            //document.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            DocumentQuery query = new DocumentQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new DocumentQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new DocumentQuery() { DocName = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new DocumentQuery() { SubSystemCode = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryDocumentAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_Document>(response.Result.ToList<ERP_Document>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = DocumentInformation.IndexOf(document.DSearchControl.ResultsGrid.SelectedItem as ERP_Document);
            DocumentInformation[index] = EditingDocument;
            CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            document.DSearchControl.IsEnabled = true;
            document.BtnAdd.Visibility = Visibility.Visible;
            document.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in document.TbPage.Items)
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
                var dtoremove = (ERP_Document)CollectionViewSource.GetDefaultView(DocumentInformation).CurrentItem;

                if (client.DocumentExists(dtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        DocumentInformation.Remove(dtoremove);
                        ApiAck ack = client.DeleteDocument(dtoremove);
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

