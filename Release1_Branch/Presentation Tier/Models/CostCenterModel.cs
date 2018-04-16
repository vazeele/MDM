using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SmartBiz.MDMAPI.Common.Entities;
using System.Windows.Data;
using System.Windows.Controls;
using System.Runtime.Serialization;
using SmartBiz.MDM.Presentation.ServiceReference;
using SmartBiz.MDM.Presentation.Models;
using SmartBiz.MDM.Presentation.CustomControls;
using System.ComponentModel;
namespace SmartBiz.MDM.Presentation
{
    class CostCenterModel
    {
        ObservableCollection<FIN_CostCenter> CostCenterInformation; //These will be shown in the datagrid
        CostCenter costcenter; //Reference to Transaction Interface
        FIN_CostCenter EditingCostCenter; //Stores what we're editing right now, Incase we need to cancel the edit
        public CostCenterModel(CostCenter costcenter)
        {
            this.costcenter = costcenter;
            costcenter.CCSearchControl.Search = Search;
           // costcenter.CCSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_CostCenter_AutoGeneratingColumn;
            costcenter.CCSearchControl.ResultsGrid.SelectionChanged += dgv_CostCenter_SelectionChanged;
            costcenter.CostCenterGrid.SourceUpdated += CostCenterGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(costcenter.CCSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(costcenter.CCSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            CostCenterInformation = costcenter.CCSearchControl.ResultsGrid.ItemsSource as ObservableCollection<FIN_CostCenter>;
            costcenter.CostCenterGrid.DataContext = CostCenterInformation;
        }
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_CostCenter_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(FIN_CostCenter).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
        }
        //Fill Datagrid of the ForeignKeyDropDown when it's dropped down
        void CustomCombo_DropDownOpened(object sender, EventArgs e)
        {
            //DocumentAttributesModel.DocAttributesSearch(costcenter.DocAttribLTIDropDown.SearchControl);
        }
        //Search button of the ForeignKey drop down
        public void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            //costcenter.DocAttribLTIDropDown.ResetPager();
            //DocumentAttributesModel.DocAttributesSearch(costcenter.DocAttribLTIDropDown.SearchControl);
        }
        //When the user selects a row in the datagrid
        void dgv_CostCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (costcenter.CCSearchControl.ResultsGrid.SelectedItem != null)
                EditingCostCenter = (costcenter.CCSearchControl.ResultsGrid.SelectedItem as FIN_CostCenter).Clone() as FIN_CostCenter;
        }
        //If the user updates a record, then go to the Update mode automatically
        void CostCenterGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (costcenter.CostCenterGrid.DataContext == CostCenterInformation)
            {
                costcenter.BtnSave.Visibility = Visibility.Visible;
                costcenter.BtnCancelUpdate.Visibility = Visibility.Visible;
                costcenter.CCSearchControl.IsEnabled = false;
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
                var CostCenterToSave = (FIN_CostCenter)CollectionViewSource.GetDefaultView(costcenter.CostCenterGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                //costcenter.txt_LastTransactionSerialNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //costcenter.txt_LastVoucherNoLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //costcenter.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                costcenter.txt_CostCenterCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(costcenter.CostCenterGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (costcenter.BtnAdd.IsChecked == true)
                {

                    var cctosave = (CostCenterToSave);
                    if (!client.CostCenterExists(cctosave))
                    {
                        ApiAck ack = client.CreateCostCenter(cctosave);
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
                    ApiAck ack = client.UpdateCostCenter(CostCenterToSave);
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
                ObservableCollection<FIN_CostCenter> newCollection = new ObservableCollection<FIN_CostCenter>() { new FIN_CostCenter() };
                costcenter.BtnSave.Visibility = Visibility.Visible;
                costcenter.CostCenterGrid.DataContext = newCollection;
                costcenter.CCSearchControl.IsEnabled = false;
                costcenter.btnAdd_text.Text = "Cancel Add";
                costcenter.btnSave_text.Text = "Confirm Add";
                costcenter.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in costcenter.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                //costcenter.DocAttribLTIDropDown.IsEnabled = true;
                //costcenter.txt_SubSystemCodeLTI.IsEnabled = true;
                costcenter.txt_CostCenterCode.IsEnabled = true;

            }
            //If user cancels the add  , renable textboxes,grid etc;
            else
            {
                CompleteAdd();
            }
        }
        //Code for renabling tabs,textboxes etc; when an add operation finishes
        private void CompleteAdd()
        {   //Bind the original collection when the Add operation is complete       
            costcenter.CostCenterGrid.DataContext = CostCenterInformation;
            costcenter.BtnSave.Visibility = Visibility.Collapsed;
            costcenter.CCSearchControl.IsEnabled = true;
            costcenter.btnAdd_text.Text = "Add";
            costcenter.BtnAdd.IsChecked = false;
            costcenter.btnSave_text.Text = "Update";
            costcenter.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in costcenter.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            //costcenter.DocAttribLTIDropDown.IsEnabled = false;
            //costcenter.txt_SubSystemCodeLTI.IsEnabled = false;
            costcenter.txt_CostCenterCode.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            CostCenterQuery query = new CostCenterQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new CostCenterQuery() { CostCenterCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new CostCenterQuery() { CostCenterDescription = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryCostCenterAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<FIN_CostCenter>(response.Result.ToList<FIN_CostCenter>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = CostCenterInformation.IndexOf(costcenter.CCSearchControl.ResultsGrid.SelectedItem as FIN_CostCenter);
            CostCenterInformation[index] = EditingCostCenter;
            CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            costcenter.BtnSave.Visibility = Visibility.Collapsed;
            costcenter.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            costcenter.CCSearchControl.IsEnabled = true;
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
                var cctoremove = (FIN_CostCenter)CollectionViewSource.GetDefaultView(CostCenterInformation).CurrentItem;

                if (client.CostCenterExists(cctoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        CostCenterInformation.Remove(cctoremove);
                        ApiAck ack = client.DeleteCostCenter(cctoremove);
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

