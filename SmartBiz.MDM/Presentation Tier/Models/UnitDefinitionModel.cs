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
    internal class UnitDefinitionModel
    {
        private ObservableCollection<ERP_UnitDefinition> UnitDefinitionInformation; //These will be shown in the datagrid
        private Unit unit; //Reference to Transaction Interface
        private ERP_UnitDefinition EditingUnitDefinition; //Stores what we're editing right now, Incase we need to cancel the edit

        public UnitDefinitionModel(Unit unit)
        {
            this.unit = unit;
            unit.UDSearchControl.Search = unit.MUCDropDown.Search = Search;
            unit.UDSearchControl.ResultsGrid.SelectedCellsChanged += dgv_UnitDefinition_SelectionChanged;
            unit.UnitDefinitionGrid.SourceUpdated += UnitDefinitionGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(unit.UDSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(unit.UDSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            UnitDefinitionInformation = unit.UDSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_UnitDefinition>;
            unit.UnitDefinitionGrid.DataContext = UnitDefinitionInformation;
            if (UnitDefinitionInformation.Count>0)
            {
                EditingUnitDefinition = UnitDefinitionInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_UnitDefinition_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (unit.UDSearchControl.ResultsGrid.SelectedItem != null)
                EditingUnitDefinition = (unit.UDSearchControl.ResultsGrid.SelectedItem as ERP_UnitDefinition).Clone();
        }

        //If the user updates a record, then go to the Update mode automatically
        private void UnitDefinitionGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (unit.UnitDefinitionGrid.DataContext == UnitDefinitionInformation)
            {
                unit.BtnSave.Visibility = Visibility.Visible;
                unit.BtnCancelUpdate.Visibility = Visibility.Visible;
                unit.UDSearchControl.IsEnabled = false;
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
                var UnitDefinitionToSave = (ERP_UnitDefinition)CollectionViewSource.GetDefaultView(unit.UnitDefinitionGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                unit.txt_UnitCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(unit.UnitDefinitionGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (unit.BtnAdd.IsChecked == true)
                {
                    var udtosave = (UnitDefinitionToSave);
                    if (!client.UnitDefinitionExists(udtosave))
                    {
                        ApiAck ack = client.CreateUnitDefinition(udtosave);
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
                    ApiAck ack = client.UpdateUnitDefinition(UnitDefinitionToSave);
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
                ObservableCollection<ERP_UnitDefinition> newCollection = new ObservableCollection<ERP_UnitDefinition>() { new ERP_UnitDefinition() };
                unit.BtnSave.Visibility = Visibility.Visible;
                unit.UnitDefinitionGrid.DataContext = newCollection;
                unit.UDSearchControl.IsEnabled = false;
                unit.btnAdd_text.Text = "Cancel Add";
                unit.btnSave_text.Text = "Confirm Add";
                unit.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in unit.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                unit.txt_UnitCode.IsEnabled = true;
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
            unit.UnitDefinitionGrid.DataContext = UnitDefinitionInformation;
            unit.BtnSave.Visibility = Visibility.Collapsed;
            unit.UDSearchControl.IsEnabled = true;
            unit.btnAdd_text.Text = "Add";
            unit.BtnAdd.IsChecked = false;
            unit.btnSave_text.Text = "Update";
            unit.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in unit.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            unit.txt_UnitCode.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            UnitDefinitionQuery query = new UnitDefinitionQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new UnitDefinitionQuery() { UnitCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new UnitDefinitionQuery() { MajorUnitCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new UnitDefinitionQuery() { StandardSyntax = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryUnitDefinitionAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_UnitDefinition>(response.Result.ToList<ERP_UnitDefinition>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = UnitDefinitionInformation.IndexOf(unit.UDSearchControl.ResultsGrid.SelectedItem as ERP_UnitDefinition);
            UnitDefinitionInformation[index] = EditingUnitDefinition;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            unit.BtnSave.Visibility = Visibility.Collapsed;
            unit.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            unit.UDSearchControl.IsEnabled = true;
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
                var udtoremove = (ERP_UnitDefinition)CollectionViewSource.GetDefaultView(UnitDefinitionInformation).CurrentItem;

                if (client.UnitDefinitionExists(udtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        UnitDefinitionInformation.Remove(udtoremove);
                        ApiAck ack = client.DeleteUnitDefinition(udtoremove);
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