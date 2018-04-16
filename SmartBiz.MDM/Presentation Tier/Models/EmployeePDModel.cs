using SmartBiz.MDM.Presentation.CustomControls;
using SmartBiz.MDM.Presentation.Models;
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
    internal class EmployeePDModel
    {
        private ObservableCollection<HR_EMPLOYEE_PD> EmployeePDInformation; //These will be shown in the datagrid
        private Employee employeepd; //Reference to Transaction Interface
        private HR_EMPLOYEE_PD EditingEmployeePD; //Stores what we're editing right now, Incase we need to cancel the edit

        public EmployeePDModel(Employee employeepd)
        {
            this.employeepd = employeepd;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            employeepd.PDSearchControl.Search = Search;
            employeepd.PDDropDown.Search = EmployeeModel.Search;
            employeepd.PD_N_DropDown.Search = NationalityModel.Search;
            employeepd.PD_R_DropDown.Search = ReligionModel.Search;
            employeepd.PDSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmployeePD_SelectionChanged;
            employeepd.EmployeePDGrid.SourceUpdated += EmployeePDGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(employeepd.PDSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(employeepd.PDSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            EmployeePDInformation = employeepd.PDSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMPLOYEE_PD>;
            employeepd.EmployeePDGrid.DataContext = EmployeePDInformation;
            if (EmployeePDInformation.Count>0)
            {
                EditingEmployeePD = EmployeePDInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmployeePD_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (employeepd.PDSearchControl.ResultsGrid.SelectedItem != null)
                EditingEmployeePD = (employeepd.PDSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_PD).Clone() as HR_EMPLOYEE_PD;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmployeePDGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (employeepd.EmployeePDGrid.DataContext == EmployeePDInformation)
            {
                employeepd.BtnSave.Visibility = Visibility.Visible;
                employeepd.BtnCancelUpdate.Visibility = Visibility.Visible;
                employeepd.PDSearchControl.IsEnabled = false;
                employeepd.BtnAdd.Visibility = Visibility.Collapsed;
                employeepd.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in employeepd.TbPage.Items)
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
                var EmployeePDToSave = (HR_EMPLOYEE_PD)CollectionViewSource.GetDefaultView(employeepd.EmployeePDGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(employeepd.EmployeePDGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (employeepd.BtnAdd.IsChecked == true)
                {
                    if (!client.EmployeePDExists(EmployeePDToSave))
                    {
                        ApiAck ack = client.CreateEmployeePD(EmployeePDToSave);
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
                    ApiAck ack = client.UpdateEmployeePD(EmployeePDToSave);
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
            if (employeepd.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMPLOYEE_PD> newCollection = new ObservableCollection<HR_EMPLOYEE_PD>() { new HR_EMPLOYEE_PD() };
                employeepd.BtnSave.Visibility = Visibility.Visible;
                employeepd.EmployeePDGrid.DataContext = newCollection;
                employeepd.PDSearchControl.IsEnabled = false;
                employeepd.btnAdd_text.Text = "Cancel Add";
                employeepd.btnSave_text.Text = "Confirm Add";
                employeepd.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in employeepd.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                employeepd.PDDropDown.IsEnabled = true;
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
            employeepd.EmployeePDGrid.DataContext = EmployeePDInformation;
            employeepd.BtnSave.Visibility = Visibility.Collapsed;
            employeepd.PDSearchControl.IsEnabled = true;
            employeepd.btnAdd_text.Text = "Add";
            employeepd.BtnAdd.IsChecked = false;
            employeepd.btnSave_text.Text = "Update";
            employeepd.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in employeepd.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            employeepd.PDDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmployeePDQuery query = new EmployeePDQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new EmployeePDQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new EmployeePDQuery() { Gender = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new EmployeePDQuery() { MaritalStatus = SearchControl.SearchTextBox.Text };
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEmployeePDAsync(query, pagesize, pagePosition);
            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMPLOYEE_PD>(response.Result.ToList<HR_EMPLOYEE_PD>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = EmployeePDInformation.IndexOf(employeepd.PDSearchControl.ResultsGrid.SelectedItem as HR_EMPLOYEE_PD);
            EmployeePDInformation[index] = EditingEmployeePD;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            employeepd.BtnSave.Visibility = Visibility.Collapsed;
            employeepd.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            employeepd.PDSearchControl.IsEnabled = true;
            employeepd.BtnAdd.Visibility = Visibility.Visible;
            employeepd.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in employeepd.TbPage.Items)
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
                var pdtoremove = (HR_EMPLOYEE_PD)CollectionViewSource.GetDefaultView(EmployeePDInformation).CurrentItem;

                if (client.EmployeePDExists(pdtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        EmployeePDInformation.Remove(pdtoremove);
                        ApiAck ack = client.DeleteEmployeePD(pdtoremove);
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