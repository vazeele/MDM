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
    internal class AreaModel
    {

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AreaQuery query = new AreaQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AreaQuery() { AreaCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AreaQuery() { AreaName = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new AreaQuery() { RegionCode = SearchControl.SearchTextBox.Text };
            }
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAreaAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_Area>(response.Result.ToList<FIN_Area>()); ;
        }


        private ObservableCollection<FIN_Area> AreaRegionInformation; //These will be shown in the datagrid
        private AreaRegion arearegion; //Reference to AreaRegion Interface
        private FIN_Area EditingAreaRegion; //Stores what we're editing right now, Incase we need to cancel the edit

        public AreaModel(AreaRegion arearegion)
        {
            this.arearegion = arearegion;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            arearegion.ASearchControl.Search = Search;
            arearegion.RCDropDown.Search = RegionModel.Search;
            arearegion.ASearchControl.ResultsGrid.SelectedCellsChanged += dgv_Area_SelectionChanged;
            arearegion.AreaGrid.SourceUpdated += AreaGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(arearegion.ASearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(arearegion.ASearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            AreaRegionInformation = arearegion.ASearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_Area>;
            arearegion.AreaGrid.DataContext = AreaRegionInformation;
            if (AreaRegionInformation[0] != null)
            {
                EditingAreaRegion = AreaRegionInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Area_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (arearegion.ASearchControl.ResultsGrid.SelectedItem != null)
                EditingAreaRegion = (arearegion.ASearchControl.ResultsGrid.SelectedItem as FIN_Area).Clone() as FIN_Area;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void AreaGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (arearegion.AreaGrid.DataContext == AreaRegionInformation)
            {
                arearegion.BtnSave.Visibility = Visibility.Visible;
                arearegion.BtnCancelUpdate.Visibility = Visibility.Visible;
                arearegion.ASearchControl.IsEnabled = false;
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
                var AreaToSave = (FIN_Area)CollectionViewSource.GetDefaultView(arearegion.AreaGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                arearegion.txt_AreaCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(arearegion.AreaGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (arearegion.BtnAdd.IsChecked == true)
                {
                    if (!client.AreaExists(AreaToSave))
                    {
                        ApiAck ack = client.CreateArea(AreaToSave);
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
                    ApiAck ack = client.UpdateArea(AreaToSave);
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
                ObservableCollection<FIN_Area> newCollection = new ObservableCollection<FIN_Area>() { new FIN_Area() };
                arearegion.BtnSave.Visibility = Visibility.Visible;
                arearegion.AreaGrid.DataContext = newCollection;
                arearegion.ASearchControl.IsEnabled = false;
                arearegion.btnAdd_text.Text = "Cancel Add";
                arearegion.btnSave_text.Text = "Confirm Add";
                arearegion.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in arearegion.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                arearegion.RCDropDown.IsEnabled = true;
                arearegion.txt_AreaCode.IsEnabled = true;

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
            arearegion.AreaGrid.DataContext = AreaRegionInformation;
            arearegion.BtnSave.Visibility = Visibility.Collapsed;
            arearegion.ASearchControl.IsEnabled = true;
            arearegion.btnAdd_text.Text = "Add";
            arearegion.BtnAdd.IsChecked = false;
            arearegion.btnSave_text.Text = "Update";
            arearegion.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in arearegion.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            arearegion.RCDropDown.IsEnabled = false;
            arearegion.txt_AreaCode.IsEnabled = false;

        }


        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = AreaRegionInformation.IndexOf(arearegion.ASearchControl.ResultsGrid.SelectedItem as FIN_Area);
            AreaRegionInformation[index] = EditingAreaRegion;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            arearegion.BtnSave.Visibility = Visibility.Collapsed;
            arearegion.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            arearegion.ASearchControl.IsEnabled = true;
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
                var atoremove = (FIN_Area)CollectionViewSource.GetDefaultView(AreaRegionInformation).CurrentItem;

                if (client.AreaExists(atoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        AreaRegionInformation.Remove(atoremove);
                        ApiAck ack = client.DeleteArea(atoremove);
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