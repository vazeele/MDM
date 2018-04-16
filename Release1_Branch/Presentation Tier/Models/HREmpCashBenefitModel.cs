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
    class HREmpCashBenefitModel
    {
        ObservableCollection<HR_EMP_CASH_BENEFIT> LastBenefitInformation; //These will be shown in the datagrid
        HRBenefits benefit; //Reference to HR Benefit Interface
        HR_EMP_CASH_BENEFIT EditingLastBenefitInfo; //Stores what we're editing right now, in case we need to cancel the edit
        public HREmpCashBenefitModel(HRBenefits benefit)
        {
            this.benefit = benefit;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            benefit.EmpCashBenefitSearchControl.Search = Search;

            benefit.EmpCashBenifitEmpIdDropDown.Search = EmployeeModel.Search;
            benefit.EmpCashBenifitsBenCodeDropDown.Search = HRCashBenefitModel.Search;

            //Fill the combobox with the values of the [Values] attribute; See entity.cs
            //benefit.EmpCashBenefitSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_EmpCashBenefit_AutoGeneratingColumn;
            benefit.EmpCashBenefitSearchControl.ResultsGrid.SelectionChanged += dgv_EmpCashbenefit_SelectionChanged;
            benefit.EmpCashBenefitGrid.SourceUpdated += EmpCashBenefitGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(benefit.EmpCashBenefitSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(benefit.EmpCashBenefitSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            LastBenefitInformation  = benefit.EmpCashBenefitSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_CASH_BENEFIT>;
            benefit.EmpCashBenefitGrid.DataContext = LastBenefitInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_EmpCashBenefit_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMP_CASH_BENEFIT).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }

        
        //When the user selects a row in the datagrid
        void dgv_EmpCashbenefit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (benefit.EmpCashBenefitSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastBenefitInfo = (benefit.EmpCashBenefitSearchControl.ResultsGrid.SelectedItem as HR_EMP_CASH_BENEFIT).Clone() as HR_EMP_CASH_BENEFIT;
        }
        //If the user updates a record, then go to the Update mode automatically
        void EmpCashBenefitGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (benefit.EmpCashBenefitGrid.DataContext == LastBenefitInformation)
            {
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.BtnCancelUpdate.Visibility = Visibility.Visible;
                benefit.EmpCashBenefitSearchControl.IsEnabled = false;
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
                var LastBenefitInfoToSave = (HR_EMP_CASH_BENEFIT)CollectionViewSource.GetDefaultView(benefit.EmpCashBenefitGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                benefit.txt_EmpCashBenifits_Amount.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                benefit.EmpCashBenifitEmpIdDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                benefit.EmpCashBenifitsBenCodeDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                      

                //Checking if all controls are in valid state
                if (!Helper.IsValid(benefit.EmpCashBenefitGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (benefit.BtnAdd.IsChecked == true)
                {
                    
                    var BenefitToSave = (LastBenefitInfoToSave);
                    if (!client.HREmpCashBenefitsExists(BenefitToSave))
                    {
                        ApiAck ack = client.CreateHREmpCashBenefits(BenefitToSave);
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
                    ApiAck ack = client.UpdateHREmpCashBenefits(LastBenefitInfoToSave);
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
                ObservableCollection<HR_EMP_CASH_BENEFIT>  newCollection = new ObservableCollection<HR_EMP_CASH_BENEFIT>() { new HR_EMP_CASH_BENEFIT()};
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.EmpCashBenefitGrid.DataContext = newCollection;
                benefit.EmpCashBenefitSearchControl.IsEnabled = false;
                benefit.btnAdd_text.Text = "Cancel Add";
                benefit.btnSave_text.Text = "Confirm Add";
                benefit.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in benefit.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                benefit.EmpCashBenifitEmpIdDropDown.IsEnabled = true;
                benefit.EmpCashBenifitsBenCodeDropDown.IsEnabled = true;
                     

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
            benefit.EmpCashBenefitGrid.DataContext = LastBenefitInformation;
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.EmpCashBenefitSearchControl.IsEnabled = true;
            benefit.btnAdd_text.Text = "Add";
            benefit.BtnAdd.IsChecked = false;
            benefit.btnSave_text.Text = "Update";
            benefit.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in benefit.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            benefit.EmpCashBenifitEmpIdDropDown.IsEnabled = false;
            benefit.EmpCashBenifitsBenCodeDropDown.IsEnabled = false;
            
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HREmpCashBenefitsQuery query = new HREmpCashBenefitsQuery(); //by default we have an empty query
             if(SearchControl.OptionOne.IsChecked == true)
                 query = new HREmpCashBenefitsQuery() { EmpCode = SearchControl.SearchTextBox.Text };
             if (SearchControl.OptionTwo.IsChecked == true)
                 query = new HREmpCashBenefitsQuery() { BenefitCode = SearchControl.SearchTextBox.Text };
            
            
            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryHREmpCashBenefitsAsync(query, pagesize, pagePosition);

          //  var response = client.ReadAllAccountType(pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_CASH_BENEFIT>(response.Result.ToList<HR_EMP_CASH_BENEFIT>());        
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= LastBenefitInformation.IndexOf(benefit.EmpCashBenefitSearchControl.ResultsGrid.SelectedItem as HR_EMP_CASH_BENEFIT);
           LastBenefitInformation [index]= EditingLastBenefitInfo;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            benefit.EmpCashBenefitSearchControl.IsEnabled = true;
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
                var BenefitToRemove = (HR_EMP_CASH_BENEFIT)CollectionViewSource.GetDefaultView(LastBenefitInformation).CurrentItem;        

                if (client.HREmpCashBenefitsExists(BenefitToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastBenefitInformation.Remove(BenefitToRemove);
                        ApiAck ack = client.DeleteHREmpCashBenefits(BenefitToRemove);
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

