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
    internal class HREmpPassportModel
    {
        private ObservableCollection<HR_EMP_PASSPORT> LastPassportInformation; //These will be shown in the datagrid
        private HREmployeePassport passport; //Reference to Employee Passport
        private HR_EMP_PASSPORT EditingLastPassportInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public HREmpPassportModel(HREmployeePassport passport)
        {
            this.passport = passport;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            passport.EMPPASSPORTSearchControl.Search = EmployeeModel.Search;

            passport.HREmployeeDropDown.Search = EmployeeModel.Search;
            passport.HRCountryDropDown.Search = HRCountryModel.Search;

            passport.EMPPASSPORTSearchControl.ResultsGrid.SelectedCellsChanged += dgv_Passport_SelectionChanged;
            passport.EMPPASSPORTGrid.SourceUpdated += EMPPASSPORTGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(passport.EMPPASSPORTSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(passport.EMPPASSPORTSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastPassportInformation = passport.EMPPASSPORTSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_PASSPORT>;
            passport.EMPPASSPORTGrid.DataContext = LastPassportInformation;
            if (LastPassportInformation.Count > 0)
            {
                EditingLastPassportInfo = LastPassportInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Passport_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (passport.EMPPASSPORTSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastPassportInfo = (passport.EMPPASSPORTSearchControl.ResultsGrid.SelectedItem as HR_EMP_PASSPORT).Clone() as HR_EMP_PASSPORT;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EMPPASSPORTGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (passport.EMPPASSPORTGrid.DataContext == LastPassportInformation)
            {
                passport.BtnSave.Visibility = Visibility.Visible;
                passport.BtnCancelUpdate.Visibility = Visibility.Visible;
                passport.EMPPASSPORTSearchControl.IsEnabled = false;
                passport.BtnAdd.Visibility = Visibility.Collapsed;
                passport.btnDelete.Visibility = Visibility.Collapsed;

            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var LastPassportInfoToSave = (HR_EMP_PASSPORT)CollectionViewSource.GetDefaultView(passport.EMPPASSPORTGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                passport.HREmployeeDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                passport.txt_EMPPASSPORT_EP_SEQNO.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(passport.EMPPASSPORTGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (passport.BtnAdd.IsChecked == true)
                {
                    var BenefitToSave = (LastPassportInfoToSave);
                    if (!client.EMPPASSPORTExists(BenefitToSave))
                    {
                        ApiAck ack = client.CreateEMPPASSPORT(BenefitToSave);
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
                    ApiAck ack = client.UpdateEMPPASSPORT(LastPassportInfoToSave);
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
            if (passport.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_PASSPORT> newCollection = new ObservableCollection<HR_EMP_PASSPORT>() { new HR_EMP_PASSPORT() };
                passport.BtnSave.Visibility = Visibility.Visible;
                passport.EMPPASSPORTGrid.DataContext = newCollection;
                passport.EMPPASSPORTSearchControl.IsEnabled = false;
                passport.btnAdd_text.Text = "Cancel Add";
                passport.btnSave_text.Text = "Confirm Add";
                passport.btnDelete.Visibility = System.Windows.Visibility.Collapsed;

                passport.HREmployeeDropDown.IsEnabled = true;
                passport.txt_EMPPASSPORT_EP_SEQNO.IsEnabled = true;



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
            passport.EMPPASSPORTGrid.DataContext = LastPassportInformation;
            passport.BtnSave.Visibility = Visibility.Collapsed;
            passport.EMPPASSPORTSearchControl.IsEnabled = true;
            passport.btnAdd_text.Text = "Add";
            passport.BtnAdd.IsChecked = false;
            passport.btnSave_text.Text = "Update";
            passport.btnDelete.Visibility = System.Windows.Visibility.Visible;


            passport.HREmployeeDropDown.IsEnabled = false;
            passport.txt_EMPPASSPORT_EP_SEQNO.IsEnabled = false;

        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmpPassportQuery query = new EmpPassportQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new EmpPassportQuery() { EmpNumber = SearchControl.SearchTextBox.Text };
            if (SearchControl.OptionTwo.IsChecked == true)
                query = new EmpPassportQuery() { SequenceNum = Decimal.Parse(SearchControl.SearchTextBox.Text) };
            if (SearchControl.OptionThree.IsChecked == true)
                query = new EmpPassportQuery() { passportNum = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEMPPASSPORTAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_PASSPORT>(response.Result.ToList<HR_EMP_PASSPORT>());
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastPassportInformation.IndexOf(passport.EMPPASSPORTSearchControl.ResultsGrid.SelectedItem as HR_EMP_PASSPORT);
            LastPassportInformation[index] = EditingLastPassportInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            passport.BtnSave.Visibility = Visibility.Collapsed;
            passport.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            passport.EMPPASSPORTSearchControl.IsEnabled = true;
            passport.BtnAdd.Visibility = Visibility.Visible;
            passport.btnDelete.Visibility = Visibility.Visible;

        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var BenefitToRemove = (HR_EMP_PASSPORT)CollectionViewSource.GetDefaultView(LastPassportInformation).CurrentItem;

                if (client.EMPPASSPORTExists(BenefitToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastPassportInformation.Remove(BenefitToRemove);
                        ApiAck ack = client.DeleteEMPPASSPORT(BenefitToRemove);
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