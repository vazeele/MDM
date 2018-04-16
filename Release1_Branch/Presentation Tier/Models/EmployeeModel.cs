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
    class EmployeeModel
    {
        ObservableCollection<HR_EMPLOYEE> EmployeeInformation; //These will be shown in the datagrid
        Employee employee; //Reference to Transaction Interface
        HR_EMPLOYEE EditingEmployee; //Stores what we're editing right now, Incase we need to cancel the edit
        public EmployeeModel(Employee employee)
        {
            this.employee = employee;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employee.ESearchControl.Search = Search;
           // employee.ESearchControl.ResultsGrid.AutoGeneratingColumn += dgv_Employee_AutoGeneratingColumn;
            employee.ESearchControl.ResultsGrid.SelectionChanged += dgv_Employee_SelectionChanged;
            employee.EmployeeGrid.SourceUpdated += EmployeeGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employee.ESearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(employee.ESearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeInformation = employee.ESearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE>;
            employee.EmployeeGrid.DataContext = EmployeeInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_Employee_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMPLOYEE).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
        //When the user selects a row in the datagrid
        void dgv_Employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employee.ESearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployee = (employee.ESearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE).Clone() as HR_EMPLOYEE;
        }
        //If the user updates a record, then go to the Update mode automatically
        void EmployeeGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employee.EmployeeGrid.DataContext == EmployeeInformation)
            {
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.BtnCancelUpdate.Visibility = Visibility.Visible;
                employee.ESearchControl.IsEnabled = false;
                employee.BtnAdd.Visibility = Visibility.Collapsed;
                employee.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
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
                var EmployeeToSave = (HR_EMPLOYEE)CollectionViewSource.GetDefaultView(employee.EmployeeGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                employee.txt_EMP_NUMBER.GetBindingExpression(TextBox.TextProperty).UpdateSource();         
                //Checking if all controls are in valid state
                if (!Helper.IsValid(employee.EmployeeGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employee.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeExists(EmployeeToSave))
                    {
                        ApiAck ack = client.CreateEmployee(EmployeeToSave);
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
                    ApiAck ack = client.UpdateEmployee(EmployeeToSave);
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
            if (employee.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMPLOYEE>  newCollection = new ObservableCollection<HR_EMPLOYEE>() { new HR_EMPLOYEE()};
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.EmployeeGrid.DataContext = newCollection;
                employee.ESearchControl.IsEnabled = false;
                employee.btnAdd_text.Text = "Cancel Add";
                employee.btnSave_text.Text = "Confirm Add";
                employee.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employee.txt_EMP_NUMBER.IsEnabled = true;
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
            employee.EmployeeGrid.DataContext = EmployeeInformation;              
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.ESearchControl.IsEnabled = true;
            employee.btnAdd_text.Text = "Add";
            employee.BtnAdd.IsChecked = false;
            employee.btnSave_text.Text = "Update";
            employee.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employee.txt_EMP_NUMBER.IsEnabled = false;
        }        
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeQuery query = new EmployeeQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeQuery() { EmployeeTitle = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeQuery() { EmployeeCallingName = SearchControl.SearchTextBox.Text };
               
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE>(response.Result.ToList<HR_EMPLOYEE>()); ;         
        }
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= EmployeeInformation.IndexOf(employee.ESearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE);
           EmployeeInformation [index]= EditingEmployee;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employee.ESearchControl.IsEnabled = true;
            employee.BtnAdd.Visibility = Visibility.Visible;
            employee.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
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
                var etoremove = (HR_EMPLOYEE)CollectionViewSource.GetDefaultView(EmployeeInformation).CurrentItem;

                if (client.EmployeeExists(etoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeInformation.Remove(etoremove);
                        ApiAck ack = client.DeleteEmployee(etoremove);
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

