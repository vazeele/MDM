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
    internal class EmployeeOCModel
    {
        private ObservableCollection<HR_EMPLOYEE_OC> EmployeeOCInformation; //These will be shown in the datagrid
        private Employee employee; //Reference to Employee Interface
        private HR_EMPLOYEE_OC EditingEmployeeOCInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public EmployeeOCModel(Employee employee)
        {
            this.employee = employee;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employee.OCSearchControl.Search = Search;
            employee.OCDropDown.Search = EmployeeModel.Search;
            employee.OCSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmployeeOC_SelectionChanged;
            employee.EmployeeOCGrid.SourceUpdated += EmployeeOCGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employee.OCSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(employee.OCSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeOCInformation = employee.OCSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_OC>;
            employee.OCSearchControl.DataContext = EmployeeOCInformation;
            if (EmployeeOCInformation.Count > 0)
            {
                EditingEmployeeOCInfo = EmployeeOCInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmployeeOC_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (employee.OCSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeeOCInfo = (employee.OCSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_OC).Clone() as HR_EMPLOYEE_OC;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmployeeOCGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employee.EmployeeOCGrid.DataContext == EmployeeOCInformation)
            {
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.BtnCancelUpdate.Visibility = Visibility.Visible;
                employee.OCSearchControl.IsEnabled = false;
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
                var EmployeeOCToSave = (HR_EMPLOYEE_OC)CollectionViewSource.GetDefaultView(employee.EmployeeOCGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                employee.OCDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                //employee.txt_LastEmployeeSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(employee.EmployeeOCGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employee.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeOCExists(EmployeeOCToSave))
                    {
                        ApiAck ack = client.CreateEmployeeOC(EmployeeOCToSave);
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
                    ApiAck ack = client.UpdateEmployeeOC(EmployeeOCToSave);
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
                ObservableCollection<HR_EMPLOYEE_OC> newCollection = new ObservableCollection<HR_EMPLOYEE_OC>() { new HR_EMPLOYEE_OC() };
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.EmployeeOCGrid.DataContext = newCollection;
                employee.OCSearchControl.IsEnabled = false;
                employee.btnAdd_text.Text = "Cancel Add";
                employee.btnSave_text.Text = "Confirm Add";
                employee.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employee.OCDropDown.IsEnabled = true;
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
            employee.EmployeeOCGrid.DataContext = EmployeeOCInformation;
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.OCSearchControl.IsEnabled = true;
            employee.btnAdd_text.Text = "Add";
            employee.BtnAdd.IsChecked = false;
            employee.btnSave_text.Text = "Update";
            employee.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employee.OCDropDown.IsEnabled = false;
            //employee.txt_SubSystemCodeLTI.IsEnabled = false;
            //employee.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeOCQuery query = new EmployeeOCQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeOCQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeOCQuery() { OfficeExtension = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeOCQuery() { Phone = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeOCAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_OC>(response.Result.ToList<HR_EMPLOYEE_OC>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = EmployeeOCInformation.IndexOf(employee.OCSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_OC);
            EmployeeOCInformation[index] = EditingEmployeeOCInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employee.OCSearchControl.IsEnabled = true;
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
                var eoctoremove = (HR_EMPLOYEE_OC)CollectionViewSource.GetDefaultView(EmployeeOCInformation).CurrentItem;

                if (client.EmployeeOCExists(eoctoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeOCInformation.Remove(eoctoremove);
                        ApiAck ack = client.DeleteEmployeeOC(eoctoremove);
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