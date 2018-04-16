﻿using System;
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
    class AccountTypeModel
    {
        ObservableCollection<FIN_AccountType> LastAccountInformation; //These will be shown in the datagrid
        Accounts account; //Reference to Account Interface
        FIN_AccountType EditingLastAccountInfo; //Stores what we're editing right now, Incase we need to cancel the edit
        public AccountTypeModel(Accounts account)
        {
            this.account = account;
            //To hide unnecessary columns
            //account.AccountTypeSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_AccountType_AutoGeneratingColumn;
            account.AccountTypeSearchControl.ResultsGrid.SelectionChanged += dgv_AccountType_SelectionChanged;
            account.AccountTypeGrid.SourceUpdated += AccountTypeGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(account.AccountTypeSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(account.AccountTypeSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            LastAccountInformation  = account.AccountTypeSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_AccountType>;
            account.AccountTypeGrid.DataContext = LastAccountInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_AccountType_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(FIN_AccountType).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }

        //Fill Datagrid of the ForeignKeyDropDown when it's dropped down
        //void CustomCombo_DropDownOpened(object sender, EventArgs e)
        //{
        //    DocumentAttributesModel.DocAttributesSearch(transaction.DocAttribLTIDropDown.SearchControl);
        //}
        //Search button of the ForeignKey drop down
        //public  void btn_Search_Click(object sender, RoutedEventArgs e)
        //{
        //    transaction.DocAttribLTIDropDown.ResetPager();
        //    DocumentAttributesModel.DocAttributesSearch(transaction.DocAttribLTIDropDown.SearchControl);
        //} 
        //When the user selects a row in the datagrid
        void dgv_AccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (account.AccountTypeSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastAccountInfo = (account.AccountTypeSearchControl.ResultsGrid.SelectedItem as FIN_AccountType).Clone() as FIN_AccountType;
        }
        //If the user updates a record, then go to the Update mode automatically
        void AccountTypeGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (account.AccountTypeGrid.DataContext == LastAccountInformation)
            {
                account.BtnSave.Visibility = Visibility.Visible;
                account.BtnCancelUpdate.Visibility = Visibility.Visible;
                account.AccountTypeSearchControl.IsEnabled = false;
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
                var LastAccountInfoToSave = (FIN_AccountType)CollectionViewSource.GetDefaultView(account.AccountTypeGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                account.txt_AccountType_AccountType .GetBindingExpression(TextBox.TextProperty).UpdateSource();
                account.txt_AccountType_AccountDesc.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                      

                //Checking if all controls are in valid state
                if (!Helper.IsValid(account.AccountTypeGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (account.BtnAdd.IsChecked == true)
                {
                    
                    var ltitosave = (LastAccountInfoToSave);
                    if (!client.AccountTypeExists(ltitosave))
                    {
                        ApiAck ack = client.CreateAccountType(ltitosave);
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
                    ApiAck ack = client.UpdateAccountType(LastAccountInfoToSave);
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
                ObservableCollection<FIN_AccountType>  newCollection = new ObservableCollection<FIN_AccountType>() { new FIN_AccountType()};
                account.BtnSave.Visibility = Visibility.Visible;
                account.AccountTypeGrid.DataContext = newCollection;
                account.AccountTypeSearchControl.IsEnabled = false;
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
            //If user cancels the add  , renable textboxes,grid etc;
            else
            {
                CompleteAdd();
            }
        }
        //Code for renabling tabs,textboxes etc; when an add operation finishes
        private void CompleteAdd()
        {   //Bind the original collection when the Add operation is complete       
            account.AccountTypeGrid.DataContext = LastAccountInformation;
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.AccountTypeSearchControl.IsEnabled = true;
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
            AccountTypeQuery query = new AccountTypeQuery(); //by default we have an empty query
             if(SearchControl.OptionOne.IsChecked==true)          
               query = new AccountTypeQuery() { AccountType = int.Parse(SearchControl.SearchTextBox.Text) };
            
            
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAccountTypeAsync(query, pagesize, pagePosition);

          //  var response = client.ReadAllAccountType(pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_AccountType>(response.Result.ToList<FIN_AccountType>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= LastAccountInformation.IndexOf(account.AccountTypeSearchControl.ResultsGrid.SelectedItem as FIN_AccountType);
           LastAccountInformation [index]= EditingLastAccountInfo;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            account.BtnSave.Visibility = Visibility.Collapsed;
            account.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            account.AccountTypeSearchControl.IsEnabled = true;
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
                var ltitoremove = (FIN_AccountType)CollectionViewSource.GetDefaultView(LastAccountInformation).CurrentItem;        

                if (client.AccountTypeExists(ltitoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastAccountInformation.Remove(ltitoremove);
                        ApiAck ack = client.DeleteAccountType(ltitoremove);
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

