﻿using SmartBiz.MDM.Presentation.CustomControls;
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
    internal class HREmpNonCashBenefitModel
    {
        private ObservableCollection<HR_EMP_NONCASH_BENEFIT> LastBenefitInformation; //These will be shown in the datagrid
        private HRBenefits benefit; //Reference to HR Benefit Interface
        private HR_EMP_NONCASH_BENEFIT EditingLastBenefitInfo; //Stores what we're editing right now, in case we need to cancel the edit

        public HREmpNonCashBenefitModel(HRBenefits benefit)
        {
            this.benefit = benefit;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            benefit.EmpNonCashBenefitSearchControl.Search = Search;
            benefit.EmpNonCashBenifitEmpIdDropDown.Search = EmployeeModel.Search;
            benefit.EmpNonCashBenifitsBenCodeDropDown.Search = HRNonCashBenefitModel.Search;

            //Fill the combobox with the values of the [Values] attribute; See entity.cs
            benefit.cmb_EmpNonCashBenifits_AssessManagementFlag.ItemsSource = Helper.getItemSource(typeof(HR_EMP_NONCASH_BENEFIT), "ENBEN_ASSES_MGMT_FLG");
            benefit.EmpNonCashBenefitSearchControl.ResultsGrid.SelectedCellsChanged += dgv_EmpNonCashbenefit_SelectionChanged;
            benefit.EmpNonCashBenefitGrid.SourceUpdated += EmpNonCashBenefitGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(benefit.EmpNonCashBenefitSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(benefit.EmpNonCashBenefitSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastBenefitInformation = benefit.EmpNonCashBenefitSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_EMP_NONCASH_BENEFIT>;
            benefit.EmpNonCashBenefitGrid.DataContext = LastBenefitInformation;
            if (LastBenefitInformation.Count>0)
            {
                EditingLastBenefitInfo = LastBenefitInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_EmpNonCashbenefit_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (benefit.EmpNonCashBenefitSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastBenefitInfo = (benefit.EmpNonCashBenefitSearchControl.ResultsGrid.SelectedItem as HR_EMP_NONCASH_BENEFIT).Clone() as HR_EMP_NONCASH_BENEFIT;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void EmpNonCashBenefitGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (benefit.EmpNonCashBenefitGrid.DataContext == LastBenefitInformation)
            {
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.BtnCancelUpdate.Visibility = Visibility.Visible;
                benefit.EmpNonCashBenefitSearchControl.IsEnabled = false;
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
                var LastBenefitInfoToSave = (HR_EMP_NONCASH_BENEFIT)CollectionViewSource.GetDefaultView(benefit.EmpNonCashBenefitGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                benefit.txt_EmpCashBenifits_Amount.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                benefit.EmpCashBenifitEmpIdDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();
                benefit.EmpCashBenifitsBenCodeDropDown.GetBindingExpression(CustomSearchDropDown.CustomSelectedItemProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(benefit.EmpNonCashBenefitGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (benefit.BtnAdd.IsChecked == true)
                {
                    var BenefitToSave = (LastBenefitInfoToSave);
                    if (!client.HREmpNonCashBenefitsExists(BenefitToSave))
                    {
                        ApiAck ack = client.CreateHREmpNonCashBenefits(BenefitToSave);
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
                    ApiAck ack = client.UpdateHREmpNonCashBenefits(LastBenefitInfoToSave);
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
                ObservableCollection<HR_EMP_NONCASH_BENEFIT> newCollection = new ObservableCollection<HR_EMP_NONCASH_BENEFIT>() { new HR_EMP_NONCASH_BENEFIT() };
                benefit.BtnSave.Visibility = Visibility.Visible;
                benefit.EmpNonCashBenefitGrid.DataContext = newCollection;
                benefit.EmpNonCashBenefitSearchControl.IsEnabled = false;
                benefit.cmb_EmpNonCashBenifits_AssessManagementFlag.SelectedIndex = 0;
                benefit.btnAdd_text.Text = "Cancel Add";
                benefit.btnSave_text.Text = "Confirm Add";
                benefit.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in benefit.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                benefit.EmpNonCashBenifitEmpIdDropDown.IsEnabled = true;
                benefit.EmpNonCashBenifitsBenCodeDropDown.IsEnabled = true;
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
            benefit.EmpNonCashBenefitGrid.DataContext = LastBenefitInformation;
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.EmpNonCashBenefitSearchControl.IsEnabled = true;
            benefit.btnAdd_text.Text = "Add";
            benefit.BtnAdd.IsChecked = false;
            benefit.btnSave_text.Text = "Update";
            benefit.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in benefit.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            benefit.EmpNonCashBenifitEmpIdDropDown.IsEnabled = false;
            benefit.EmpNonCashBenifitsBenCodeDropDown.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HREmpNonCashBenefitsQuery query = new HREmpNonCashBenefitsQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new HREmpNonCashBenefitsQuery() { EmpCode = SearchControl.SearchTextBox.Text };
            if (SearchControl.OptionTwo.IsChecked == true)
                query = new HREmpNonCashBenefitsQuery() { NonBenefitCode = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryHREmpNonCashBenefitsAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_EMP_NONCASH_BENEFIT>(response.Result.ToList<HR_EMP_NONCASH_BENEFIT>());
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastBenefitInformation.IndexOf(benefit.EmpNonCashBenefitSearchControl.ResultsGrid.SelectedItem as HR_EMP_NONCASH_BENEFIT);
            LastBenefitInformation[index] = EditingLastBenefitInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            benefit.BtnSave.Visibility = Visibility.Collapsed;
            benefit.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            benefit.EmpNonCashBenefitSearchControl.IsEnabled = true;
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
                var BenefitToRemove = (HR_EMP_NONCASH_BENEFIT)CollectionViewSource.GetDefaultView(LastBenefitInformation).CurrentItem;

                if (client.HREmpNonCashBenefitsExists(BenefitToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastBenefitInformation.Remove(BenefitToRemove);
                        ApiAck ack = client.DeleteHREmpNonCashBenefits(BenefitToRemove);
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