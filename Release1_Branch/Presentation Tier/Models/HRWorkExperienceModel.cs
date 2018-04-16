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
    class HRWorkExperienceModel
    {
        ObservableCollection<HR_EMP_WORK_EXPERIENCE> HRWorkExperiences; //These will be shown in the datagrid
        HREmployeeWorkExperience HREmployeeWorkExperienceUI; //Reference to Transaction Interface
        HR_EMP_WORK_EXPERIENCE EditingHRWorkExperience; //Stores what we're editing right now, Incase we need to cancel the edit
        public HRWorkExperienceModel(HREmployeeWorkExperience HREmployeeWorkExperienceUI)
        {
            this.HREmployeeWorkExperienceUI = HREmployeeWorkExperienceUI;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            HREmployeeWorkExperienceUI.EMPWORKSearchControl.Search = Search;
            HREmployeeWorkExperienceUI.HREmployeeWEDropDown.Search = EmployeeModel.Search;  

           //HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_LastTransactionInfo_AutoGeneratingColumn;
            
            HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.SelectionChanged += dgv_LastTransactionInfo_SelectionChanged;

            HREmployeeWorkExperienceUI.EMPWORKGrid.SourceUpdated += EMPWORKGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid, ItemSourceChanged);
            }            
            Search(HREmployeeWorkExperienceUI.EMPWORKSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            HRWorkExperiences = HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_WORK_EXPERIENCE>;
            HREmployeeWorkExperienceUI.EMPWORKGrid.DataContext = HRWorkExperiences;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_LastTransactionInfo_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMP_WORK_EXPERIENCE).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
      
        //When the user selects a row in the datagrid
        void dgv_LastTransactionInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.SelectedItem != null)
                EditingHRWorkExperience = (HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.SelectedItem as HR_EMP_WORK_EXPERIENCE).Clone() as HR_EMP_WORK_EXPERIENCE;
        }
        //If the user updates a record, then go to the Update mode automatically
        void EMPWORKGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (HREmployeeWorkExperienceUI.EMPWORKGrid.DataContext == HRWorkExperiences)
            {
                HREmployeeWorkExperienceUI.BtnSave.Visibility = Visibility.Visible;
                HREmployeeWorkExperienceUI.BtnCancelUpdate.Visibility = Visibility.Visible;
                HREmployeeWorkExperienceUI.EMPWORKSearchControl.IsEnabled = false;
                HREmployeeWorkExperienceUI.BtnAdd.Visibility = Visibility.Collapsed;
                HREmployeeWorkExperienceUI.btnDelete.Visibility = Visibility.Collapsed;
               
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var EmployeeExpToSave = (HR_EMP_WORK_EXPERIENCE)CollectionViewSource.GetDefaultView(HREmployeeWorkExperienceUI.EMPWORKGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                HREmployeeWorkExperienceUI.HREmployeeWEDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                HREmployeeWorkExperienceUI.txt_EMPWORK_EEXP_TELEPHONE.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                HREmployeeWorkExperienceUI.txt_EMPWORK_EEXP_EMAIL.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                HREmployeeWorkExperienceUI.dpk_EMPWORK_EEXP_FROM_DATE.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
                HREmployeeWorkExperienceUI.dpk_EMPWORK_EEXP_TO_DATE.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();


                //Checking if all controls are in valid state
                if (!Helper.IsValid(HREmployeeWorkExperienceUI.EMPWORKGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (HREmployeeWorkExperienceUI.BtnAdd.IsChecked == true)
                {
                    
                 
                    if (!client.EMPWORKExists(EmployeeExpToSave))
                    {
                        ApiAck ack = client.CreateEMPWORK(EmployeeExpToSave);
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
                    ApiAck ack = client.UpdateEMPWORK(EmployeeExpToSave);
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
            if (HREmployeeWorkExperienceUI.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_WORK_EXPERIENCE>  newCollection = new ObservableCollection<HR_EMP_WORK_EXPERIENCE>() { new HR_EMP_WORK_EXPERIENCE()};
                HREmployeeWorkExperienceUI.BtnSave.Visibility = Visibility.Visible;
                HREmployeeWorkExperienceUI.EMPWORKGrid.DataContext = newCollection;
                HREmployeeWorkExperienceUI.EMPWORKSearchControl.IsEnabled = false;
                HREmployeeWorkExperienceUI.btnAdd_text.Text = "Cancel Add";
                HREmployeeWorkExperienceUI.btnSave_text.Text = "Confirm Add";
                HREmployeeWorkExperienceUI.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
             
                HREmployeeWorkExperienceUI.HREmployeeWEDropDown.IsEnabled = true;
                  

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
            HREmployeeWorkExperienceUI.EMPWORKGrid.DataContext = HRWorkExperiences;              
            HREmployeeWorkExperienceUI.BtnSave.Visibility = Visibility.Collapsed;
            HREmployeeWorkExperienceUI.EMPWORKSearchControl.IsEnabled = true;
            HREmployeeWorkExperienceUI.btnAdd_text.Text = "Add";
            HREmployeeWorkExperienceUI.BtnAdd.IsChecked = false;
            HREmployeeWorkExperienceUI.btnSave_text.Text = "Update";
            HREmployeeWorkExperienceUI.btnDelete.Visibility = System.Windows.Visibility.Visible;
        
            HREmployeeWorkExperienceUI.HREmployeeWEDropDown.IsEnabled = false;
           
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HRWorkExperienceQuery query = new HRWorkExperienceQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new HRWorkExperienceQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new HRWorkExperienceQuery() { Company = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionThree.IsChecked == true)
            {
                int value=0;
                if (int.TryParse(SearchControl.SearchTextBox.Text, out value))
                {
                    query = new HRWorkExperienceQuery() { Years = value };
                }
               
            }
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEMPWORKEXPERIENCEAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_WORK_EXPERIENCE>(response.Result.ToList<HR_EMP_WORK_EXPERIENCE>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= HRWorkExperiences.IndexOf(HREmployeeWorkExperienceUI.EMPWORKSearchControl.ResultsGrid.SelectedItem as HR_EMP_WORK_EXPERIENCE);
           HRWorkExperiences [index]= EditingHRWorkExperience;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            HREmployeeWorkExperienceUI.BtnSave.Visibility = Visibility.Collapsed;
            HREmployeeWorkExperienceUI.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            HREmployeeWorkExperienceUI.EMPWORKSearchControl.IsEnabled = true;
            HREmployeeWorkExperienceUI.BtnAdd.Visibility = Visibility.Visible;
            HREmployeeWorkExperienceUI.btnDelete.Visibility = Visibility.Visible;
          
        }     
        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var eexptoremove = (HR_EMP_WORK_EXPERIENCE)CollectionViewSource.GetDefaultView(HRWorkExperiences).CurrentItem;        

                if (client.EMPWORKExists(eexptoremove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        HRWorkExperiences.Remove(eexptoremove);
                        ApiAck ack = client.DeleteEMPWORK(eexptoremove);
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

