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
    class PeriodModel
    {
        ObservableCollection<ERP_Period> PeriodInformation; //These will be shown in the datagrid
        Production Production; //Reference to Transaction Interface
        ERP_Period EditingPeriodInfomation; //Stores what we're editing right now, Incase we need to cancel the edit
        public PeriodModel(Production Production)
        {
            this.Production = Production;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Production.PeriodSearchControl.Search = Search;

            //Production.PeriodSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_Period_AutoGeneratingColumn;

            Production.PeriodSearchControl.ResultsGrid.SelectionChanged += dgv_Period_SelectionChanged;

            Production.PeriodGrid.SourceUpdated += PeriodGrid_SourceUpdated;            
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {                
                dpd.AddValueChanged(Production.PeriodSearchControl.ResultsGrid, ItemSourceChanged);               
            }
            
            Search(Production.PeriodSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            PeriodInformation = Production.PeriodSearchControl.ResultsGrid.ItemsSource as ObservableCollection<ERP_Period>;
            Production.PeriodGrid.DataContext = PeriodInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_Period_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(ERP_Period).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }        
        //When the user selects a row in the datagrid
        void dgv_Period_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Production.PeriodSearchControl.ResultsGrid.SelectedItem!=null)
                EditingPeriodInfomation = (Production.PeriodSearchControl.ResultsGrid.SelectedItem as ERP_Period).Clone() as ERP_Period;            
        }
        //If the user updates a record, then go to the Update mode automatically
        void PeriodGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Production.PeriodGrid.DataContext == PeriodInformation)
            {
                Production.BtnSave.Visibility = Visibility.Visible;
                Production.BtnCancelUpdate.Visibility = Visibility.Visible;
                Production.PeriodSearchControl.IsEnabled = false;
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
                var SavePeriodInformation = (ERP_Period)CollectionViewSource.GetDefaultView(Production.PeriodGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                /*EmergencyContact.txt_FullName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_Relationship.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_PermanentAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_OfficialAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneHome.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneMobile.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneOffice.GetBindingExpression(TextBox.TextProperty).UpdateSource();*/
                        
                //Checking if all controls are in valid state
                if (!Helper.IsValid(Production.PeriodGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Production.BtnAdd.IsChecked == true)
                {                                    
                    if (!client.ExistPeriod(SavePeriodInformation))
                    {
                        ApiAck ack = client.CreatePeriod(SavePeriodInformation);
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
                    ApiAck ack = client.UpdatePeriod(SavePeriodInformation);
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
                ObservableCollection<ERP_Period>  newCollection = new ObservableCollection<ERP_Period>() { new ERP_Period()};
                Production.BtnSave.Visibility = Visibility.Visible;
                Production.PeriodGrid.DataContext = newCollection;
                Production.PeriodSearchControl.IsEnabled = false;
                Production.btnAdd_text.Text = "Cancel Add";
                Production.btnSave_text.Text = "Confirm Add";
                Production.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Production.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Production.txt_ProductCodeP.IsEnabled = true;
                Production.txt_FinancialYearP.IsEnabled = true;
                Production.txt_AccountingPeriodP.IsEnabled = true;
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
            Production.PeriodGrid.DataContext = PeriodInformation;
            Production.BtnSave.Visibility = Visibility.Collapsed;
            Production.PeriodSearchControl.IsEnabled = true;
            Production.btnAdd_text.Text = "Add";
            Production.BtnAdd.IsChecked = false;
            Production.btnSave_text.Text = "Update";
            Production.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Production.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            Production.txt_ProductCodeP.IsEnabled = false;
            Production.txt_FinancialYearP.IsEnabled = false;
            Production.txt_AccountingPeriodP.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            PeriodQuery query = new PeriodQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new PeriodQuery() { ProductCode = SearchControl.SearchTextBox.Text };
            }
            if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new PeriodQuery() { FinancialYear = Int32.Parse(SearchControl.SearchTextBox.Text) };
            }
            if (SearchControl.OptionThree.IsChecked == true)
            {
                query = new PeriodQuery() {AccountingPeriod = Int32.Parse(SearchControl.SearchTextBox.Text) };
            } 
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryPeriodAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<ERP_Period>(response.Result.ToList<ERP_Period>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= PeriodInformation.IndexOf(Production.PeriodSearchControl.ResultsGrid.SelectedItem as ERP_Period);
           PeriodInformation[index] = EditingPeriodInfomation;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            Production.BtnSave.Visibility = Visibility.Collapsed;
            Production.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Production.PeriodSearchControl.IsEnabled = true;
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
                var RemovePeriodInformation = (ERP_Period)CollectionViewSource.GetDefaultView(Production.PeriodGrid.DataContext).CurrentItem;        

                if (client.ExistPeriod(RemovePeriodInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        PeriodInformation.Remove(RemovePeriodInformation);
                        ApiAck ack = client.DeletePeriod(RemovePeriodInformation);
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

