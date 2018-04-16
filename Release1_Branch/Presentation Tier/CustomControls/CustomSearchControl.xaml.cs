using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartBiz.MDM.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomSearchControl.xaml
    /// </summary>
    public partial class CustomSearchControl : UserControl
    {
        
        public CustomSearchControl()
        {
            InitializeComponent();
            CustomPager.SearchControl = this;
            SearchButton.Click += (s, e) =>
            {
                 CustomPager.Search(this);
                 CustomPager.ResetPager();
              
            };
            ResultsGrid.AutoGeneratingColumn += ResultGrid_AutoGeneratingColumns;

        }
        //Stop generating columns for fields which don't have the [DataMember] attribute;See entity.cs
        //This is used to hide fields like EnteredUser,EnteredData etc;
        void ResultGrid_AutoGeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        { 
            Type CollectionType=  (sender as DataGrid).ItemsSource.GetType().GenericTypeArguments[0];
            if (!Attribute.IsDefined(CollectionType.GetProperty(e.PropertyName), typeof(DataMemberAttribute)))
                e.Cancel = true;
            e.Column.Header = Helper.FormatHeaders(e.PropertyName, false);
        }
        private int _noOfOptions =3;
        public int NoOfOptions
        {
            get { return _noOfOptions; }
            set {  
                if(value==1){
                    OptionTwo.Visibility=Visibility.Collapsed;
                    OptionThree.Visibility=Visibility.Collapsed;
                }
                else if(value==2){                  
                    OptionThree.Visibility=Visibility.Collapsed;
                }
                else if(value==3){

                }
                else
                {
                    throw new ArgumentException("No of options should be between 1 and 3");
                }
            }
        }
        public System.Windows.HorizontalAlignment PagerHorizontalAlignment
        {
            get { return CustomPager.HorizontalAlignment; }
            set { CustomPager.HorizontalAlignment = value; }
        }
        public String OptionOneText
        {
            get { return  OptionOne.Content as string; }
            set {  OptionOne.Content = value; }
        }
     
        public String OptionTwoText
        {
            get { return  OptionTwo.Content as string; }
            set {  OptionTwo.Content = value; }
        }
        
        public String OptionThreeText
        {
            get { return  OptionThree.Content as string; }
            set {  OptionThree.Content = value; }
        }
        public void ResetPager()
        {
            CustomPager.ResetPager();
        }
        public void ResetSearchControl()
        {
            this.SearchTextBox.Text = null;
            OptionOne.IsChecked = false;
            OptionTwo.IsChecked = false;
            OptionThree.IsChecked = false;
            ResetPager();
        }
        public Action<CustomSearchControl> Search
        {
            set { CustomPager.Search = value; }
            get { return CustomPager.Search; }
        }
        public int PagePosition
        {
            get { return CustomPager.PagePosition; }
            set { CustomPager.PagePosition = value; }
        }
        public int PageCount
        {
            get { return CustomPager.PageCount; }
            set { CustomPager.PageCount = value; }
        }
        public int PageSize
        {

            get { return CustomPager.PageSize; }
            set { CustomPager.PageSize = value; }
        }
        public int ResultCount
        {
            get { return CustomPager.ResultCount; }
            set { CustomPager.ResultCount = value; }
        }


        }
       
    }


