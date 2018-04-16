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
    class EmergencyContactModel
    {
        ObservableCollection<HR_EMP_EMERGENCY> EmergencyContactInformation; //These will be shown in the datagrid
        EmergencyContact EmergencyContact; //Reference to Transaction Interface
        HR_EMP_EMERGENCY EditingEmergencyContactInfomation; //Stores what we're editing right now, Incase we need to cancel the edit
        public EmergencyContactModel(EmergencyContact EmergencyContact)
        {
            this.EmergencyContact = EmergencyContact;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            EmergencyContact.EmergencyContactSearchControl.Search = Search;
            EmergencyContact.EmployeeNumberDropDown.Search = EmployeeModel.Search;             

            //transaction.cmb_TransactionTypLTI.ItemsSource = Helper.getItemSource(typeof(ERP_LastTransactionInfo), "TransactionType"); //Fill the combobox with the values of the [Values] attribute; See entity.cs
           // EmergencyContact.EmergencyContactSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_EmergencyContact_AutoGeneratingColumn;

            EmergencyContact.EmergencyContactSearchControl.ResultsGrid.SelectionChanged += dgv_EmergencyContact_SelectionChanged;

            EmergencyContact.EmergencyContactGrid.SourceUpdated += EmergencyContactGrid_SourceUpdated;            
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {                
                dpd.AddValueChanged(EmergencyContact.EmergencyContactSearchControl.ResultsGrid, ItemSourceChanged);               
            }
            
            Search(EmergencyContact.EmergencyContactSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            EmergencyContactInformation = EmergencyContact.EmergencyContactSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_EMERGENCY>;
            EmergencyContact.EmergencyContactGrid.DataContext = EmergencyContactInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_EmergencyContact_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMP_EMERGENCY).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }        
        //When the user selects a row in the datagrid
        void dgv_EmergencyContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmergencyContact.EmergencyContactSearchControl.ResultsGrid.SelectedItem!=null)
                EditingEmergencyContactInfomation = (EmergencyContact.EmergencyContactSearchControl.ResultsGrid.SelectedItem as HR_EMP_EMERGENCY).Clone() as HR_EMP_EMERGENCY;            
        }
        //If the user updates a record, then go to the Update mode automatically
        void EmergencyContactGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (EmergencyContact.EmergencyContactGrid.DataContext == EmergencyContactInformation)
            {
                EmergencyContact.BtnSave.Visibility = Visibility.Visible;
                EmergencyContact.BtnCancelUpdate.Visibility = Visibility.Visible;
                EmergencyContact.EmergencyContactSearchControl.IsEnabled = false;
                EmergencyContact.BtnAdd.Visibility = Visibility.Collapsed;
                EmergencyContact.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in EmergencyContact.TbPage.Items)
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
                var SaveEmergencyContactInformation = (HR_EMP_EMERGENCY)CollectionViewSource.GetDefaultView(EmergencyContact.EmergencyContactGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                EmergencyContact.txt_FullName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_Relationship.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_PermanentAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_OfficialAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneHome.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneMobile.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneOffice.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        
                //Checking if all controls are in valid state
                if (!Helper.IsValid(EmergencyContact.EmergencyContactGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (EmergencyContact.BtnAdd.IsChecked == true)
                {                                    
                    if (!client.ExistEmergency(SaveEmergencyContactInformation))
                    {
                        ApiAck ack = client.CreateEmergency(SaveEmergencyContactInformation);
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
                    ApiAck ack = client.UpdateEmergency(SaveEmergencyContactInformation);
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
            if (EmergencyContact.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_EMERGENCY>  newCollection = new ObservableCollection<HR_EMP_EMERGENCY>() { new HR_EMP_EMERGENCY()};
                EmergencyContact.BtnSave.Visibility = Visibility.Visible;
                EmergencyContact.EmergencyContactGrid.DataContext = newCollection;
                EmergencyContact.EmergencyContactSearchControl.IsEnabled = false;
                EmergencyContact.btnAdd_text.Text = "Cancel Add";
                EmergencyContact.btnSave_text.Text = "Confirm Add";
                EmergencyContact.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in EmergencyContact.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                EmergencyContact.EmployeeNumberDropDown.IsEnabled = true;                      
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
            EmergencyContact.EmergencyContactGrid.DataContext = EmergencyContactInformation;              
            EmergencyContact.BtnSave.Visibility = Visibility.Collapsed;
            EmergencyContact.EmergencyContactSearchControl.IsEnabled = true;
            EmergencyContact.btnAdd_text.Text = "Add";
            EmergencyContact.BtnAdd.IsChecked = false;
            EmergencyContact.btnSave_text.Text = "Update";
            EmergencyContact.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in EmergencyContact.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            EmergencyContact.EmployeeNumberDropDown.IsEnabled = false;           
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmergencyQuery query = new EmergencyQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmergencyQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }           
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmergencyAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_EMERGENCY>(response.Result.ToList<HR_EMP_EMERGENCY>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= EmergencyContactInformation.IndexOf(EmergencyContact.EmergencyContactSearchControl.ResultsGrid.SelectedItem as HR_EMP_EMERGENCY);
           EmergencyContactInformation [index]= EditingEmergencyContactInfomation;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            EmergencyContact.BtnSave.Visibility = Visibility.Collapsed;
            EmergencyContact.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            EmergencyContact.EmergencyContactSearchControl.IsEnabled = true;
            EmergencyContact.BtnAdd.Visibility = Visibility.Visible;
            EmergencyContact.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in EmergencyContact.TbPage.Items)
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
                var RemoveEmergencyContactInformation = (HR_EMP_EMERGENCY)CollectionViewSource.GetDefaultView(EmergencyContact.EmergencyContactGrid.DataContext).CurrentItem;        

                if (client.ExistEmergency(RemoveEmergencyContactInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmergencyContactInformation.Remove(RemoveEmergencyContactInformation);
                        ApiAck ack = client.DeleteEmergency(RemoveEmergencyContactInformation);
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

