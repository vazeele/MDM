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
    class EmployeeJIModel
    {
        ObservableCollection<HR_EMPLOYEE_JI> EmployeeJIInformation; //These will be shown in the datagrid
        Employee employeeji; //Reference to Transaction Interface
        HR_EMPLOYEE_JI EditingEmployeeJI; //Stores what we're editing right now, Incase we need to cancel the edit
        public EmployeeJIModel(Employee employeeji)
        {
            this.employeeji = employeeji;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employeeji.JISearchControl.Search = Search;
            employeeji.JIDropDown.Search = EmployeeModel.Search;
            employeeji.JI_C_DropDown.Search = CorporateTitleModel.Search;
            employeeji.JI_D_DropDown.Search = DesignationModel.Search;
            employeeji.JI_SAL_GRD_DropDown.Search = SalaryGradeModel.Search;
            //employeeji.JISearchControl.ResultsGrid.AutoGeneratingColumn += dgv_EmployeeJI_AutoGeneratingColumn;
            employeeji.JISearchControl.ResultsGrid.SelectionChanged += dgv_EmployeeJI_SelectionChanged;
            employeeji.EmployeeJIGrid.SourceUpdated += EmployeeJIGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employeeji.JISearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(employeeji.JISearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeJIInformation = employeeji.JISearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_JI>;
            employeeji.EmployeeJIGrid.DataContext = EmployeeJIInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_EmployeeJI_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMPLOYEE_JI).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
        //When the user selects a row in the datagrid
        void dgv_EmployeeJI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employeeji.JISearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeeJI = (employeeji.JISearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_JI).Clone() as HR_EMPLOYEE_JI;
        }
        //If the user updates a record, then go to the Update mode automatically
        void EmployeeJIGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employeeji.EmployeeJIGrid.DataContext == EmployeeJIInformation)
            {
                employeeji.BtnSave.Visibility = Visibility.Visible;
                employeeji.BtnCancelUpdate.Visibility = Visibility.Visible;
                employeeji.JISearchControl.IsEnabled = false;
                employeeji.BtnAdd.Visibility = Visibility.Collapsed;
                employeeji.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in employeeji.TbPage.Items)
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
                var EmployeeJIToSave = (HR_EMPLOYEE_JI)CollectionViewSource.GetDefaultView(employeeji.EmployeeJIGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
               
                //employeeji.JIDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();         
               
                //Checking if all controls are in valid state
                if (!Helper.IsValid(employeeji.EmployeeJIGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employeeji.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeJIExists(EmployeeJIToSave))
                    {
                        ApiAck ack = client.CreateEmployeeJI(EmployeeJIToSave);
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
                    ApiAck ack = client.UpdateEmployeeJI(EmployeeJIToSave);
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
            if (employeeji.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMPLOYEE_JI>  newCollection = new ObservableCollection<HR_EMPLOYEE_JI>() { new HR_EMPLOYEE_JI()};
                employeeji.BtnSave.Visibility = Visibility.Visible;
                employeeji.EmployeeJIGrid.DataContext = newCollection;
                employeeji.JISearchControl.IsEnabled = false;
                employeeji.btnAdd_text.Text = "Cancel Add";
                employeeji.btnSave_text.Text = "Confirm Add";
                employeeji.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employeeji.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employeeji.JIDropDown.IsEnabled = true;
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
            employeeji.EmployeeJIGrid.DataContext = EmployeeJIInformation;              
            employeeji.BtnSave.Visibility = Visibility.Collapsed;
            employeeji.JISearchControl.IsEnabled = true;
            employeeji.btnAdd_text.Text = "Add";
            employeeji.BtnAdd.IsChecked = false;
            employeeji.btnSave_text.Text = "Update";
            employeeji.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employeeji.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employeeji.JIDropDown.IsEnabled = false;
        }        
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeJIQuery query = new EmployeeJIQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeJIQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeJIQuery() { WorkingHours = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeJIQuery() { Confirm = int.Parse(SearchControl.SearchTextBox.Text) };
               
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeJIAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_JI>(response.Result.ToList<HR_EMPLOYEE_JI>()); ;         
        }
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= EmployeeJIInformation.IndexOf(employeeji.JISearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_JI);
           EmployeeJIInformation [index]= EditingEmployeeJI;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            employeeji.BtnSave.Visibility = Visibility.Collapsed;
            employeeji.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employeeji.JISearchControl.IsEnabled = true;
            employeeji.BtnAdd.Visibility = Visibility.Visible;
            employeeji.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in employeeji.TbPage.Items)
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
                var jitoremove = (HR_EMPLOYEE_JI)CollectionViewSource.GetDefaultView(EmployeeJIInformation).CurrentItem;

                if (client.EmployeeJIExists(jitoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeJIInformation.Remove(jitoremove);
                        ApiAck ack = client.DeleteEmployeeJI(jitoremove);
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

