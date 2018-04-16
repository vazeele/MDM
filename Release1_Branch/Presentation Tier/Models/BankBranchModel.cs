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
    class BankBranchModel
    {
        ObservableCollection<FIN_BankBranch> BankBranches; //These will be shown in the datagrid
        Bank bank; //Reference to Transaction Interface
        FIN_BankBranch EditingBankBranch; //Stores what we're editing right now, Incase we need to cancel the edit
        public BankBranchModel(Bank bank)
        {
            this.bank = bank;
             //The search control needs to know the method it has to use inorder to search. So we pass a method
            bank.BankBranchSearchControl.Search = Search;
            bank.BankBranchBankCodeDropDown.Search = BankModel.Search;
            bank.BankBranchAreaDropDown.Search = AreaModel.Search;

            //bank.BankBranchSearchControl.ResultsGrid.AutoGeneratingColumn += ResultsGrid_AutoGeneratingColumn;
            bank.BankBranchSearchControl.ResultsGrid.SelectionChanged += SearchControl_SelectionChanged;
            bank.BankBranchGrid.SourceUpdated += EditGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(bank.BankBranchSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(bank.BankBranchSearchControl);  //Initial Search to fill datagrid with values at startup         
        }

      

        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            BankBranches = bank.BankBranchSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_BankBranch>;
            bank.BankBranchGrid.DataContext = BankBranches;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void ResultsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(FIN_BankBranch).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
      
        //When the user selects a row in the datagrid
        void SearchControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bank.BankBranchSearchControl.ResultsGrid.SelectedItem != null)
                EditingBankBranch = (bank.BankBranchSearchControl.ResultsGrid.SelectedItem as FIN_BankBranch).Clone();
        }
        //If the user updates a record, then go to the Update mode automatically
        void EditGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (bank.BankBranchGrid.DataContext == BankBranches)
            {
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.BtnCancelUpdate.Visibility = Visibility.Visible;
                bank.BankBranchSearchControl.IsEnabled = false;
                bank.BtnAdd.Visibility = Visibility.Collapsed;
                bank.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in bank.TbPage.Items)
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
                var BankBranchToSave = (FIN_BankBranch)CollectionViewSource.GetDefaultView(bank.BankBranchGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                bank.BankBranchBankCodeDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                bank.txt_BankBranchCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();          

                //Checking if all controls are in valid state
                if (!Helper.IsValid(bank.BankBranchGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (bank.BtnAdd.IsChecked == true)
                {
                    
                    if (!client.BankBranchExists(BankBranchToSave))
                    {
                        ApiAck ack = client.CreateBankBranch(BankBranchToSave);
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
                    ApiAck ack = client.UpdateBankBranch(BankBranchToSave);
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
            if (bank.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_BankBranch>  newCollection = new ObservableCollection<FIN_BankBranch>() { new FIN_BankBranch()};
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.BankBranchGrid.DataContext = newCollection;
                bank.BankBranchSearchControl.IsEnabled = false;
                bank.btnAdd_text.Text = "Cancel Add";
                bank.btnSave_text.Text = "Confirm Add";
                bank.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in bank.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                bank.BankBranchBankCodeDropDown.IsEnabled = true;
                bank.txt_BankBranchCode.IsEnabled = true;
                     

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
            bank.BankBranchGrid.DataContext = BankBranches;              
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.BankBranchSearchControl.IsEnabled = true;
            bank.btnAdd_text.Text = "Add";
            bank.BtnAdd.IsChecked = false;
            bank.btnSave_text.Text = "Update";
            bank.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in bank.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            bank.BankBranchBankCodeDropDown.IsEnabled = false;
            bank.txt_BankBranchCode.IsEnabled = false;
           
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            var query = new BankBranchQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new BankBranchQuery() { BankCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new BankBranchQuery() { BankBranchCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new BankBranchQuery() { BranchName = SearchControl.SearchTextBox.Text };
            }
        
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryBankBranchAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_BankBranch>(response.Result.ToList<FIN_BankBranch>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= BankBranches.IndexOf(bank.BankBranchSearchControl.ResultsGrid.SelectedItem as FIN_BankBranch);
           BankBranches [index]= EditingBankBranch;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            bank.BankBranchSearchControl.IsEnabled = true;
            bank.BtnAdd.Visibility = Visibility.Visible;
            bank.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in bank.TbPage.Items)
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
                var bankbranchtoremove = (FIN_BankBranch)CollectionViewSource.GetDefaultView(BankBranches).CurrentItem;

                if (client.BankBranchExists(bankbranchtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        BankBranches.Remove(bankbranchtoremove);
                        ApiAck ack = client.DeleteBankBranch(bankbranchtoremove);
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

