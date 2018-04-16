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
    class TrainingModel
    {
        ObservableCollection<HR_EMP_TRAININGS> TrainingInformation; //These will be shown in the datagrid
        Training Training; //Reference to Transaction Interface
        HR_EMP_TRAININGS EditingTrainingInfomation; //Stores what we're editing right now, Incase we need to cancel the edit
        public TrainingModel(Training Training)
        {
            this.Training = Training;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            Training.TrainingSearchControl.Search = Search;
            Training.EmployeeNumberDropDown.Search = EmployeeModel.Search;

            Training.cmb_TrainingStatus.ItemsSource = Helper.getItemSource(typeof(HR_EMP_TRAININGS), "TN_STATUS");

            //Training.TrainingSearchControl.ResultsGrid.AutoGeneratingColumn += dgv_Training_AutoGeneratingColumn;

            Training.TrainingSearchControl.ResultsGrid.SelectionChanged += dgv_Training_SelectionChanged;

            Training.TrainingGrid.SourceUpdated += TrainingGrid_SourceUpdated;            
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {                
                dpd.AddValueChanged(Training.TrainingSearchControl.ResultsGrid, ItemSourceChanged);               
            }
            
            Search(Training.TrainingSearchControl);  //Initial Search to fill datagrid with values at startup         
        }
        //This is a event which fires whenever newitems arrive at the datagrid
        void ItemSourceChanged(object sender, EventArgs e)
        {
            TrainingInformation = Training.TrainingSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_TRAININGS>;
            Training.TrainingGrid.DataContext = TrainingInformation;
        }  
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void dgv_Training_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!Attribute.IsDefined(typeof(HR_EMP_TRAININGS).GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }        
        //When the user selects a row in the datagrid
        void dgv_Training_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Training.TrainingSearchControl.ResultsGrid.SelectedItem!=null)
                EditingTrainingInfomation = (Training.TrainingSearchControl.ResultsGrid.SelectedItem as HR_EMP_TRAININGS).Clone() as HR_EMP_TRAININGS;            
        }
        //If the user updates a record, then go to the Update mode automatically
        void TrainingGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (Training.TrainingGrid.DataContext == TrainingInformation)
            {
                Training.BtnSave.Visibility = Visibility.Visible;
                Training.BtnCancelUpdate.Visibility = Visibility.Visible;
                Training.TrainingSearchControl.IsEnabled = false;
                Training.BtnAdd.Visibility = Visibility.Collapsed;
                Training.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in Training.TbPage.Items)
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
                var SaveTrainingInformation = (HR_EMP_TRAININGS)CollectionViewSource.GetDefaultView(Training.TrainingGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                /*EmergencyContact.txt_FullName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_Relationship.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_PermanentAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_OfficialAddress.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneHome.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneMobile.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                EmergencyContact.txt_TelephoneOffice.GetBindingExpression(TextBox.TextProperty).UpdateSource();*/
                        
                //Checking if all controls are in valid state
                if (!Helper.IsValid(Training.TrainingGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (Training.BtnAdd.IsChecked == true)
                {                                    
                    if (!client.ExistTraining(SaveTrainingInformation))
                    {
                        ApiAck ack = client.CreateTraining(SaveTrainingInformation);
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
                    ApiAck ack = client.UpdateTraining(SaveTrainingInformation);
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
            if (Training.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_TRAININGS>  newCollection = new ObservableCollection<HR_EMP_TRAININGS>() { new HR_EMP_TRAININGS()};
                Training.BtnSave.Visibility = Visibility.Visible;
                Training.TrainingGrid.DataContext = newCollection;
                Training.TrainingSearchControl.IsEnabled = false;
                Training.btnAdd_text.Text = "Cancel Add";
                Training.btnSave_text.Text = "Confirm Add";
                Training.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in Training.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                Training.EmployeeNumberDropDown.IsEnabled = true;
                Training.txt_TrainingID.IsEnabled = true;     
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
            Training.TrainingGrid.DataContext = TrainingInformation;
            Training.BtnSave.Visibility = Visibility.Collapsed;
            Training.TrainingSearchControl.IsEnabled = true;
            Training.btnAdd_text.Text = "Add";
            Training.BtnAdd.IsChecked = false;
            Training.btnSave_text.Text = "Update";
            Training.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in Training.TbPage.Items)
            {
                t.IsEnabled = true;
            }
            Training.EmployeeNumberDropDown.IsEnabled = false;
            Training.txt_TrainingID.IsEnabled = false;
        }        
    
        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            TrainingQuery query = new TrainingQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
            {
                query = new TrainingQuery() { EmployeeNumber = SearchControl.SearchTextBox.Text };
            }
            else if (SearchControl.OptionTwo.IsChecked == true)
            {
                query = new TrainingQuery() { TrainingID = Int32.Parse(SearchControl.SearchTextBox.Text) };
            } 
          
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryTrainingAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results         
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_TRAININGS>(response.Result.ToList<HR_EMP_TRAININGS>()); ;         
        }
       
        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {           
           int index= TrainingInformation.IndexOf(Training.TrainingSearchControl.ResultsGrid.SelectedItem as HR_EMP_TRAININGS);
           TrainingInformation [index]= EditingTrainingInfomation;
           CompleteUpdate();
        }
        //Update has been completed, Reenable textboxes,Datagrid etc;
        public void CompleteUpdate()
        {
            Training.BtnSave.Visibility = Visibility.Collapsed;
            Training.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            Training.TrainingSearchControl.IsEnabled = true;
            Training.BtnAdd.Visibility = Visibility.Visible;
            Training.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in Training.TbPage.Items)
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
                var RemoveTrainingInformation = (HR_EMP_TRAININGS)CollectionViewSource.GetDefaultView(Training.TrainingGrid.DataContext).CurrentItem;        

                if (client.ExistTraining(RemoveTrainingInformation))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        TrainingInformation.Remove(RemoveTrainingInformation);
                        ApiAck ack = client.DeleteTraining(RemoveTrainingInformation);
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

