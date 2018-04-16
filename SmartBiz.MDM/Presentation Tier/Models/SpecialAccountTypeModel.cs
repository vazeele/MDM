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
    internal class SpecialAccountTypeModel
    {
        private ObservableCollection<FIN_SpecialAccountType> LastAccountInformation; //These will be shown in the datagrid
        private Accounts account; //Reference to Account Interface
        private FIN_SpecialAccountType EditingLastAccountInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public SpecialAccountTypeModel(Accounts account)
        {
            this.account = account;
            account.SpecialAccountTypeSearchControl.ResultsGrid.SelectedCellsChanged += dgv_SpecialAccountType_SelectionChanged;
            account.SpecialAccountTypeGrid.SourceUpdated += SpecialAccountTypeGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(account.SpecialAccountTypeSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(account.SpecialAccountTypeSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastAccountInformation = account.SpecialAccountTypeSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_SpecialAccountType>;
            account.SpecialAccountTypeGrid.DataContext = LastAccountInformation;
            if (LastAccountInformation.Count>0)
            {
                EditingLastAccountInfo = LastAccountInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_SpecialAccountType_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (account.SpecialAccountTypeSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastAccountInfo = (account.SpecialAccountTypeSearchControl.ResultsGrid.SelectedItem as FIN_SpecialAccountType).Clone() as FIN_SpecialAccountType;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void SpecialAccountTypeGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (account.SpecialAccountTypeGrid.DataContext == LastAccountInformation)
            {
                account.BtnSave.Visibility = Visibility.Visible;
                account.BtnCancelUpdate.Visibility = Visibility.Visible;
                account.SpecialAccountTypeSearchControl.IsEnabled = false;
                account.BtnAdd.Visibility = Visibility.Collapsed;
                account.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in account.TbPage.Items)
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
                var LastAccountInfoToSave = (FIN_SpecialAccountType)CollectionViewSource.GetDefaultView(account.SpecialAccountTypeGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                account.txt_ProfitLossType_TypeID.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                account.txt_ProfitLossType_Name.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(account.SpecialAccountTypeGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (account.BtnAdd.IsChecked == true)
                {
                    var ltitosave = (LastAccountInfoToSave);
                    if (!client.SpecialAccountTypeExists(ltitosave))
                    {
                        ApiAck ack = client.CreateSpecialAccountType(ltitosave);
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
                    ApiAck ack = client.UpdateSpecialAccountType(LastAccountInfoToSave);
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
            if (account.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_SpecialAccountType> newCollection = new ObservableCollection<FIN_SpecialAccountType>() { new FIN_SpecialAccountType() };
                account.BtnSave.Visibility = Visibility.Visible;
                account.SpecialAccountTypeGrid.DataContext = newCollection;
                account.SpecialAccountTypeSearchControl.IsEnabled = false;
                account.btnAdd_text.Text = "Cancel Add";
                account.btnSave_text.Text = "Confirm Add";
                account.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in account.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                account.txt_AccountType_AccountDesc.IsEnabled = true;
                account.txt_AccountType_AccountType.IsEnabled = true;
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
            account.SpecialAccountTypeGrid.DataContext = LastAccountInformation;
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.SpecialAccountTypeSearchControl.IsEnabled = true;
            account.btnAdd_text.Text = "Add";
            account.BtnAdd.IsChecked = false;
            account.btnSave_text.Text = "Update";
            account.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in account.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            account.txt_AccountType_AccountType.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            SpecialAccountQuery query = new SpecialAccountQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new SpecialAccountQuery() { TypeId = int.Parse(SearchControl.SearchTextBox.Text) };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QuerySpecialAccountTypeAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_SpecialAccountType>(response.Result.ToList<FIN_SpecialAccountType>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastAccountInformation.IndexOf(account.SpecialAccountTypeSearchControl.ResultsGrid.SelectedItem as FIN_SpecialAccountType);
            LastAccountInformation[index] = EditingLastAccountInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            account.SpecialAccountTypeSearchControl.IsEnabled = true;
            account.BtnAdd.Visibility = Visibility.Visible;
            account.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in account.TbPage.Items)
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
                var ltitoremove = (FIN_SpecialAccountType)CollectionViewSource.GetDefaultView(LastAccountInformation).CurrentItem;

                if (client.SpecialAccountTypeExists(ltitoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastAccountInformation.Remove(ltitoremove);
                        ApiAck ack = client.DeleteSpecialAccountType(ltitoremove);
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