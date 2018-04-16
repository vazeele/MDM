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
    class WarningModel
    {
        ObservableCollection<HR_EMP_WARNINGS> WarningInformation; //These will be shown in the datagrid
        Warning Warning; //Reference to Transaction Interface
        HR_EMP_WARNINGS EditingWarningInfomation; //Stores what we're editing right now, Incase we need to cancel the edit
        public WarningModel(Warning Warning)
        {
            this.Warning = Warning;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Warning.WarningSearchControl.Search = Search;
            Warning.EmployeeNumberDropDown.Search = EmployeeModel.Search;

           // Warning.WarningSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_Warning_AutoGeneratingColumn;

            Warning.WarningSearchControl.ResultsGrid.SelectionChanged += dgv_Warning_SelectionChanged;

            Warning.WarningGrid.SourceUpdated += WarningGrid_SourceUpdated;            
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {                
                dpd.AddValueChanged(Warning.WarningSearchControl.ResultsGrid, ItemSourceChanged);               
            }
            
            Search(Warning.WarningSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            WarningInformation = Warning.WarningSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_WARNINGS>;
            Warning.WarningGrid.DataContext = WarningInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_Warning_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMP_WARNINGS).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }        
        //When the user selects a row in the datagrid
        void dgv_Warning_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Warning.WarningSearchControl.ResultsGrid.SelectedItem!=null)
                EditingWarningInfomation = (Warning.WarningSearchControl.ResultsGrid.SelectedItem as HR_EMP_WARNINGS).Clone() as HR_EMP_WARNINGS;            
        }
        //If the user updates a record, then go to the Update mode automatically
        void WarningGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Warning.WarningGrid.DataContext == WarningInformation)
            {
                Warning.BtnSave.Visibility = Visibility.Visible;
                Warning.BtnCancelUpdate.Visibility = Visibility.Visible;
                Warning.WarningSearchControl.IsEnabled = false;
                Warning.BtnAdd.Visibility = Visibility.Collapsed;
                Warning.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in Warning.TbPage.Items)
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
                var SaveWarningInformation = (HR_EMP_WARNINGS)CollectionViewSource.GetDefaultView(Warning.WarningGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                /*EmergencyContact.txt_FullName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_Relationship.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_PermanentAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_OfficialAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneHome.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneMobile.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneOffice.GetBindingExpression(TextBox.TextProperty).UpdateSource();*/
                        
                //Checking if all controls are in valid state
                if (!Helper.IsValid(Warning.WarningGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Warning.BtnAdd.IsChecked == true)
                {                                    
                    if (!client.ExistWarning(SaveWarningInformation))
                    {
                        ApiAck ack = client.CreateWarning(SaveWarningInformation);
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
                    ApiAck ack = client.UpdateWarning(SaveWarningInformation);
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
            if (Warning.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_WARNINGS>  newCollection = new ObservableCollection<HR_EMP_WARNINGS>() { new HR_EMP_WARNINGS()};
                Warning.BtnSave.Visibility = Visibility.Visible;
                Warning.WarningGrid.DataContext = newCollection;
                Warning.WarningSearchControl.IsEnabled = false;
                Warning.btnAdd_text.Text = "Cancel Add";
                Warning.btnSave_text.Text = "Confirm Add";
                Warning.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Warning.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Warning.EmployeeNumberDropDown.IsEnabled = true;
                Warning.txt_WarningID.IsEnabled = true;     
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
            Warning.WarningGrid.DataContext = WarningInformation;
            Warning.BtnSave.Visibility = Visibility.Collapsed;
            Warning.WarningSearchControl.IsEnabled = true;
            Warning.btnAdd_text.Text = "Add";
            Warning.BtnAdd.IsChecked = false;
            Warning.btnSave_text.Text = "Update";
            Warning.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Warning.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            Warning.EmployeeNumberDropDown.IsEnabled = false;
            Warning.txt_WarningID.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            WarningQuery query = new WarningQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new WarningQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new WarningQuery() { WarningID = Int32.Parse(SearchControl.SearchTextBox.Text) };
            } 
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryWarningAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_WARNINGS>(response.Result.ToList<HR_EMP_WARNINGS>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= WarningInformation.IndexOf(Warning.WarningSearchControl.ResultsGrid.SelectedItem as HR_EMP_WARNINGS);
           WarningInformation[index] = EditingWarningInfomation;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            Warning.BtnSave.Visibility = Visibility.Collapsed;
            Warning.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Warning.WarningSearchControl.IsEnabled = true;
            Warning.BtnAdd.Visibility = Visibility.Visible;
            Warning.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in Warning.TbPage.Items)
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
                var RemoveWarningInformation = (HR_EMP_WARNINGS)CollectionViewSource.GetDefaultView(Warning.WarningGrid.DataContext).CurrentItem;        

                if (client.ExistWarning(RemoveWarningInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        WarningInformation.Remove(RemoveWarningInformation);
                        ApiAck ack = client.DeleteWarning(RemoveWarningInformation);
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

