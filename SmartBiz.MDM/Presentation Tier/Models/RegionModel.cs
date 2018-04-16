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
    internal class RegionModel
    {
        private ObservableCollection<FIN_Region> AreaRegionInformation; //These will be shown in the datagrid
        private AreaRegion arearegion; //Reference to AreaRegion Interface
        private FIN_Region EditingAreaRegion; //Stores what we're editing right now, Incase we need to cancel the edit

        public RegionModel(AreaRegion arearegion)
        {
            this.arearegion = arearegion;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            arearegion.RSearchControl.Search = Search;
            arearegion.RSearchControl.ResultsGrid.SelectedCellsChanged += dgv_Region_SelectionChanged;
            arearegion.RegionGrid.SourceUpdated += RegionGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(arearegion.RSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(arearegion.RSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            AreaRegionInformation = arearegion.RSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_Region>;
            arearegion.RegionGrid.DataContext = AreaRegionInformation;
            if (AreaRegionInformation[0] != null)
            {
                EditingAreaRegion = AreaRegionInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Region_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (arearegion.RSearchControl.ResultsGrid.SelectedItem != null)
                EditingAreaRegion = (arearegion.RSearchControl.ResultsGrid.SelectedItem as FIN_Region).Clone() as FIN_Region;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void RegionGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (arearegion.RegionGrid.DataContext == AreaRegionInformation)
            {
                arearegion.BtnSave.Visibility = Visibility.Visible;
                arearegion.BtnCancelUpdate.Visibility = Visibility.Visible;
                arearegion.RSearchControl.IsEnabled = false;
                arearegion.BtnAdd.Visibility = Visibility.Collapsed;
                arearegion.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in arearegion.TbPage.Items)
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
                var RegionToSave = (FIN_Region)CollectionViewSource.GetDefaultView(arearegion.RegionGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                arearegion.txt_RegionCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(arearegion.RegionGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (arearegion.BtnAdd.IsChecked == true)
                {
                    if (!client.RegionExists(RegionToSave))
                    {
                        ApiAck ack = client.CreateRegion(RegionToSave);
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
                    ApiAck ack = client.UpdateRegion(RegionToSave);
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
            if (arearegion.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_Region> newCollection = new ObservableCollection<FIN_Region>() { new FIN_Region() };
                arearegion.BtnSave.Visibility = Visibility.Visible;
                arearegion.RegionGrid.DataContext = newCollection;
                arearegion.RSearchControl.IsEnabled = false;
                arearegion.btnAdd_text.Text = "Cancel Add";
                arearegion.btnSave_text.Text = "Confirm Add";
                arearegion.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in arearegion.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                arearegion.txt_RegionCode.IsEnabled = true;
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
            arearegion.RegionGrid.DataContext = AreaRegionInformation;
            arearegion.BtnSave.Visibility = Visibility.Collapsed;
            arearegion.RSearchControl.IsEnabled = true;
            arearegion.btnAdd_text.Text = "Add";
            arearegion.BtnAdd.IsChecked = false;
            arearegion.btnSave_text.Text = "Update";
            arearegion.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in arearegion.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            arearegion.txt_RegionCode.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            RegionQuery query = new RegionQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new RegionQuery() { RegionCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new RegionQuery() { RegionName = SearchControl.SearchTextBox.Text };
            }

            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryRegionAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_Region>(response.Result.ToList<FIN_Region>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = AreaRegionInformation.IndexOf(arearegion.RSearchControl.ResultsGrid.SelectedItem as FIN_Region);
            AreaRegionInformation[index] = EditingAreaRegion;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            arearegion.BtnSave.Visibility = Visibility.Collapsed;
            arearegion.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            arearegion.RSearchControl.IsEnabled = true;
            arearegion.BtnAdd.Visibility = Visibility.Visible;
            arearegion.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in arearegion.TbPage.Items)
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
                var rtoremove = (FIN_Region)CollectionViewSource.GetDefaultView(AreaRegionInformation).CurrentItem;

                if (client.RegionExists(rtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        AreaRegionInformation.Remove(rtoremove);
                        ApiAck ack = client.DeleteRegion(rtoremove);
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