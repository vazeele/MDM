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
    internal class EmployeeEIMWSModel
    {
        private ObservableCollection<HR_EMPLOYEE_EIM_WS> EmployeeEIMWSInformation; //These will be shown in the datagrid
        private Employee employee; //Reference to Employee Interface
        private HR_EMPLOYEE_EIM_WS EditingEmployeeEIMWSInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public EmployeeEIMWSModel(Employee employee)
        {
            this.employee = employee;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employee.EIMWSSearchControl.Search = Search;
            employee.EIMWSDropDown.Search = EmployeeModel.Search;
            employee.EIMWSSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmployeeEIMWS_SelectionChanged;
            employee.EmployeeEIMWSGrid.SourceUpdated += EmployeeEIMWSGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employee.EIMWSSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(employee.EIMWSSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeEIMWSInformation = employee.EIMWSSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_EIM_WS>;
            employee.EIMWSSearchControl.DataContext = EmployeeEIMWSInformation;
            if (EmployeeEIMWSInformation.Count > 0)
            {
                EditingEmployeeEIMWSInfo = EmployeeEIMWSInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmployeeEIMWS_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (employee.EIMWSSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeeEIMWSInfo = (employee.EIMWSSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_EIM_WS).Clone() as HR_EMPLOYEE_EIM_WS;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmployeeEIMWSGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employee.EmployeeEIMWSGrid.DataContext == EmployeeEIMWSInformation)
            {
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.BtnCancelUpdate.Visibility = Visibility.Visible;
                employee.EIMWSSearchControl.IsEnabled = false;
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
                var EmployeeEIMWSToSave = (HR_EMPLOYEE_EIM_WS)CollectionViewSource.GetDefaultView(employee.EmployeeEIMWSGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                employee.EIMWSDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                //employee.txt_LastEmployeeSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(employee.EmployeeEIMWSGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employee.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeEIMWSExists(EmployeeEIMWSToSave))
                    {
                        ApiAck ack = client.CreateEmployeeEIMWS(EmployeeEIMWSToSave);
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
                    ApiAck ack = client.UpdateEmployeeEIMWS(EmployeeEIMWSToSave);
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
                ObservableCollection<HR_EMPLOYEE_EIM_WS> newCollection = new ObservableCollection<HR_EMPLOYEE_EIM_WS>() { new HR_EMPLOYEE_EIM_WS() };
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.EmployeeEIMWSGrid.DataContext = newCollection;
                employee.EIMWSSearchControl.IsEnabled = false;
                employee.btnAdd_text.Text = "Cancel Add";
                employee.btnSave_text.Text = "Confirm Add";
                employee.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employee.EIMWSDropDown.IsEnabled = true;
                //employee.txt_SubSystemCodeLTI.IsEnabled = true;
                //employee.txt_ProductCodeLTI.IsEnabled = true;
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
            employee.EmployeeEIMWSGrid.DataContext = EmployeeEIMWSInformation;
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.EIMWSSearchControl.IsEnabled = true;
            employee.btnAdd_text.Text = "Add";
            employee.BtnAdd.IsChecked = false;
            employee.btnSave_text.Text = "Update";
            employee.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employee.EIMWSDropDown.IsEnabled = false;
            //employee.txt_SubSystemCodeLTI.IsEnabled = false;
            //employee.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeEIMWSQuery query = new EmployeeEIMWSQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeEIMWSQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeEIMWSQuery() { Payroll = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeEIMWSQuery() { Location = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeEIMWSAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_EIM_WS>(response.Result.ToList<HR_EMPLOYEE_EIM_WS>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = EmployeeEIMWSInformation.IndexOf(employee.EIMWSSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_EIM_WS);
            EmployeeEIMWSInformation[index] = EditingEmployeeEIMWSInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employee.EIMWSSearchControl.IsEnabled = true;
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
                var eeimwstoremove = (HR_EMPLOYEE_EIM_WS)CollectionViewSource.GetDefaultView(EmployeeEIMWSInformation).CurrentItem;

                if (client.EmployeeEIMWSExists(eeimwstoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeEIMWSInformation.Remove(eeimwstoremove);
                        ApiAck ack = client.DeleteEmployeeEIMWS(eeimwstoremove);
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