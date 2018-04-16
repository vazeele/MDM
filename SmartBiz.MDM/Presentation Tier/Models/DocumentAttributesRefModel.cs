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
    internal class DocumentAttributesRefModel
    {
        private ObservableCollection<ERP_DocumentAttributesRef> DocumentAttributesRefInformation; //These will be shown in the datagrid
        private Document document; //Reference to Transaction Interface
        private ERP_DocumentAttributesRef EditingDocumentAttributesRef; //Stores what we're editing right now, Incase we need to cancel the edit

        public DocumentAttributesRefModel(Document document)
        {
            this.document = document;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            document.DARSearchControl.Search = Search;
            document.DARSearchControl.ResultsGrid.SelectedCellsChanged += dgv_DocumentAttributesRefModel_SelectionChanged;
            document.DocumentAttributesRefGrid.SourceUpdated += DocumentAttributesRefGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(document.DARSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(document.DARSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            DocumentAttributesRefInformation = document.DARSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_DocumentAttributesRef>;
            document.DocumentAttributesRefGrid.DataContext = DocumentAttributesRefInformation;
            if (DocumentAttributesRefInformation.Count > 0)
            {
                EditingDocumentAttributesRef = DocumentAttributesRefInformation[0].Clone() as ERP_DocumentAttributesRef;
         
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_DocumentAttributesRefModel_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (document.DARSearchControl.ResultsGrid.SelectedItem != null)
                EditingDocumentAttributesRef = (document.DARSearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributesRef).Clone() as ERP_DocumentAttributesRef;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void DocumentAttributesRefGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (document.DocumentAttributesRefGrid.DataContext == DocumentAttributesRefInformation)
            {
                document.BtnSave.Visibility = Visibility.Visible;
                document.BtnCancelUpdate.Visibility = Visibility.Visible;
                document.DARSearchControl.IsEnabled = false;
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
                var DocumentAttributesRefToSave = (ERP_DocumentAttributesRef)CollectionViewSource.GetDefaultView(document.DocumentAttributesRefGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                document.txt_DocCodeDAR.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                document.txt_TxCodeDAR.GetBindingExpression(TextBox.TextProperty).UpdateSource();


                //Checking if all controls are in valid state
                if (!Helper.IsValid(document.DocumentAttributesRefGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (document.BtnAdd.IsChecked == true)
                {
                    var dartosave = (DocumentAttributesRefToSave);
                    if (!client.DocumentAttributesRefExists(dartosave))
                    {
                        ApiAck ack = client.CreateDocumentAttributesRef(dartosave);
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
                    ApiAck ack = client.UpdateDocumentAttributesRef(DocumentAttributesRefToSave);
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
                ObservableCollection<ERP_DocumentAttributesRef> newCollection = new ObservableCollection<ERP_DocumentAttributesRef>() { new ERP_DocumentAttributesRef() };
                document.BtnSave.Visibility = Visibility.Visible;
                document.DocumentAttributesRefGrid.DataContext = newCollection;
                document.DARSearchControl.IsEnabled = false;
                document.btnAdd_text.Text = "Cancel Add";
                document.btnSave_text.Text = "Confirm Add";
                document.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in document.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                document.txt_DocCodeDAR.IsEnabled = true;
                document.txt_TxCodeDAR.IsEnabled = true;
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
            document.DocumentAttributesRefGrid.DataContext = DocumentAttributesRefInformation;
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.DARSearchControl.IsEnabled = true;
            document.btnAdd_text.Text = "Add";
            document.BtnAdd.IsChecked = false;
            document.btnSave_text.Text = "Update";
            document.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in document.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            document.txt_DocCodeDAR.IsEnabled = false;
            document.txt_TxCodeDAR.IsEnabled = false;
        }

        //Normal Search method has been made resuable
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            DocumentAttributesRefQuery query = new DocumentAttributesRefQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new DocumentAttributesRefQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new DocumentAttributesRefQuery() { TxCode = SearchControl.SearchTextBox.Text };
            }
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryDocumentAttributesRefAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<ERP_DocumentAttributesRef>();
            var DocumentAttributes = new ObservableCollection<ERP_DocumentAttributesRef>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = DocumentAttributes;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = DocumentAttributesRefInformation.IndexOf(document.DARSearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributesRef);
            DocumentAttributesRefInformation[index] = EditingDocumentAttributesRef;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            document.DARSearchControl.IsEnabled = true;
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
                var dartoremove = (ERP_DocumentAttributesRef)CollectionViewSource.GetDefaultView(DocumentAttributesRefInformation).CurrentItem;

                if (client.DocumentAttributesRefExists(dartoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        DocumentAttributesRefInformation.Remove(dartoremove);
                        ApiAck ack = client.DeleteDocumentAttributesRef(dartoremove);
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