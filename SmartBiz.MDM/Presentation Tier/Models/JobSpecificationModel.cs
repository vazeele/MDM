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
    internal class JobSpecificationModel
    {
        private ObservableCollection<HR_EMP_JOBSPEC> JobSpecificationInformation; //These will be shown in the datagrid
        private JobSpecification JobSpecification; //Reference to Transaction Interface
        private HR_EMP_JOBSPEC EditingJobSpecificationInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public JobSpecificationModel(JobSpecification JobSpecification)
        {
            this.JobSpecification = JobSpecification;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            JobSpecification.JobSpecificationSearchControl.Search = Search;
            JobSpecification.EmployeeNumberDropDown.Search = EmployeeModel.Search;
            JobSpecification.JobDescriptionCategoryCodeDropDown.Search = JDCategoryModel.Search;

            JobSpecification.JobSpecificationSearchControl.ResultsGrid.SelectedCellsChanged += dgv_JobSpecification_SelectionChanged;
            JobSpecification.JobSpecificationGrid.SourceUpdated += JobSpecificationGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(JobSpecification.JobSpecificationSearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(JobSpecification.JobSpecificationSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            JobSpecificationInformation = JobSpecification.JobSpecificationSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_JOBSPEC>;
            JobSpecification.JobSpecificationGrid.DataContext = JobSpecificationInformation;
            if (JobSpecificationInformation.Count>0)
            {
                EditingJobSpecificationInfomation = JobSpecificationInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_JobSpecification_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (JobSpecification.JobSpecificationSearchControl.ResultsGrid.SelectedItem != null)
                EditingJobSpecificationInfomation = (JobSpecification.JobSpecificationSearchControl.ResultsGrid.SelectedItem as HR_EMP_JOBSPEC).Clone() as HR_EMP_JOBSPEC;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void JobSpecificationGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (JobSpecification.JobSpecificationGrid.DataContext == JobSpecificationInformation)
            {
                JobSpecification.BtnSave.Visibility = Visibility.Visible;
                JobSpecification.BtnCancelUpdate.Visibility = Visibility.Visible;
                JobSpecification.JobSpecificationSearchControl.IsEnabled = false;
                JobSpecification.BtnAdd.Visibility = Visibility.Collapsed;
                JobSpecification.btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var SaveJobSpecificationInformation = (HR_EMP_JOBSPEC)CollectionViewSource.GetDefaultView(JobSpecification.JobSpecificationGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(JobSpecification.JobSpecificationGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (JobSpecification.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistJobSpecification(SaveJobSpecificationInformation))
                    {
                        ApiAck ack = client.CreateJobSpecification(SaveJobSpecificationInformation);
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
                    ApiAck ack = client.UpdateJobSpecification(SaveJobSpecificationInformation);
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
            if (JobSpecification.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_JOBSPEC> newCollection = new ObservableCollection<HR_EMP_JOBSPEC>() { new HR_EMP_JOBSPEC() };
                JobSpecification.BtnSave.Visibility = Visibility.Visible;
                JobSpecification.JobSpecificationGrid.DataContext = newCollection;
                JobSpecification.JobSpecificationSearchControl.IsEnabled = false;
                JobSpecification.btnAdd_text.Text = "Cancel Add";
                JobSpecification.btnSave_text.Text = "Confirm Add";
                JobSpecification.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                JobSpecification.EmployeeNumberDropDown.IsEnabled = true;
                JobSpecification.JobDescriptionCategoryCodeDropDown.IsEnabled = true;
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
            JobSpecification.JobSpecificationGrid.DataContext = JobSpecificationInformation;
            JobSpecification.BtnSave.Visibility = Visibility.Collapsed;
            JobSpecification.JobSpecificationSearchControl.IsEnabled = true;
            JobSpecification.btnAdd_text.Text = "Add";
            JobSpecification.BtnAdd.IsChecked = false;
            JobSpecification.btnSave_text.Text = "Update";
            JobSpecification.btnDelete.Visibility = System.Windows.Visibility.Visible;
            JobSpecification.EmployeeNumberDropDown.IsEnabled = false;
            JobSpecification.JobDescriptionCategoryCodeDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            JobSpecificationQuery query = new JobSpecificationQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new JobSpecificationQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new JobSpecificationQuery() { JobDescriptionCategoryCode = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryJobSpecificationAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_JOBSPEC>(response.Result.ToList<HR_EMP_JOBSPEC>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = JobSpecificationInformation.IndexOf(JobSpecification.JobSpecificationSearchControl.ResultsGrid.SelectedItem as HR_EMP_JOBSPEC);
            JobSpecificationInformation[index] = EditingJobSpecificationInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            JobSpecification.BtnSave.Visibility = Visibility.Collapsed;
            JobSpecification.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            JobSpecification.JobSpecificationSearchControl.IsEnabled = true;
            JobSpecification.BtnAdd.Visibility = Visibility.Visible;
            JobSpecification.btnDelete.Visibility = Visibility.Visible;
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var RemoveJobSpecificationInformation = (HR_EMP_JOBSPEC)CollectionViewSource.GetDefaultView(JobSpecification.JobSpecificationGrid.DataContext).CurrentItem;

                if (client.ExistJobSpecification(RemoveJobSpecificationInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        JobSpecificationInformation.Remove(RemoveJobSpecificationInformation);
                        ApiAck ack = client.DeleteJobSpecification(RemoveJobSpecificationInformation);
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