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
    internal class LastTransactionInfoModel
    {
        private ObservableCollection<ERP_LastTransactionInfo> LastTransactionInformation; //These will be shown in the datagrid
        private Transaction transaction; //Reference to Transaction Interface
        private ERP_LastTransactionInfo EditingLastTransactionInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public LastTransactionInfoModel(Transaction transaction)
        {
            this.transaction = transaction;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            transaction.LTISearchControl.Search = Search;
            transaction.DocAttribLTIDropDown.Search = DocumentAttributesModel.Search;
            transaction.LTISearchControl.ResultsGrid.SelectedCellsChanged += dgv_LastTransactionInfo_SelectionChanged;
            transaction.LastTransactionInfoGrid.SourceUpdated += LastTransactionInfoGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(transaction.LTISearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(transaction.LTISearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastTransactionInformation = transaction.LTISearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_LastTransactionInfo>;
            transaction.LastTransactionInfoGrid.DataContext = LastTransactionInformation;
            if (LastTransactionInformation.Count > 0)
            {
                EditingLastTransactionInfo = LastTransactionInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_LastTransactionInfo_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (transaction.LTISearchControl.ResultsGrid.SelectedItem != null)
                EditingLastTransactionInfo = (transaction.LTISearchControl.ResultsGrid.SelectedItem as ERP_LastTransactionInfo).Clone() as ERP_LastTransactionInfo;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void LastTransactionInfoGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (transaction.LastTransactionInfoGrid.DataContext == LastTransactionInformation)
            {
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.BtnCancelUpdate.Visibility = Visibility.Visible;
                transaction.LTISearchControl.IsEnabled = false;
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
                var LastTransactionInfoToSave = (ERP_LastTransactionInfo)CollectionViewSource.GetDefaultView(transaction.LastTransactionInfoGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                transaction.DocAttribLTIDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                transaction.txt_LastTransactionSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                transaction.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(transaction.LastTransactionInfoGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (transaction.BtnAdd.IsChecked == true)
                {
                    if (!client.LastTransactionInfoExists(LastTransactionInfoToSave))
                    {
                        ApiAck ack = client.CreateLastTransactionInfo(LastTransactionInfoToSave);
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
                    ApiAck ack = client.UpdateLastTransactionInfo(LastTransactionInfoToSave);
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
                ObservableCollection<ERP_LastTransactionInfo> newCollection = new ObservableCollection<ERP_LastTransactionInfo>() { new ERP_LastTransactionInfo() };
                transaction.BtnSave.Visibility = Visibility.Visible;
                transaction.LastTransactionInfoGrid.DataContext = newCollection;
                transaction.LTISearchControl.IsEnabled = false;
                transaction.btnAdd_text.Text = "Cancel Add";
                transaction.btnSave_text.Text = "Confirm Add";
                transaction.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in transaction.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                transaction.DocAttribLTIDropDown.IsEnabled = true;
                transaction.txt_SubSystemCodeLTI.IsEnabled = true;
                transaction.txt_ProductCodeLTI.IsEnabled = true;
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
            transaction.LastTransactionInfoGrid.DataContext = LastTransactionInformation;
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.LTISearchControl.IsEnabled = true;
            transaction.btnAdd_text.Text = "Add";
            transaction.BtnAdd.IsChecked = false;
            transaction.btnSave_text.Text = "Update";
            transaction.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in transaction.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            transaction.DocAttribLTIDropDown.IsEnabled = false;
            transaction.txt_SubSystemCodeLTI.IsEnabled = false;
            transaction.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            LastTransactionInfoQuery query = new LastTransactionInfoQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new LastTransactionInfoQuery() { ProductCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new LastTransactionInfoQuery() { SubSystemCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new LastTransactionInfoQuery() { DocCode = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryLastTransactionInfoAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_LastTransactionInfo>(response.Result.ToList<ERP_LastTransactionInfo>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastTransactionInformation.IndexOf(transaction.LTISearchControl.ResultsGrid.SelectedItem as ERP_LastTransactionInfo);
            LastTransactionInformation[index] = EditingLastTransactionInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            transaction.BtnSave.Visibility = Visibility.Collapsed;
            transaction.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            transaction.LTISearchControl.IsEnabled = true;
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
                var ltitoremove = (ERP_LastTransactionInfo)CollectionViewSource.GetDefaultView(LastTransactionInformation).CurrentItem;

                if (client.LastTransactionInfoExists(ltitoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastTransactionInformation.Remove(ltitoremove);
                        ApiAck ack = client.DeleteLastTransactionInfo(ltitoremove);
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