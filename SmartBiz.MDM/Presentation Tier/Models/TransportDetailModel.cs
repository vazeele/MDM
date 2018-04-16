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
    internal class TransportDetailModel
    {
        private ObservableCollection<HR_EMP_TRANSPORT> TransportDetailInformation; //These will be shown in the datagrid
        private TransportDetail TransportDetail; //Reference to Transaction Interface
        private HR_EMP_TRANSPORT EditingTransportDetailInfomation; //Stores what we're editing right now, Incase we need to cancel the edit

        public TransportDetailModel(TransportDetail TransportDetail)
        {
            this.TransportDetail = TransportDetail;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            TransportDetail.TransportDetailSearchControl.Search = Search;
            TransportDetail.EmployeeNumberDropDown.Search = EmployeeModel.Search;
            TransportDetail.cmb_CommutingMode.ItemsSource = Helper.getItemSource(typeof(HR_EMP_TRANSPORT), "ETPORT_COMMUTING_MODE");
            TransportDetail.TransportDetailSearchControl.ResultsGrid.SelectedCellsChanged += dgv_TransportDetail_SelectionChanged;
            TransportDetail.TransportDetailGrid.SourceUpdated += TransportDetailGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(TransportDetail.TransportDetailSearchControl.ResultsGrid, ItemSourceChanged);
            }

            Search(TransportDetail.TransportDetailSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            TransportDetailInformation = TransportDetail.TransportDetailSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_TRANSPORT>;
            TransportDetail.TransportDetailGrid.DataContext = TransportDetailInformation;
            if (TransportDetailInformation.Count>0)
            {
                EditingTransportDetailInfomation = TransportDetailInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_TransportDetail_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TransportDetail.TransportDetailSearchControl.ResultsGrid.SelectedItem != null)
                EditingTransportDetailInfomation = (TransportDetail.TransportDetailSearchControl.ResultsGrid.SelectedItem as HR_EMP_TRANSPORT).Clone() as HR_EMP_TRANSPORT;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void TransportDetailGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (TransportDetail.TransportDetailGrid.DataContext == TransportDetailInformation)
            {
                TransportDetail.BtnSave.Visibility = Visibility.Visible;
                TransportDetail.BtnCancelUpdate.Visibility = Visibility.Visible;
                TransportDetail.TransportDetailSearchControl.IsEnabled = false;
                TransportDetail.BtnAdd.Visibility = Visibility.Collapsed;
                TransportDetail.btnDelete.Visibility = Visibility.Collapsed;
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var SaveTransportDetailInformation = (HR_EMP_TRANSPORT)CollectionViewSource.GetDefaultView(TransportDetail.TransportDetailGrid.DataContext).CurrentItem;

                //Checking if all controls are in valid state
                if (!Helper.IsValid(TransportDetail.TransportDetailGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (TransportDetail.BtnAdd.IsChecked == true)
                {
                    if (!client.ExistTransport(SaveTransportDetailInformation))
                    {
                        ApiAck ack = client.CreateTransport(SaveTransportDetailInformation);
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
                    ApiAck ack = client.UpdateTransport(SaveTransportDetailInformation);
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
            if (TransportDetail.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_TRANSPORT> newCollection = new ObservableCollection<HR_EMP_TRANSPORT>() { new HR_EMP_TRANSPORT() };
                TransportDetail.BtnSave.Visibility = Visibility.Visible;
                TransportDetail.TransportDetailGrid.DataContext = newCollection;
                TransportDetail.TransportDetailSearchControl.IsEnabled = false;
                TransportDetail.btnAdd_text.Text = "Cancel Add";
                TransportDetail.btnSave_text.Text = "Confirm Add";
                TransportDetail.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                TransportDetail.EmployeeNumberDropDown.IsEnabled = true;
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
            TransportDetail.TransportDetailGrid.DataContext = TransportDetailInformation;
            TransportDetail.BtnSave.Visibility = Visibility.Collapsed;
            TransportDetail.TransportDetailSearchControl.IsEnabled = true;
            TransportDetail.btnAdd_text.Text = "Add";
            TransportDetail.BtnAdd.IsChecked = false;
            TransportDetail.btnSave_text.Text = "Update";
            TransportDetail.btnDelete.Visibility = System.Windows.Visibility.Visible;
            TransportDetail.EmployeeNumberDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            TransportQuery query = new TransportQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new TransportQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryTransportAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_TRANSPORT>(response.Result.ToList<HR_EMP_TRANSPORT>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = TransportDetailInformation.IndexOf(TransportDetail.TransportDetailSearchControl.ResultsGrid.SelectedItem as HR_EMP_TRANSPORT);
            TransportDetailInformation[index] = EditingTransportDetailInfomation;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            TransportDetail.BtnSave.Visibility = Visibility.Collapsed;
            TransportDetail.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            TransportDetail.TransportDetailSearchControl.IsEnabled = true;
            TransportDetail.BtnAdd.Visibility = Visibility.Visible;
            TransportDetail.btnDelete.Visibility = Visibility.Visible;
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var RemoveTransportDetailInformation = (HR_EMP_TRANSPORT)CollectionViewSource.GetDefaultView(TransportDetail.TransportDetailGrid.DataContext).CurrentItem;

                if (client.ExistTransport(RemoveTransportDetailInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        TransportDetailInformation.Remove(RemoveTransportDetailInformation);
                        ApiAck ack = client.DeleteTransport(RemoveTransportDetailInformation);
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