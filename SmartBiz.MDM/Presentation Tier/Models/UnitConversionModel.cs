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
    internal class UnitConversionModel
    {
        private ObservableCollection<ERP_UnitConversion> UnitConversionInformation; //These will be shown in the datagrid
        private Unit unit; //Reference to Transaction Interface
        private ERP_UnitConversion EditingUnitConversion; //Stores what we're editing right now, Incase we need to cancel the edit

        public UnitConversionModel(Unit unit)
        {
            this.unit = unit;
            unit.UCSearchControl.Search = Search;
            unit.FUCDropDown.Search = unit.TUCDropDown.Search = UnitDefinitionModel.Search;
            unit.UCSearchControl.ResultsGrid.SelectedCellsChanged += dgv_UnitConversion_SelectionChanged;
            unit.UnitConversionGrid.SourceUpdated += UnitConversionGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(unit.UCSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(unit.UCSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            UnitConversionInformation = unit.UCSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_UnitConversion>;
            unit.UnitConversionGrid.DataContext = UnitConversionInformation;
            if (UnitConversionInformation[0] != null)
            {
                EditingUnitConversion = UnitConversionInformation[0].Clone();
            }

        }

        //When the user selects a row in the datagrid
        private void dgv_UnitConversion_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (unit.UCSearchControl.ResultsGrid.SelectedItem != null)
                EditingUnitConversion = (unit.UCSearchControl.ResultsGrid.SelectedItem as ERP_UnitConversion).Clone() as ERP_UnitConversion;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void UnitConversionGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (unit.UnitConversionGrid.DataContext == UnitConversionInformation)
            {
                unit.BtnSave.Visibility = Visibility.Visible;
                unit.BtnCancelUpdate.Visibility = Visibility.Visible;
                unit.UCSearchControl.IsEnabled = false;
                unit.BtnAdd.Visibility = Visibility.Collapsed;
                unit.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in unit.TbPage.Items)
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
                var UnitConversionToSave = (ERP_UnitConversion)CollectionViewSource.GetDefaultView(unit.UnitConversionGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                unit.FUCDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(unit.UnitConversionGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (unit.BtnAdd.IsChecked == true)
                {
                    var uctosave = (UnitConversionToSave);
                    if (!client.UnitConversionExists(uctosave))
                    {
                        ApiAck ack = client.CreateUnitConversion(uctosave);
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
                    ApiAck ack = client.UpdateUnitConversion(UnitConversionToSave);
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
            if (unit.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<ERP_UnitConversion> newCollection = new ObservableCollection<ERP_UnitConversion>() { new ERP_UnitConversion() };
                unit.BtnSave.Visibility = Visibility.Visible;
                unit.UnitConversionGrid.DataContext = newCollection;
                unit.UCSearchControl.IsEnabled = false;
                unit.btnAdd_text.Text = "Cancel Add";
                unit.btnSave_text.Text = "Confirm Add";
                unit.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in unit.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                unit.FUCDropDown.IsEnabled = true;
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
            unit.UnitConversionGrid.DataContext = UnitConversionInformation;
            unit.BtnSave.Visibility = Visibility.Collapsed;
            unit.UCSearchControl.IsEnabled = true;
            unit.btnAdd_text.Text = "Add";
            unit.BtnAdd.IsChecked = false;
            unit.btnSave_text.Text = "Update";
            unit.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in unit.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            unit.FUCDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            UnitConversionQuery query = new UnitConversionQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new UnitConversionQuery() { FromUnitCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new UnitConversionQuery() { ToUnitCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new UnitConversionQuery() { ConversionFactor = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryUnitConversionAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_UnitConversion>(response.Result.ToList<ERP_UnitConversion>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = UnitConversionInformation.IndexOf(unit.UCSearchControl.ResultsGrid.SelectedItem as ERP_UnitConversion);
            UnitConversionInformation[index] = EditingUnitConversion;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            unit.BtnSave.Visibility = Visibility.Collapsed;
            unit.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            unit.UCSearchControl.IsEnabled = true;
            unit.BtnAdd.Visibility = Visibility.Visible;
            unit.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in unit.TbPage.Items)
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
                var uctoremove = (ERP_UnitConversion)CollectionViewSource.GetDefaultView(UnitConversionInformation).CurrentItem;

                if (client.UnitConversionExists(uctoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        UnitConversionInformation.Remove(uctoremove);
                        ApiAck ack = client.DeleteUnitConversion(uctoremove);
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