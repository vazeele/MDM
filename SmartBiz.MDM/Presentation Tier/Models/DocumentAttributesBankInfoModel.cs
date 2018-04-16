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
    internal class DocumentAttributesBankInfoModel
    {
        private ObservableCollection<ERP_DocumentAttributesBankInfo> DocumentAttributesBankInformation; //These will be shown in the datagrid
        private Document document; //Reference to Transaction Interface
        private ERP_DocumentAttributesBankInfo EditingDocumentAttributesBankInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public DocumentAttributesBankInfoModel(Document document)
        {
            this.document = document;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            document.DABISearchControl.Search = Search;
            document.DCDABIDropDown.Search = DocumentModel.Search;
            document.TCDABIDropDown.Search = DocumentModel.Search;
            document.FBCDABIDropDown.Search = BankAccountModel.Search;
            document.FBrCDABIDropDown.Search = BankAccountModel.Search;
            document.FASNDropDown.Search = BankAccountModel.Search;

            
            document.DABISearchControl.ResultsGrid.SelectedCellsChanged += dgv_DocumentAttributesBankInfoModel_SelectionChanged;
            document.DocumentAttributesBankInfoGrid.SourceUpdated += DocumentAttributesBankInfoGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(document.DABISearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(document.DABISearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            DocumentAttributesBankInformation = document.DABISearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_DocumentAttributesBankInfo>;
            document.DocumentAttributesBankInfoGrid.DataContext = DocumentAttributesBankInformation;
            if (DocumentAttributesBankInformation.Count > 0)
            {
                EditingDocumentAttributesBankInfo = DocumentAttributesBankInformation[0].Clone();
         
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_DocumentAttributesBankInfoModel_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (document.DABISearchControl.ResultsGrid.SelectedItem != null)
                EditingDocumentAttributesBankInfo = (document.DABISearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributesBankInfo).Clone() as ERP_DocumentAttributesBankInfo;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void DocumentAttributesBankInfoGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (document.DocumentAttributesBankInfoGrid.DataContext == DocumentAttributesBankInformation)
            {
                document.BtnSave.Visibility = Visibility.Visible;
                document.BtnCancelUpdate.Visibility = Visibility.Visible;
                document.DABISearchControl.IsEnabled = false;
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
                var DocumentAttributesBankInfoToSave = (ERP_DocumentAttributesBankInfo)CollectionViewSource.GetDefaultView(document.DocumentAttributesBankInfoGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                document.DCDABIDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                document.TCDABIDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(document.DocumentAttributesBankInfoGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (document.BtnAdd.IsChecked == true)
                {
                    var dabitosave = (DocumentAttributesBankInfoToSave);
                    if (!client.DocumentAttributesBankInfoExists(dabitosave))
                    {
                        ApiAck ack = client.CreateDocumentAttributesBankInfo(dabitosave);
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
                    ApiAck ack = client.UpdateDocumentAttributesBankInfo(DocumentAttributesBankInfoToSave);
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
                ObservableCollection<ERP_DocumentAttributesBankInfo> newCollection = new ObservableCollection<ERP_DocumentAttributesBankInfo>() { new ERP_DocumentAttributesBankInfo() };
                document.BtnSave.Visibility = Visibility.Visible;
                document.DocumentAttributesBankInfoGrid.DataContext = newCollection;
                document.DABISearchControl.IsEnabled = false;
                document.btnAdd_text.Text = "Cancel Add";
                document.btnSave_text.Text = "Confirm Add";
                document.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in document.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                document.DCDABIDropDown.IsEnabled = true;
                document.TCDABIDropDown.IsEnabled = true;
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
            document.DocumentAttributesBankInfoGrid.DataContext = DocumentAttributesBankInformation;
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.DABISearchControl.IsEnabled = true;
            document.btnAdd_text.Text = "Add";
            document.BtnAdd.IsChecked = false;
            document.btnSave_text.Text = "Update";
            document.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in document.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            document.DCDABIDropDown.IsEnabled = false;
            document.TCDABIDropDown.IsEnabled = false;
        }

        //Normal Search method has been made resuable
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            DocumentAttributesBankInfoQuery query = new DocumentAttributesBankInfoQuery();
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new DocumentAttributesBankInfoQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new DocumentAttributesBankInfoQuery() { TxCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new DocumentAttributesBankInfoQuery() { FixedBankCode = SearchControl.SearchTextBox.Text };
            }
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryDocumentAttributesBankInfoAsync(query, pagesize, pagePosition);

            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            var servicelist = response.Result.ToList<ERP_DocumentAttributesBankInfo>();
            var DocumentAttributes = new ObservableCollection<ERP_DocumentAttributesBankInfo>(servicelist);
            SearchControl.ResultsGrid.ItemsSource = DocumentAttributes;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = DocumentAttributesBankInformation.IndexOf(document.DABISearchControl.ResultsGrid.SelectedItem as ERP_DocumentAttributesBankInfo);
            DocumentAttributesBankInformation[index] = EditingDocumentAttributesBankInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            document.BtnSave.Visibility = Visibility.Collapsed;
            document.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            document.DABISearchControl.IsEnabled = true;
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
                var dabitoremove = (ERP_DocumentAttributesBankInfo)CollectionViewSource.GetDefaultView(DocumentAttributesBankInformation).CurrentItem;

                if (client.DocumentAttributesBankInfoExists(dabitoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        DocumentAttributesBankInformation.Remove(dabitoremove);
                        ApiAck ack = client.DeleteDocumentAttributesBankInfo(dabitoremove);
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