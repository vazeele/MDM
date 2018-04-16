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
    class HRCashBenefitModel
    {
        ObservableCollection<HR_CASH_BENEFIT> LastBenefitInformation; //These will be shown in the datagrid
        HRBenefits benefit; //Reference to Account Interface
        HR_CASH_BENEFIT EditingLastBenefitInfo; //Stores what we're editing right now, Incase we need to cancel the edit
        public HRCashBenefitModel(HRBenefits benefit)
        {
            this.benefit = benefit;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            benefit.CashBenefitSearchControl.Search = Search;

            //Fill the combobox with the values of the [Values] attribute; See entity.cs
            benefit.cmb_CashBenifits_ActiveInactiveFlag.ItemsSource = Helper.getItemSource(typeof(HR_CASH_BENEFIT), "STATUS_FLAG");
            //benefit.CashBenefitSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_CashBenefit_AutoGeneratingColumn;
            benefit.CashBenefitSearchControl.ResultsGrid.SelectionChanged += dgv_AccountType_SelectionChanged;
            benefit.CashBenefitGrid.SourceUpdated += CashBenefitGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(benefit.CashBenefitSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(benefit.CashBenefitSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            LastBenefitInformation  = benefit.CashBenefitSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_CASH_BENEFIT>;
            benefit.CashBenefitGrid.DataContext = LastBenefitInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_CashBenefit_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_CASH_BENEFIT).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }

        //Fill Datagrid of the ForeignKeyDropDown when it's dropped down
        //void CustomCombo_DropDownOpened(object sender, EventArgs e)
        //{
        //    DocumentAttributesModel.DocAttributesSearch(transaction.DocAttribLTIDropDown.SearchControl);
        //}
        //Search button of the ForeignKey drop down
        //public  void btn_Search_Click(object sender, RoutedEventArgs e)
        //{
        //    transaction.DocAttribLTIDropDown.ResetPager();
        //    DocumentAttributesModel.DocAttributesSearch(transaction.DocAttribLTIDropDown.SearchControl);
        //} 
        //When the user selects a row in the datagrid
        void dgv_AccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (benefit.CashBenefitSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastBenefitInfo = (benefit.CashBenefitSearchControl.ResultsGrid.SelectedItem as HR_CASH_BENEFIT).Clone() as HR_CASH_BENEFIT;
        }
        //If the user updates a record, then go to the Update mode automatically
        void CashBenefitGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (benefit.CashBenefitGrid.DataContext == LastBenefitInformation)
            {
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.BtnCancelUpdate.Visibility = Visibility.Visible;
                benefit.CashBenefitSearchControl.IsEnabled = false;
                benefit.BtnAdd.Visibility = Visibility.Collapsed;
                benefit.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in benefit.TbPage.Items)
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
                var LastBenefitInfoToSave = (HR_CASH_BENEFIT)CollectionViewSource.GetDefaultView(benefit.CashBenefitGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                benefit.txt_CashBenifits_BenefifCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                benefit.txt_CashBenifits_Name.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                benefit.txt_CashBenifits_Amount.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                      

                //Checking if all controls are in valid state
                if (!Helper.IsValid(benefit.CashBenefitGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (benefit.BtnAdd.IsChecked == true)
                {
                    
                    var BenefitToSave = (LastBenefitInfoToSave);
                    if (!client.HRCashBenefitExists(BenefitToSave))
                    {
                        ApiAck ack = client.CreateHRCashBenefit(BenefitToSave);
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
                    ApiAck ack = client.UpdateHRCashBenefit(LastBenefitInfoToSave);
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
            if (benefit.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_CASH_BENEFIT>  newCollection = new ObservableCollection<HR_CASH_BENEFIT>() { new HR_CASH_BENEFIT()};
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.CashBenefitGrid.DataContext = newCollection;
                benefit.CashBenefitSearchControl.IsEnabled = false;
                benefit.cmb_CashBenifits_ActiveInactiveFlag.SelectedIndex = 0;
                benefit.btnAdd_text.Text = "Cancel Add";
                benefit.btnSave_text.Text = "Confirm Add";
                benefit.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in benefit.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                benefit.txt_CashBenifits_BenefifCode.IsEnabled = true;
                     

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
            benefit.CashBenefitGrid.DataContext = LastBenefitInformation;
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.CashBenefitSearchControl.IsEnabled = true;
            benefit.btnAdd_text.Text = "Add";
            benefit.BtnAdd.IsChecked = false;
            benefit.btnSave_text.Text = "Update";
            benefit.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in benefit.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            benefit.txt_CashBenifits_BenefifCode.IsEnabled = false;
            
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HRCashBenefitQuery query = new HRCashBenefitQuery(); //by default we have an empty query
             if(SearchControl.OptionOne.IsChecked==true)
                 query = new HRCashBenefitQuery() { BenefitCode = SearchControl.SearchTextBox.Text };
            
            
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryHRCashBenefitsAsync(query, pagesize, pagePosition);

          //  var response = client.ReadAllAccountType(pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_CASH_BENEFIT>(response.Result.ToList<HR_CASH_BENEFIT>());        
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= LastBenefitInformation.IndexOf(benefit.CashBenefitSearchControl.ResultsGrid.SelectedItem as HR_CASH_BENEFIT);
           LastBenefitInformation [index]= EditingLastBenefitInfo;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            benefit.CashBenefitSearchControl.IsEnabled = true;
            benefit.BtnAdd.Visibility = Visibility.Visible;
            benefit.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in benefit.TbPage.Items)
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
                var BenefitToRemove = (HR_CASH_BENEFIT)CollectionViewSource.GetDefaultView(LastBenefitInformation).CurrentItem;        

                if (client.HRCashBenefitExists(BenefitToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastBenefitInformation.Remove(BenefitToRemove);
                        ApiAck ack = client.DeleteHRCashBenefit(BenefitToRemove);
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

