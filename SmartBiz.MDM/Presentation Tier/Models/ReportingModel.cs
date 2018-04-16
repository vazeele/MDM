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
    internal class ReportingModel
    {
        private ObservableCollection<HR_EMP_REPORTTO> ReportingInformation; //These will be shown in the datagrid
        private Reporting Reporting; //Reference to Transaction Interface
        private HR_EMP_REPORTTO EditingReportingInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public ReportingModel(Reporting Reporting)
        {
            this.Reporting = Reporting;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Reporting.ReportingSearchControl.Search = Search;
            Reporting.EmployeeNumberDropDown.Search = EmployeeModel.Search;
            Reporting.EmployeeNumberDropDown2.Search = EmployeeModel.Search;

            Reporting.cmb_Reportingmode.ItemsSource = Helper.getItemSource(typeof(HR_EMP_REPORTTO), "EREP_REPORTING_MODE");
            Reporting.ReportingSearchControl.ResultsGrid.SelectedCellsChanged += dgv_Reporting_SelectionChanged;
            Reporting.ReportingGrid.SourceUpdated += ReportingGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(Reporting.ReportingSearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(Reporting.ReportingSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            ReportingInformation = Reporting.ReportingSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_REPORTTO>;
            Reporting.ReportingGrid.DataContext = ReportingInformation;
            if (ReportingInformation.Count>0)
            {
                EditingReportingInfomation = ReportingInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Reporting_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Reporting.ReportingSearchControl.ResultsGrid.SelectedItem != null)
                EditingReportingInfomation = (Reporting.ReportingSearchControl.ResultsGrid.SelectedItem as HR_EMP_REPORTTO).Clone() as HR_EMP_REPORTTO;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void ReportingGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Reporting.ReportingGrid.DataContext == ReportingInformation)
            {
                Reporting.BtnSave.Visibility = Visibility.Visible;
                Reporting.BtnCancelUpdate.Visibility = Visibility.Visible;
                Reporting.ReportingSearchControl.IsEnabled = false;
                Reporting.BtnAdd.Visibility = Visibility.Collapsed;
                Reporting.btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var SaveReportingInformation = (HR_EMP_REPORTTO)CollectionViewSource.GetDefaultView(Reporting.ReportingGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(Reporting.ReportingGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Reporting.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistReporting(SaveReportingInformation))
                    {
                        ApiAck ack = client.CreateReporting(SaveReportingInformation);
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
                    ApiAck ack = client.UpdateReporting(SaveReportingInformation);
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
            if (Reporting.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_REPORTTO> newCollection = new ObservableCollection<HR_EMP_REPORTTO>() { new HR_EMP_REPORTTO() };
                Reporting.BtnSave.Visibility = Visibility.Visible;
                Reporting.ReportingGrid.DataContext = newCollection;
                Reporting.ReportingSearchControl.IsEnabled = false;
                Reporting.btnAdd_text.Text = "Cancel Add";
                Reporting.btnSave_text.Text = "Confirm Add";
                Reporting.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                Reporting.EmployeeNumberDropDown.IsEnabled = true;
                Reporting.EmployeeNumberDropDown2.IsEnabled = true;
                Reporting.cmb_Reportingmode.IsEnabled = true;
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
            Reporting.ReportingGrid.DataContext = ReportingInformation;
            Reporting.BtnSave.Visibility = Visibility.Collapsed;
            Reporting.ReportingSearchControl.IsEnabled = true;
            Reporting.btnAdd_text.Text = "Add";
            Reporting.BtnAdd.IsChecked = false;
            Reporting.btnSave_text.Text = "Update";
            Reporting.btnDelete.Visibility = System.Windows.Visibility.Visible;
            Reporting.EmployeeNumberDropDown.IsEnabled = false;
            Reporting.EmployeeNumberDropDown2.IsEnabled = false;
            Reporting.cmb_Reportingmode.IsEnabled =false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            ReportingQuery query = new ReportingQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new ReportingQuery() { SuperiorEmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new ReportingQuery() { SubordinateEmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new ReportingQuery() { ReportingMode = Int16.Parse(SearchControl.SearchTextBox.Text) };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryReportingAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_REPORTTO>(response.Result.ToList<HR_EMP_REPORTTO>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = ReportingInformation.IndexOf(Reporting.ReportingSearchControl.ResultsGrid.SelectedItem as HR_EMP_REPORTTO);
            ReportingInformation[index] = EditingReportingInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            Reporting.BtnSave.Visibility = Visibility.Collapsed;
            Reporting.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Reporting.ReportingSearchControl.IsEnabled = true;
            Reporting.BtnAdd.Visibility = Visibility.Visible;
            Reporting.btnDelete.Visibility = Visibility.Visible;
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var RemoveReportingInformation = (HR_EMP_REPORTTO)CollectionViewSource.GetDefaultView(Reporting.ReportingGrid.DataContext).CurrentItem;

                if (client.ExistReporting(RemoveReportingInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        ReportingInformation.Remove(RemoveReportingInformation);
                        ApiAck ack = client.DeleteReporting(RemoveReportingInformation);
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