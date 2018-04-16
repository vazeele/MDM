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
    class DocumentAttributesModel
    {
        ObservableCollection<ERP_DocumentAttributes> DocumentAttributesInformation; //These will be shown in the datagrid
        Document document; //Reference to Transaction Interface
        ERP_DocumentAttributes EditingDocumentAttributes; //Stores what we're editing right now, Incase we need to cancel the edit
        public DocumentAttributesModel(Document document)
        {
            this.document = document;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            document.DASearchControl.Search = Search;
            document.DCDADropDown.Search = DocumentModel.Search;
            //document.DASearchControl.ResultsGrid.AutoGeneratingColumn += dgv_DocumentAttributesModel_AutoGeneratingColumn;
            document.DASearchControl.ResultsGrid.SelectionChanged += dgv_DocumentAttributesModel_SelectionChanged;
            document.DocumentAttributesGrid.SourceUpdated += DocumentAttributesGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(document.DASearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(document.DASearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            DocumentAttributesInformation = document.DASearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_DocumentAttributes>;
            document.DocumentAttributesGrid.DataContext = DocumentAttributesInformation;
        }
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_DocumentAttributesModel_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(ERP_DocumentAttributes).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
        }
        //Fill Datagrid of the ForeignKeyDropDown when it's dropped down
        void CustomCombo_DropDownOpened(object sender, EventArgs e)
        {
            DocumentAttributesModel.Search(document.DCDADropDown.SearchControl);
        }
        //Search button of the ForeignKey drop down
        public void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            document.DCDADropDown.ResetPager();
            DocumentAttributesModel.Search(document.DCDADropDown.SearchControl);
        }
        //When the user selects a row in the datagrid
        void dgv_DocumentAttributesModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (document.DASearchControl.ResultsGrid.SelectedItem != null)
                EditingDocumentAttributes = (document.DASearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributes).Clone() as ERP_DocumentAttributes;
        }
        //If the user updates a record, then go to the Update mode automatically
        void DocumentAttributesGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (document.DocumentAttributesGrid.DataContext == DocumentAttributesInformation)
            {
                document.BtnSave.Visibility = Visibility.Visible;
                document.BtnCancelUpdate.Visibility = Visibility.Visible;
                document.DASearchControl.IsEnabled = false;
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
                var DocumentAttributesToSave = (ERP_DocumentAttributes)CollectionViewSource.GetDefaultView(document.DocumentAttributesGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                //document.txt_LastTransactionSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //document.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //document.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                document.txt_TxCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(document.DocumentAttributesGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (document.BtnAdd.IsChecked == true)
                {

                    var datosave = (DocumentAttributesToSave);
                    if (!client.DocumentAttributesExists(datosave))
                    {
                        ApiAck ack = client.CreateDocumentAttributes(datosave);
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
                    ApiAck ack = client.UpdateDocumentAttributes(DocumentAttributesToSave);
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
                ObservableCollection<ERP_DocumentAttributes> newCollection = new ObservableCollection<ERP_DocumentAttributes>() { new ERP_DocumentAttributes() };
                document.BtnSave.Visibility = Visibility.Visible;
                document.DocumentAttributesGrid.DataContext = newCollection;
                document.DASearchControl.IsEnabled = false;
                document.btnAdd_text.Text = "Cancel Add";
                document.btnSave_text.Text = "Confirm Add";
                document.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in document.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                document.DCDADropDown.IsEnabled = true;
                document.txt_TxCode.IsEnabled = true;
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
            document.DocumentAttributesGrid.DataContext = DocumentAttributesInformation;
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.DASearchControl.IsEnabled = true;
            document.btnAdd_text.Text = "Add";
            document.BtnAdd.IsChecked = false;
            document.btnSave_text.Text = "Update";
            document.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in document.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            document.DCDADropDown.IsEnabled = false;
            document.txt_TxCode.IsEnabled = false;
            //document.txt_ProductCodeLTI.IsEnabled = false;
        }

        //Normal Search method has been made resuable
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            DocumentAttributesQuery query = new DocumentAttributesQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new DocumentAttributesQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new DocumentAttributesQuery() { TxCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new DocumentAttributesQuery() { ShortName = SearchControl.SearchTextBox.Text };
            }
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryDocumentAttributesAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<ERP_DocumentAttributes>();
            var DocumentAttributes = new ObservableCollection<ERP_DocumentAttributes>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = DocumentAttributes;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = DocumentAttributesInformation.IndexOf(document.DASearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributes);
            DocumentAttributesInformation[index] = EditingDocumentAttributes;
            CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            document.DASearchControl.IsEnabled = true;
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
                var datoremove = (ERP_DocumentAttributes)CollectionViewSource.GetDefaultView(DocumentAttributesInformation).CurrentItem;

                if (client.DocumentAttributesExists(datoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        DocumentAttributesInformation.Remove(datoremove);
                        ApiAck ack = client.DeleteDocumentAttributes(datoremove);
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

