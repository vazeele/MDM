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
    internal class BasicSalaryModel
    {
        private ObservableCollection<HR_EMP_BASICSALARY> BasicSalaryInformation; //These will be shown in the datagrid
        private BasicSalary BasicSalary; //Reference to Transaction Interface
        private HR_EMP_BASICSALARY EditingBasicSalaryInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public BasicSalaryModel(BasicSalary BasicSalary)
        {
            this.BasicSalary = BasicSalary;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            BasicSalary.BasicSalarySearchControl.Search = Search;
            BasicSalary.EmployeeNumberDropDown.Search = EmployeeModel.Search;
            BasicSalary.SalaryGradeCodeDropDown.Search = SalaryGradeModel.Search;
            BasicSalary.CurrencyIDDropDown.Search = CurrencyModel2.Search;

            BasicSalary.BasicSalarySearchControl.ResultsGrid.SelectedCellsChanged += dgv_BasicSalary_SelectionChanged;
            BasicSalary.BasicSalaryGrid.SourceUpdated += BasicSalaryGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(BasicSalary.BasicSalarySearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(BasicSalary.BasicSalarySearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            BasicSalaryInformation = BasicSalary.BasicSalarySearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_BASICSALARY>;
            BasicSalary.BasicSalaryGrid.DataContext = BasicSalaryInformation;
            if (BasicSalaryInformation.Count>0)
            {
                EditingBasicSalaryInfomation = BasicSalaryInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_BasicSalary_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (BasicSalary.BasicSalarySearchControl.ResultsGrid.SelectedItem != null)
                EditingBasicSalaryInfomation = (BasicSalary.BasicSalarySearchControl.ResultsGrid.SelectedItem as HR_EMP_BASICSALARY).Clone() as HR_EMP_BASICSALARY;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void BasicSalaryGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (BasicSalary.BasicSalaryGrid.DataContext == BasicSalaryInformation)
            {
                BasicSalary.BtnSave.Visibility = Visibility.Visible;
                BasicSalary.BtnCancelUpdate.Visibility = Visibility.Visible;
                BasicSalary.BasicSalarySearchControl.IsEnabled = false;
                BasicSalary.BtnAdd.Visibility = Visibility.Collapsed;
                BasicSalary.btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var SaveBasicSalaryInformation = (HR_EMP_BASICSALARY)CollectionViewSource.GetDefaultView(BasicSalary.BasicSalaryGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(BasicSalary.BasicSalaryGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (BasicSalary.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistBasicSalary(SaveBasicSalaryInformation))
                    {
                        ApiAck ack = client.CreateBasicSalary(SaveBasicSalaryInformation);
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
                    ApiAck ack = client.UpdateBasicSalary(SaveBasicSalaryInformation);
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
            if (BasicSalary.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_BASICSALARY> newCollection = new ObservableCollection<HR_EMP_BASICSALARY>() { new HR_EMP_BASICSALARY() };
                BasicSalary.BtnSave.Visibility = Visibility.Visible;
                BasicSalary.BasicSalaryGrid.DataContext = newCollection;
                BasicSalary.BasicSalarySearchControl.IsEnabled = false;
                BasicSalary.btnAdd_text.Text = "Cancel Add";
                BasicSalary.btnSave_text.Text = "Confirm Add";
                BasicSalary.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                BasicSalary.EmployeeNumberDropDown.IsEnabled = true;
                BasicSalary.SalaryGradeCodeDropDown.IsEnabled = true;
                BasicSalary.CurrencyIDDropDown.IsEnabled = true;
                BasicSalary.txt_Year.IsEnabled = true;
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
            BasicSalary.BasicSalaryGrid.DataContext = BasicSalaryInformation;
            BasicSalary.BtnSave.Visibility = Visibility.Collapsed;
            BasicSalary.BasicSalarySearchControl.IsEnabled = true;
            BasicSalary.btnAdd_text.Text = "Add";
            BasicSalary.BtnAdd.IsChecked = false;
            BasicSalary.btnSave_text.Text = "Update";
            BasicSalary.btnDelete.Visibility = System.Windows.Visibility.Visible;
            BasicSalary.EmployeeNumberDropDown.IsEnabled = false;
            BasicSalary.SalaryGradeCodeDropDown.IsEnabled = false;
            BasicSalary.CurrencyIDDropDown.IsEnabled = false;
            BasicSalary.txt_Year.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            BasicSalaryQuery query = new BasicSalaryQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new BasicSalaryQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new BasicSalaryQuery() { SalaryGradeCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new BasicSalaryQuery() { CurrencyID = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryBasicSalaryAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_BASICSALARY>(response.Result.ToList<HR_EMP_BASICSALARY>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = BasicSalaryInformation.IndexOf(BasicSalary.BasicSalarySearchControl.ResultsGrid.SelectedItem as HR_EMP_BASICSALARY);
            BasicSalaryInformation[index] = EditingBasicSalaryInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            BasicSalary.BtnSave.Visibility = Visibility.Collapsed;
            BasicSalary.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            BasicSalary.BasicSalarySearchControl.IsEnabled = true;
            BasicSalary.BtnAdd.Visibility = Visibility.Visible;
            BasicSalary.btnDelete.Visibility = Visibility.Visible;
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var RemoveBasicSalaryInformation = (HR_EMP_BASICSALARY)CollectionViewSource.GetDefaultView(BasicSalary.BasicSalaryGrid.DataContext).CurrentItem;

                if (client.ExistBasicSalary(RemoveBasicSalaryInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        BasicSalaryInformation.Remove(RemoveBasicSalaryInformation);
                        ApiAck ack = client.DeleteBasicSalary(RemoveBasicSalaryInformation);
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