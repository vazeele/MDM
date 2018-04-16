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
    class ProcessControlModel
    {
        ObservableCollection<ERP_ProcessControl> ProcessControlInformation; //These will be shown in the datagrid
        Production Production; //Reference to Transaction Interface
        ERP_ProcessControl EditingProcessControlInfomation; //Stores what we're editing right now, Incase we need to cancel the edit
        public ProcessControlModel(Production Production)
        {
            this.Production = Production;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Production.ProcessControlSearchControl.Search = Search;

            //Production.ProcessControlSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_ProcessControl_AutoGeneratingColumn;

            Production.ProcessControlSearchControl.ResultsGrid.SelectionChanged += dgv_ProcessControl_SelectionChanged;

            Production.ProcessControlGrid.SourceUpdated += ProcessControlGrid_SourceUpdated;            
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {                
                dpd.AddValueChanged(Production.ProcessControlSearchControl.ResultsGrid, ItemSourceChanged);               
            }
            
            Search(Production.ProcessControlSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            ProcessControlInformation = Production.ProcessControlSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_ProcessControl>;
            Production.ProcessControlGrid.DataContext = ProcessControlInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_ProcessControl_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(ERP_ProcessControl).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }        
        //When the user selects a row in the datagrid
        void dgv_ProcessControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Production.ProcessControlSearchControl.ResultsGrid.SelectedItem!=null)
                EditingProcessControlInfomation = (Production.ProcessControlSearchControl.ResultsGrid.SelectedItem as ERP_ProcessControl).Clone() as ERP_ProcessControl;            
        }
        //If the user updates a record, then go to the Update mode automatically
        void ProcessControlGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Production.ProcessControlGrid.DataContext == ProcessControlInformation)
            {
                Production.BtnSave.Visibility = Visibility.Visible;
                Production.BtnCancelUpdate.Visibility = Visibility.Visible;
                Production.ProcessControlSearchControl.IsEnabled = false;
                Production.BtnAdd.Visibility = Visibility.Collapsed;
                Production.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in Production.TbPage.Items)
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
                var SaveProcessControlInformation = (ERP_ProcessControl)CollectionViewSource.GetDefaultView(Production.ProcessControlGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                /*EmergencyContact.txt_FullName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_Relationship.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_PermanentAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_OfficialAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneHome.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneMobile.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneOffice.GetBindingExpression(TextBox.TextProperty).UpdateSource();*/
                        
                //Checking if all controls are in valid state
                if (!Helper.IsValid(Production.ProcessControlGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Production.BtnAdd.IsChecked == true)
                {                                    
                    if (!client.ExistProcessControl(SaveProcessControlInformation))
                    {
                        ApiAck ack = client.CreateProcessControl(SaveProcessControlInformation);
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
                    ApiAck ack = client.UpdateProcessControl(SaveProcessControlInformation);
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
            if (Production.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<ERP_ProcessControl>  newCollection = new ObservableCollection<ERP_ProcessControl>() { new ERP_ProcessControl()};
                Production.BtnSave.Visibility = Visibility.Visible;
                Production.ProcessControlGrid.DataContext = newCollection;
                Production.ProcessControlSearchControl.IsEnabled = false;
                Production.btnAdd_text.Text = "Cancel Add";
                Production.btnSave_text.Text = "Confirm Add";
                Production.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Production.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Production.txt_ProductCodePC.IsEnabled = true;     
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
            Production.ProcessControlGrid.DataContext = ProcessControlInformation;
            Production.BtnSave.Visibility = Visibility.Collapsed;
            Production.ProcessControlSearchControl.IsEnabled = true;
            Production.btnAdd_text.Text = "Add";
            Production.BtnAdd.IsChecked = false;
            Production.btnSave_text.Text = "Update";
            Production.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Production.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            Production.txt_ProductCodePC.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            ProcessControlQuery query = new ProcessControlQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new ProcessControlQuery() { ProductCode = SearchControl.SearchTextBox.Text };
            }            
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryProcessControlAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_ProcessControl>(response.Result.ToList<ERP_ProcessControl>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= ProcessControlInformation.IndexOf(Production.ProcessControlSearchControl.ResultsGrid.SelectedItem as ERP_ProcessControl);
           ProcessControlInformation[index] = EditingProcessControlInfomation;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            Production.BtnSave.Visibility = Visibility.Collapsed;
            Production.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Production.ProcessControlSearchControl.IsEnabled = true;
            Production.BtnAdd.Visibility = Visibility.Visible;
            Production.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in Production.TbPage.Items)
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
                var RemoveProcessControlInformation = (ERP_ProcessControl)CollectionViewSource.GetDefaultView(Production.ProcessControlGrid.DataContext).CurrentItem;        

                if (client.ExistProcessControl(RemoveProcessControlInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        ProcessControlInformation.Remove(RemoveProcessControlInformation);
                        ApiAck ack = client.DeleteProcessControl(RemoveProcessControlInformation);
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

