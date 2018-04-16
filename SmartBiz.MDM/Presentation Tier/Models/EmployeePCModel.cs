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
    internal class EmployeePCModel
    {
        private ObservableCollection<HR_EMPLOYEE_PC> EmployeePCInformation; //These will be shown in the datagrid
        private Employee employee; //Reference to Employee Interface
        private HR_EMPLOYEE_PC EditingEmployeePCInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public EmployeePCModel(Employee employee)
        {
            this.employee = employee;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employee.PCSearchControl.Search = Search;
            employee.PCDropDown.Search = EmployeeModel.Search;
            employee.PCSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmployeePC_SelectionChanged;
            employee.EmployeePCGrid.SourceUpdated += EmployeePCGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employee.PCSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(employee.PCSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeePCInformation = employee.PCSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_PC>;
            employee.PCSearchControl.DataContext = EmployeePCInformation;
            if (EmployeePCInformation.Count > 0)
            {
                EditingEmployeePCInfo = EmployeePCInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmployeePC_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (employee.PCSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeePCInfo = (employee.PCSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_PC).Clone() as HR_EMPLOYEE_PC;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmployeePCGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employee.EmployeePCGrid.DataContext == EmployeePCInformation)
            {
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.BtnCancelUpdate.Visibility = Visibility.Visible;
                employee.PCSearchControl.IsEnabled = false;
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
                var EmployeePCToSave = (HR_EMPLOYEE_PC)CollectionViewSource.GetDefaultView(employee.EmployeePCGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                employee.PCDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                //employee.txt_LastEmployeeSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(employee.EmployeePCGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employee.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeePCExists(EmployeePCToSave))
                    {
                        ApiAck ack = client.CreateEmployeePC(EmployeePCToSave);
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
                    ApiAck ack = client.UpdateEmployeePC(EmployeePCToSave);
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
                ObservableCollection<HR_EMPLOYEE_PC> newCollection = new ObservableCollection<HR_EMPLOYEE_PC>() { new HR_EMPLOYEE_PC() };
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.EmployeePCGrid.DataContext = newCollection;
                employee.PCSearchControl.IsEnabled = false;
                employee.btnAdd_text.Text = "Cancel Add";
                employee.btnSave_text.Text = "Confirm Add";
                employee.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employee.PCDropDown.IsEnabled = true;
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
            employee.EmployeePCGrid.DataContext = EmployeePCInformation;
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.PCSearchControl.IsEnabled = true;
            employee.btnAdd_text.Text = "Add";
            employee.BtnAdd.IsChecked = false;
            employee.btnSave_text.Text = "Update";
            employee.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employee.PCDropDown.IsEnabled = false;
            //employee.txt_SubSystemCodeLTI.IsEnabled = false;
            //employee.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeePCQuery query = new EmployeePCQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeePCQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeePCQuery() { City = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeePCQuery() { Province = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeePCAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_PC>(response.Result.ToList<HR_EMPLOYEE_PC>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = EmployeePCInformation.IndexOf(employee.PCSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_PC);
            EmployeePCInformation[index] = EditingEmployeePCInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employee.PCSearchControl.IsEnabled = true;
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
                var epctoremove = (HR_EMPLOYEE_PC)CollectionViewSource.GetDefaultView(EmployeePCInformation).CurrentItem;

                if (client.EmployeePCExists(epctoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeePCInformation.Remove(epctoremove);
                        ApiAck ack = client.DeleteEmployeePC(epctoremove);
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