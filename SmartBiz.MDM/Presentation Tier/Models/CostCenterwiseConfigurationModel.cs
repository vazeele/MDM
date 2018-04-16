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
    internal class CostCenterwiseConfigurationModel
    {
        private ObservableCollection<FIN_CostCenterwiseConfiguration> CostCenterwiseConfigurationInformation; //These will be shown in the datagrid
        private CostCenter costcenter; //Reference to Transaction Interface
        private FIN_CostCenterwiseConfiguration EditingCostCenterwiseConfiguration; //Stores what we're editing right now, Incase we need to cancel the edit

        public CostCenterwiseConfigurationModel(CostCenter costcenter)
        {
            this.costcenter = costcenter;
            //--------------------------------------------------------------------------------------------------
            costcenter.CCwCCDropDown.Search = CostCenterModel.Search;
            costcenter.BCCDropDown.Search = CurrencyModel.Search;
            costcenter.AANDropDown.Search = GeneralLedgerAccountModel.Search;
            costcenter.ACCDropDown.Search = CostCenterModel.Search;
            costcenter.ACurrCDropDown.Search = CurrencyModel.Search;
            costcenter.APCCCDropDown.Search = CostCenterModel.Search;
            costcenter.DCfIDropDown.Search = DocumentAttributesModel.Search;
            costcenter.TCfIDropDown.Search = DocumentAttributesModel.Search;
            costcenter.DCfRPDropDown.Search = DocumentAttributesModel.Search;
            costcenter.TCfRPDropDown.Search = DocumentAttributesModel.Search;
            costcenter.DCfRGLDropDown.Search = DocumentAttributesModel.Search;
            costcenter.TCfGRLDropDown.Search = DocumentAttributesModel.Search;
            costcenter.BGLAfRPDropDown.Search = GeneralLedgerAccountModel.Search;
            costcenter.BCCfRPDropDown.Search = CostCenterModel.Search;
            costcenter.BCuCfRPDropDown.Search = CurrencyModel.Search;
            costcenter.PNLACfRPDropDown.Search = GeneralLedgerAccountModel.Search;
            costcenter.PNLCCfRPDropDown.Search = CostCenterModel.Search;
            costcenter.PNLCuCfRPDropDown.Search = CurrencyModel.Search;
            costcenter.DBTACfRPDropDown.Search = GeneralLedgerAccountModel.Search;
            costcenter.DBTCCfRPDropDown.Search = CostCenterModel.Search;
            costcenter.DBTCuCfRPDropDown.Search = CurrencyModel.Search;
            costcenter.CRDACfRPDropDown.Search = GeneralLedgerAccountModel.Search;
            costcenter.CRDCCfRPDropDown.Search = CostCenterModel.Search;
            costcenter.CRDCuCfRPDropDown.Search = CurrencyModel.Search;
            //--------------------------------------------------------------------------------------------------
            costcenter.CCwCSearchControl.Search = Search;
            costcenter.CCwCSearchControl.ResultsGrid.SelectedCellsChanged += dgv_CostCenterwiseConfiguration_SelectionChanged;
            costcenter.CostCenterwiseConfigurationGrid.SourceUpdated += CostCenterwiseConfigurationGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(costcenter.CCwCSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(costcenter.CCwCSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            CostCenterwiseConfigurationInformation = costcenter.CCwCSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_CostCenterwiseConfiguration>;
            costcenter.CostCenterwiseConfigurationGrid.DataContext = CostCenterwiseConfigurationInformation;
            if (CostCenterwiseConfigurationInformation.Count>0)
            {
                EditingCostCenterwiseConfiguration = CostCenterwiseConfigurationInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_CostCenterwiseConfiguration_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (costcenter.CCSearchControl.ResultsGrid.SelectedItem != null)
                EditingCostCenterwiseConfiguration = (costcenter.CCwCSearchControl.ResultsGrid.SelectedItem as FIN_CostCenterwiseConfiguration).Clone() as FIN_CostCenterwiseConfiguration;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void CostCenterwiseConfigurationGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (costcenter.CostCenterwiseConfigurationGrid.DataContext == CostCenterwiseConfigurationInformation)
            {
                costcenter.BtnSave.Visibility = Visibility.Visible;
                costcenter.BtnCancelUpdate.Visibility = Visibility.Visible;
                costcenter.CCwCSearchControl.IsEnabled = false;
                costcenter.BtnAdd.Visibility = Visibility.Collapsed;
                costcenter.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in costcenter.TbPage.Items)
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
                var CostCenterwiseConfigurationToSave = (FIN_CostCenterwiseConfiguration)CollectionViewSource.GetDefaultView(costcenter.CostCenterwiseConfigurationGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                costcenter.txt_RevNo.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(costcenter.CostCenterwiseConfigurationGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (costcenter.BtnAdd.IsChecked == true)
                {
                    var ccwctosave = (CostCenterwiseConfigurationToSave);
                    if (!client.CostCenterwiseConfigurationExists(ccwctosave))
                    {
                        ApiAck ack = client.CreateCostCenterwiseConfiguration(ccwctosave);
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
                    ApiAck ack = client.UpdateCostCenterwiseConfiguration(CostCenterwiseConfigurationToSave);
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
            if (costcenter.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<FIN_CostCenterwiseConfiguration> newCollection = new ObservableCollection<FIN_CostCenterwiseConfiguration>() { new FIN_CostCenterwiseConfiguration() };
                costcenter.BtnSave.Visibility = Visibility.Visible;
                costcenter.CostCenterwiseConfigurationGrid.DataContext = newCollection;
                costcenter.CCwCSearchControl.IsEnabled = false;
                costcenter.btnAdd_text.Text = "Cancel Add";
                costcenter.btnSave_text.Text = "Confirm Add";
                costcenter.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in costcenter.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                costcenter.txt_RevNo.IsEnabled = true;
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
            costcenter.CostCenterwiseConfigurationGrid.DataContext = CostCenterwiseConfigurationInformation;
            costcenter.BtnSave.Visibility = Visibility.Collapsed;
            costcenter.CCwCSearchControl.IsEnabled = true;
            costcenter.btnAdd_text.Text = "Add";
            costcenter.BtnAdd.IsChecked = false;
            costcenter.btnSave_text.Text = "Update";
            costcenter.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in costcenter.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            costcenter.txt_RevNo.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CostCenterwiseConfigurationQuery query = new CostCenterwiseConfigurationQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new CostCenterwiseConfigurationQuery() { CostCenterCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new CostCenterwiseConfigurationQuery() { RevNo = int.Parse(SearchControl.SearchTextBox.Text) };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new CostCenterwiseConfigurationQuery() { BaseCurrencyCode = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCostCenterwiseConfigurationAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_CostCenterwiseConfiguration>(response.Result.ToList<FIN_CostCenterwiseConfiguration>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = CostCenterwiseConfigurationInformation.IndexOf(costcenter.CCwCSearchControl.ResultsGrid.SelectedItem as FIN_CostCenterwiseConfiguration);
            CostCenterwiseConfigurationInformation[index] = EditingCostCenterwiseConfiguration;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            costcenter.BtnSave.Visibility = Visibility.Collapsed;
            costcenter.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            costcenter.CCwCSearchControl.IsEnabled = true;
            costcenter.BtnAdd.Visibility = Visibility.Visible;
            costcenter.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in costcenter.TbPage.Items)
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
                var ccwctoremove = (FIN_CostCenterwiseConfiguration)CollectionViewSource.GetDefaultView(CostCenterwiseConfigurationInformation).CurrentItem;

                if (client.CostCenterwiseConfigurationExists(ccwctoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        CostCenterwiseConfigurationInformation.Remove(ccwctoremove);
                        ApiAck ack = client.DeleteCostCenterwiseConfiguration(ccwctoremove);
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