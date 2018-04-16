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
    internal class HRLanguageModel
    {
        private ObservableCollection<HR_LANGUAGE> LastLanguageInformation; //These will be shown in the datagrid
        private HRLanguages language; //Reference to HR Language Interface
        private HR_LANGUAGE EditingLastLanguageInfo; //Stores what we're editing right now, Incase we need to cancel the edit

        public HRLanguageModel(HRLanguages language)
        {
            this.language = language;

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            language.HRLanguageSearchControl.Search = Search;

            
            language.HRLanguageSearchControl.ResultsGrid.SelectedCellsChanged += dgv_HRLanguage_SelectionChanged;
            language.LanguageGrid.SourceUpdated += LanguageGrid_SourceUpdated;
            //This is a event which fires whenever newitems arrive at the datagrid
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(language.HRLanguageSearchControl.ResultsGrid, ItemSourceChanged);
            }
            Search(language.HRLanguageSearchControl);  //Initial Search to fill datagrid with values at startup
        }

        //This is a event which fires whenever newitems arrive at the datagrid
        private void ItemSourceChanged(object sender, EventArgs e)
        {
            LastLanguageInformation = language.HRLanguageSearchControl.ResultsGrid.ItemsSource as ObservableCollection<HR_LANGUAGE>;
            language.LanguageGrid.DataContext = LastLanguageInformation;
            if (LastLanguageInformation.Count>0)
            {
                EditingLastLanguageInfo = LastLanguageInformation[0].Clone();
            }
        }

        //When the user selects a row in the datagrid
        private void dgv_HRLanguage_SelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (language.HRLanguageSearchControl.ResultsGrid.SelectedItem != null)
                EditingLastLanguageInfo = (language.HRLanguageSearchControl.ResultsGrid.SelectedItem as HR_LANGUAGE).Clone() as HR_LANGUAGE;
        }

        //If the user updates a record, then go to the Update mode automatically
        private void LanguageGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (language.LanguageGrid.DataContext == LastLanguageInformation)
            {
                language.BtnSave.Visibility = Visibility.Visible;
                language.BtnCancelUpdate.Visibility = Visibility.Visible;
                language.HRLanguageSearchControl.IsEnabled = false;
                language.BtnAdd.Visibility = Visibility.Collapsed;
                language.btnDelete.Visibility = Visibility.Collapsed;
                foreach (TabItem t in language.TbPage.Items)
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
                var LastLanguageInfoToSave = (HR_LANGUAGE)CollectionViewSource.GetDefaultView(language.LanguageGrid.DataContext).CurrentItem;
                //This is used incase a user never enters a textbox but still needs validating
                language.txt_Language_LangCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                language.txt_Language_Name.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                //Checking if all controls are in valid state
                if (!Helper.IsValid(language.LanguageGrid))
                {
                    MessageBox.Show("Please fix errors before continuing", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                //A user is trying to add a record here
                if (language.BtnAdd.IsChecked == true)
                {
                    var LanguageToSave = (LastLanguageInfoToSave);
                    if (!client.HRLanguageExists(LanguageToSave))
                    {
                        ApiAck ack = client.CreateHRLanguage(LanguageToSave);
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
                    ApiAck ack = client.UpdateHRLanguage(LastLanguageInfoToSave);
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
            if (language.BtnAdd.IsChecked == true)
            {   //Unbind the existing collection and put in the object we're trying to add
                ObservableCollection<HR_LANGUAGE> newCollection = new ObservableCollection<HR_LANGUAGE>() { new HR_LANGUAGE() };
                language.BtnSave.Visibility = Visibility.Visible;
                language.LanguageGrid.DataContext = newCollection;
                language.HRLanguageSearchControl.IsEnabled = false;
                language.btnAdd_text.Text = "Cancel Add";
                language.btnSave_text.Text = "Confirm Add";
                language.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
                foreach (TabItem t in language.TbPage.Items)
                {
                    if (!t.IsSelected)
                        t.IsEnabled = false;
                }
                language.txt_Language_LangCode.IsEnabled = true;
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
            language.LanguageGrid.DataContext = LastLanguageInformation;
            language.BtnSave.Visibility = Visibility.Collapsed;
            language.HRLanguageSearchControl.IsEnabled = true;
            language.btnAdd_text.Text = "Add";
            language.BtnAdd.IsChecked = false;
            language.btnSave_text.Text = "Update";
            language.btnDelete.Visibility = System.Windows.Visibility.Visible;
            foreach (TabItem t in language.TbPage.Items)
            {
                t.IsEnabled = true;
            }

            language.txt_Language_LangCode.IsEnabled = false;
        }

        public static async void Search(CustomSearchControl SearchControl)
        {
            var client = Helper.getServiceClient();
            HRLanguageQuery query = new HRLanguageQuery(); //by default we have an empty query
            if (SearchControl.OptionOne.IsChecked == true)
                query = new HRLanguageQuery() { LangCode = SearchControl.SearchTextBox.Text };
            else
                query = new HRLanguageQuery() { Name = SearchControl.SearchTextBox.Text };

            //The search control needs to know the method it has to use inorder to search. So we pass a method
            SearchControl.Search = Search;
            int pagesize = SearchControl.PageSize;
            int pagePosition = SearchControl.PagePosition;
            var response = await client.QueryHRLanguageAsync(query, pagesize, pagePosition);

            //No response; exit
            if (response == null)
            {
                MessageBox.Show("Service isn't responding, please try again later", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            SearchControl.ResultCount = response.TotalResultCount;
            //Fill the datagrid with the results
            SearchControl.ResultsGrid.ItemsSource = new ObservableCollection<HR_LANGUAGE>(response.Result.ToList<HR_LANGUAGE>());
        }

        //Update has been cancelled by user; Revert any changes
        public void CancelUpdate()
        {
            int index = LastLanguageInformation.IndexOf(language.HRLanguageSearchControl.ResultsGrid.SelectedItem as HR_LANGUAGE);
            LastLanguageInformation[index] = EditingLastLanguageInfo;
            CompleteUpdate();
        }

        //Update has been completed, Reenable textboxes,Datagrid etc
        public void CompleteUpdate()
        {
            language.BtnSave.Visibility = Visibility.Collapsed;
            language.BtnCancelUpdate.Visibility = Visibility.Collapsed;
            language.HRLanguageSearchControl.IsEnabled = true;
            language.BtnAdd.Visibility = Visibility.Visible;
            language.btnDelete.Visibility = Visibility.Visible;
            foreach (TabItem t in language.TbPage.Items)
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
                var LanguageToRemove = (HR_LANGUAGE)CollectionViewSource.GetDefaultView(LastLanguageInformation).CurrentItem;

                if (client.HRLanguageExists(LanguageToRemove))
                {
                    var ans = MessageBox.Show("Are you sure you want to delete this record?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (ans == MessageBoxResult.Yes)
                    {
                        LastLanguageInformation.Remove(LanguageToRemove);
                        ApiAck ack = client.DeleteHRLanguage(LanguageToRemove);
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