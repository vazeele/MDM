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
    internal class EmployeeTDModel
    {
        private ObservableCollection<HR_EMPLOYEE_TD> EmployeeTDInformation; //These will be shown in the datagrid
        private Employee employee; //Reference to Employee Interface
        private HR_EMPLOYEE_TD EditingEmployeeTDInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public EmployeeTDModel(Employee employee)
        {
            this.employee = employee;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employee.TDSearchControl.Search = Search;
            employee.TDDropDown.Search = EmployeeModel.Search;
            employee.TDSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmployeeTD_SelectionChanged;
            employee.EmployeeTDGrid.SourceUpdated += EmployeeTDGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employee.TDSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(employee.TDSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeeTDInformation = employee.TDSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_TD>;
            employee.TDSearchControl.DataContext = EmployeeTDInformation;
            if (EmployeeTDInformation.Count > 0)
            {
                EditingEmployeeTDInfo = EmployeeTDInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmployeeTD_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (employee.TDSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeeTDInfo = (employee.TDSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_TD).Clone() as HR_EMPLOYEE_TD;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmployeeTDGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employee.EmployeeTDGrid.DataContext == EmployeeTDInformation)
            {
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.BtnCancelUpdate.Visibility = Visibility.Visible;
                employee.TDSearchControl.IsEnabled = false;
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
                var EmployeeTDToSave = (HR_EMPLOYEE_TD)CollectionViewSource.GetDefaultView(employee.EmployeeTDGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                employee.TDDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                //employee.txt_LastEmployeeSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //employee.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(employee.EmployeeTDGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employee.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeeTDExists(EmployeeTDToSave))
                    {
                        ApiAck ack = client.CreateEmployeeTD(EmployeeTDToSave);
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
                    ApiAck ack = client.UpdateEmployeeTD(EmployeeTDToSave);
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
                ObservableCollection<HR_EMPLOYEE_TD> newCollection = new ObservableCollection<HR_EMPLOYEE_TD>() { new HR_EMPLOYEE_TD() };
                employee.BtnSave.Visibility = Visibility.Visible;
                employee.EmployeeTDGrid.DataContext = newCollection;
                employee.TDSearchControl.IsEnabled = false;
                employee.btnAdd_text.Text = "Cancel Add";
                employee.btnSave_text.Text = "Confirm Add";
                employee.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employee.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employee.TDDropDown.IsEnabled = true;
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
            employee.EmployeeTDGrid.DataContext = EmployeeTDInformation;
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.TDSearchControl.IsEnabled = true;
            employee.btnAdd_text.Text = "Add";
            employee.BtnAdd.IsChecked = false;
            employee.btnSave_text.Text = "Update";
            employee.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employee.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employee.TDDropDown.IsEnabled = false;
            //employee.txt_SubSystemCodeLTI.IsEnabled = false;
            //employee.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeeTDQuery query = new EmployeeTDQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeeTDQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeeTDQuery() { EmployeeExempt = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeeTDQuery() { EmployeeFlag = int.Parse(SearchControl.SearchTextBox.Text) };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeeTDAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_TD>(response.Result.ToList<HR_EMPLOYEE_TD>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = EmployeeTDInformation.IndexOf(employee.TDSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_TD);
            EmployeeTDInformation[index] = EditingEmployeeTDInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            employee.BtnSave.Visibility = Visibility.Collapsed;
            employee.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employee.TDSearchControl.IsEnabled = true;
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
                var etdtoremove = (HR_EMPLOYEE_TD)CollectionViewSource.GetDefaultView(EmployeeTDInformation).CurrentItem;

                if (client.EmployeeTDExists(etdtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeeTDInformation.Remove(etdtoremove);
                        ApiAck ack = client.DeleteEmployeeTD(etdtoremove);
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