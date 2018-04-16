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
    class AgentBrokerSalesManModel
    {
        ObservableCollection<ERP_AgentBrokerSalesMan> AgentBrokerSalesManInformation; //These will be shown in the datagrid
        AgentBrokerSalesMan agentbrokersalesman; //Reference to Transaction Interface
        ERP_AgentBrokerSalesMan EditingAgentBrokerSalesMan; //Stores what we're editing right now, Incase we need to cancel the edit
        public AgentBrokerSalesManModel(AgentBrokerSalesMan agentbrokersalesman)
        {
            this.agentbrokersalesman = agentbrokersalesman;
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            agentbrokersalesman.ABSMSearchControl.Search = Search;
            agentbrokersalesman.CCDropDown.Search = DocumentAttributesModel.Search;
            agentbrokersalesman.GLADropDown.Search = CurrencyModel.Search;
           // agentbrokersalesman.ABSMSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_AgentBrokerSalesMan_AutoGeneratingColumn;
            agentbrokersalesman.ABSMSearchControl.ResultsGrid.SelectionChanged += dgv_AgentBrokerSalesMan_SelectionChanged;
            agentbrokersalesman.AgentBrokerSalesManGrid.SourceUpdated += AgentBrokerSalesManGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(agentbrokersalesman.ABSMSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(agentbrokersalesman.ABSMSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            AgentBrokerSalesManInformation = agentbrokersalesman.ABSMSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_AgentBrokerSalesMan>;
            agentbrokersalesman.AgentBrokerSalesManGrid.DataContext = AgentBrokerSalesManInformation;
        }
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_AgentBrokerSalesMan_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(ERP_AgentBrokerSalesMan).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
        }
        //When the user selects a row in the datagrid
        void dgv_AgentBrokerSalesMan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (agentbrokersalesman.ABSMSearchControl.ResultsGrid.SelectedItem != null)
                EditingAgentBrokerSalesMan = (agentbrokersalesman.ABSMSearchControl.ResultsGrid.SelectedItem as ERP_AgentBrokerSalesMan).Clone() as ERP_AgentBrokerSalesMan;
        }
        //If the user updates a record, then go to the Update mode automatically
        void AgentBrokerSalesManGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (agentbrokersalesman.AgentBrokerSalesManGrid.DataContext == AgentBrokerSalesManInformation)
            {
                agentbrokersalesman.BtnSave.Visibility = Visibility.Visible;
                agentbrokersalesman.BtnCancelUpdate.Visibility = Visibility.Visible;
                agentbrokersalesman.ABSMSearchControl.IsEnabled = false;
                agentbrokersalesman.BtnAdd.Visibility = Visibility.Collapsed;
                agentbrokersalesman.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in agentbrokersalesman.TbPage.Items)
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
                var AgentBrokerSalesManToSave = (ERP_AgentBrokerSalesMan)CollectionViewSource.GetDefaultView(agentbrokersalesman.AgentBrokerSalesManGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                agentbrokersalesman.cmb_AgentBrokerSalesManFlag.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                agentbrokersalesman.txt_Code.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //agentbrokersalesman.txt_ProductCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                //agentbrokersalesman.txt_SubSystemCodeLTI.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(agentbrokersalesman.AgentBrokerSalesManGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (agentbrokersalesman.BtnAdd.IsChecked == true)
                {

                    var absmtosave = (AgentBrokerSalesManToSave);
                    if (!client.AgentBrokerSalesManExists(absmtosave))
                    {
                        ApiAck ack = client.CreateAgentBrokerSalesMan(absmtosave);
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
                    ApiAck ack = client.UpdateAgentBrokerSalesMan(AgentBrokerSalesManToSave);
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
            if (agentbrokersalesman.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<ERP_AgentBrokerSalesMan> newCollection = new ObservableCollection<ERP_AgentBrokerSalesMan>() { new ERP_AgentBrokerSalesMan() };
                agentbrokersalesman.BtnSave.Visibility = Visibility.Visible;
                agentbrokersalesman.AgentBrokerSalesManGrid.DataContext = newCollection;
                agentbrokersalesman.ABSMSearchControl.IsEnabled = false;
                agentbrokersalesman.btnAdd_text.Text = "Cancel Add";
                agentbrokersalesman.btnSave_text.Text = "Confirm Add";
                agentbrokersalesman.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in agentbrokersalesman.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                agentbrokersalesman.cmb_AgentBrokerSalesManFlag.IsEnabled = true;
                agentbrokersalesman.txt_Code.IsEnabled = true;
                //agentbrokersalesman.txt_ProductCodeLTI.IsEnabled = true;

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
            agentbrokersalesman.AgentBrokerSalesManGrid.DataContext = AgentBrokerSalesManInformation;
            agentbrokersalesman.BtnSave.Visibility = Visibility.Collapsed;
            agentbrokersalesman.ABSMSearchControl.IsEnabled = true;
            agentbrokersalesman.btnAdd_text.Text = "Add";
            agentbrokersalesman.BtnAdd.IsChecked = false;
            agentbrokersalesman.btnSave_text.Text = "Update";
            agentbrokersalesman.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in agentbrokersalesman.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            agentbrokersalesman.cmb_AgentBrokerSalesManFlag.IsEnabled = false;
            agentbrokersalesman.txt_Code.IsEnabled = false;
            //agentbrokersalesman.txt_ProductCodeLTI.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            AgentBrokerSalesManQuery query = new AgentBrokerSalesManQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new AgentBrokerSalesManQuery() { AgentBrokerSalesManCode = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new AgentBrokerSalesManQuery() { City = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new AgentBrokerSalesManQuery() { Country = SearchControl.SearchTextBox.Text };
            }
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryAgentBrokerSalesManAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_AgentBrokerSalesMan>(response.Result.ToList<ERP_AgentBrokerSalesMan>()); ;
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = AgentBrokerSalesManInformation.IndexOf(agentbrokersalesman.ABSMSearchControl.ResultsGrid.SelectedItem as ERP_AgentBrokerSalesMan);
            AgentBrokerSalesManInformation[index] = EditingAgentBrokerSalesMan;
            CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            agentbrokersalesman.BtnSave.Visibility = Visibility.Collapsed;
            agentbrokersalesman.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            agentbrokersalesman.ABSMSearchControl.IsEnabled = true;
            agentbrokersalesman.BtnAdd.Visibility = Visibility.Visible;
            agentbrokersalesman.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in agentbrokersalesman.TbPage.Items)
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
                var absmtoremove = (ERP_AgentBrokerSalesMan)CollectionViewSource.GetDefaultView(AgentBrokerSalesManInformation).CurrentItem;

                if (client.AgentBrokerSalesManExists(absmtoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        AgentBrokerSalesManInformation.Remove(absmtoremove);
                        ApiAck ack = client.DeleteAgentBrokerSalesMan(absmtoremove);
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

