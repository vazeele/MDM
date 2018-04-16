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
    class EmployeeJSModel
    {
        ObservableCollection<HR_EMPLOYEE_JS> EmployeeJSInformation; //These will be shown in the datagrid
        Employee employeejs; //Reference to Transaction Interface
        HR_EMPLOYEE_JS EditingEmployeeJS; //Stores what we're editing right now, Incase we need to cancel the edit
        public EmployeeJSModel(Employee employeejs)
        {
            this.employeejs = employeejs;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employeejs.JSSearchControl.Search = Search;
            employeejs.JSDropDown.Search = EmployeeModel.Search;
            employeejs.JS_C_DropDown.Search = JDCategoryModel.Search;
            employeejs.JS_S_DropDown.Search = JDCategoryModel.Search;
           // employeejs.JSSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_EmployeeJS_AutoGeneratingColumn;
            employeejs.JSSearchControl.ResultsGrid.SelectionChanged += dgv_EmployeeJS_SelectionChanged;
            employeejs.EmployeeJSGrid.SourceUpdated += EmployeeJSGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employeejs.JSSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(employeejs.JSSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeJSInformation = employeejs.JSSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_JS>;
            employeejs.EmployeeJSGrid.DataContext = EmployeeJSInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_EmployeeJS_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMPLOYEE_JS).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
        //When the user selects a row in the datagrid
        void dgv_EmployeeJS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employeejs.JSSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeeJS = (employeejs.JSSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_JS).Clone() as HR_EMPLOYEE_JS;
        }
        //If the user updates a record, then go to the Update mode automatically
        void EmployeeJSGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employeejs.EmployeeJSGrid.DataContext == EmployeeJSInformation)
            {
                employeejs.BtnSave.Visibility = Visibility.Visible;
                employeejs.BtnCancelUpdate.Visibility = Visibility.Visible;
                employeejs.JSSearchControl.IsEnabled = false;
                employeejs.BtnAdd.Visibility = Visibility.Collapsed;
                employeejs.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in employeejs.TbPage.Items)
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
                var EmployeeJSToSave = (HR_EMPLOYEE_JS)CollectionViewSource.GetDefaultView(employeejs.EmployeeJSGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
               
                //employeejs.JSDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();         
               
                //Checking if all controls are in valid state
                if (!Helper.IsValid(employeejs.EmployeeJSGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employeejs.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeJSExists(EmployeeJSToSave))
                    {
                        ApiAck ack = client.CreateEmployeeJS(EmployeeJSToSave);
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
                    ApiAck ack = client.UpdateEmployeeJS(EmployeeJSToSave);
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
            if (employeejs.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMPLOYEE_JS>  newCollection = new ObservableCollection<HR_EMPLOYEE_JS>() { new HR_EMPLOYEE_JS()};
                employeejs.BtnSave.Visibility = Visibility.Visible;
                employeejs.EmployeeJSGrid.DataContext = newCollection;
                employeejs.JSSearchControl.IsEnabled = false;
                employeejs.btnAdd_text.Text = "Cancel Add";
                employeejs.btnSave_text.Text = "Confirm Add";
                employeejs.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employeejs.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employeejs.JSDropDown.IsEnabled = true;
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
            employeejs.EmployeeJSGrid.DataContext = EmployeeJSInformation;              
            employeejs.BtnSave.Visibility = Visibility.Collapsed;
            employeejs.JSSearchControl.IsEnabled = true;
            employeejs.btnAdd_text.Text = "Add";
            employeejs.BtnAdd.IsChecked = false;
            employeejs.btnSave_text.Text = "Update";
            employeejs.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employeejs.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employeejs.JSDropDown.IsEnabled = false;
        }        
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeJSQuery query = new EmployeeJSQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeJSQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeJSQuery() { EmployeeType = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeJSQuery() { EmployeeCategory = SearchControl.SearchTextBox.Text };
               
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeJSAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_JS>(response.Result.ToList<HR_EMPLOYEE_JS>()); ;         
        }
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= EmployeeJSInformation.IndexOf(employeejs.JSSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_JS);
           EmployeeJSInformation [index]= EditingEmployeeJS;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            employeejs.BtnSave.Visibility = Visibility.Collapsed;
            employeejs.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employeejs.JSSearchControl.IsEnabled = true;
            employeejs.BtnAdd.Visibility = Visibility.Visible;
            employeejs.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in employeejs.TbPage.Items)
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
                var jstoremove = (HR_EMPLOYEE_JS)CollectionViewSource.GetDefaultView(EmployeeJSInformation).CurrentItem;

                if (client.EmployeeJSExists(jstoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeJSInformation.Remove(jstoremove);
                        ApiAck ack = client.DeleteEmployeeJS(jstoremove);
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

