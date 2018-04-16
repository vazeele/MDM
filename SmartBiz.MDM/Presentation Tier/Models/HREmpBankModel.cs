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
    internal class HREmpBankModel
    {
        private ObservableCollection<HR_EMP_BANK> LastBenefitInformation; //These will be shown in the datagrid
        private HREmployeeBank bank; //Reference to Account Interface
        private HR_EMP_BANK EditingLastBenefitInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public HREmpBankModel(HREmployeeBank bank)
        {
            this.bank = bank;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            bank.EMPBANKSearchControl.Search = Search;

            bank.HRBankBranchDropDown.Search = HRBankBranchModel.Search;
            bank.HRBankDropDown.Search = HRBankModel.Search;

            bank.EMPBANKSearchControl.ResultsGrid.SelectedCellsChanged += dgv_Bank_SelectionChanged;
            bank.EMPBANKGrid.SourceUpdated += EMPBANKGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(bank.EMPBANKSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(bank.EMPBANKSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastBenefitInformation = bank.EMPBANKSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_BANK>;
            bank.EMPBANKGrid.DataContext = LastBenefitInformation;
            if (LastBenefitInformation.Count>0)
            {
                EditingLastBenefitInfo = LastBenefitInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_Bank_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (bank.EMPBANKSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastBenefitInfo = (bank.EMPBANKSearchControl.ResultsGrid.SelectedItem as HR_EMP_BANK).Clone() as HR_EMP_BANK;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EMPBANKGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (bank.EMPBANKGrid.DataContext == LastBenefitInformation)
            {
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.BtnCancelUpdate.Visibility = Visibility.Visible;
                bank.EMPBANKSearchControl.IsEnabled = false;
                bank.BtnAdd.Visibility = Visibility.Collapsed;
                bank.btnDelete.Visibility = Visibility.Collapsed;
                
            }
        }

        public void Save()
        {   //Get an instance of the service using Helper class
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var LastBenefitInfoToSave = (HR_EMP_BANK)CollectionViewSource.GetDefaultView(bank.EMPBANKGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                bank.HREmployeeDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                bank.HRBankBranchDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                bank.HRBankDropDown.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(bank.EMPBANKGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (bank.BtnAdd.IsChecked == true)
                {
                    var BenefitToSave = (LastBenefitInfoToSave);
                    if (!client.EMPBANKExists(BenefitToSave))
                    {
                        ApiAck ack = client.CreateEMPBANK(BenefitToSave);
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
                    ApiAck ack = client.UpdateEMPBANK(LastBenefitInfoToSave);
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
            if (bank.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_EMP_BANK> newCollection = new ObservableCollection<HR_EMP_BANK>() { new HR_EMP_BANK() };
                bank.BtnSave.Visibility = Visibility.Visible;
                bank.EMPBANKGrid.DataContext = newCollection;
                bank.EMPBANKSearchControl.IsEnabled = false;
                bank.btnAdd_text.Text = "Cancel Add";
                bank.btnSave_text.Text = "Confirm Add";
                bank.btnDelete.Visibility = System.Windows.Visibility.Collapsed;

                bank.HREmployeeDropDown.IsEnabled = true;
                bank.HRBankBranchDropDown.IsEnabled = true;
                bank.HRBankDropDown.IsEnabled = true;
                
                
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
            bank.EMPBANKGrid.DataContext = LastBenefitInformation;
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.EMPBANKSearchControl.IsEnabled = true;
            bank.btnAdd_text.Text = "Add";
            bank.BtnAdd.IsChecked = false;
            bank.btnSave_text.Text = "Update";
            bank.btnDelete.Visibility = System.Windows.Visibility.Visible;


            bank.HREmployeeDropDown.IsEnabled = false;
            bank.HRBankBranchDropDown.IsEnabled = false;
            bank.HRBankDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            EmpBankQuery query = new EmpBankQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new EmpBankQuery() { EmpNumber = SearchControl.SearchTextBox.Text };
            if (SearchControl.OptionTwo.IsChecked == true)
                query = new EmpBankQuery() { BankCode = SearchControl.SearchTextBox.Text };
            if (SearchControl.OptionThree.IsChecked == true)
                query = new EmpBankQuery() { BranchCode = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryEMPBANKAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_BANK>(response.Result.ToList<HR_EMP_BANK>());
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastBenefitInformation.IndexOf(bank.EMPBANKSearchControl.ResultsGrid.SelectedItem as HR_EMP_BANK);
            LastBenefitInformation[index] = EditingLastBenefitInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            bank.BtnSave.Visibility = Visibility.Collapsed;
            bank.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            bank.EMPBANKSearchControl.IsEnabled = true;
            bank.BtnAdd.Visibility = Visibility.Visible;
            bank.btnDelete.Visibility = Visibility.Visible;
            
        }

        //User pressed delete
        public void Delete()
        {
            var client = Helper.getServiceClient();
            try
            {   //Get the current object in the EditGrid
                var BenefitToRemove = (HR_EMP_BANK)CollectionViewSource.GetDefaultView(LastBenefitInformation).CurrentItem;

                if (client.EMPBANKExists(BenefitToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastBenefitInformation.Remove(BenefitToRemove);
                        ApiAck ack = client.DeleteEMPBANK(BenefitToRemove);
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