using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomSearchDropDown.xaml
    /// </summary>
    public partial class CustomSearchDropDown : UserControl
    {
        public static readonly DependencyProperty CustomSelectedItemProperty =
        DependencyProperty.Register("CustomSelectedItem", typeof(Object), typeof(CustomSearchDropDown), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CustomSelectedItemChanged)));

        public System.Windows.HorizontalAlignment PagerHorizontalAlignment
        {
            get { return SearchControl.CustomPager.HorizontalAlignment; }
            set { SearchControl.CustomPager.HorizontalAlignment = value; }
        }

        public int NoOfOptions
        {
            get { return SearchControl.NoOfOptions; }
            set { SearchControl.NoOfOptions = value; }
        }

        public String OptionOneText
        {
            get { return SearchControl.OptionOne.Content as string; }
            set { SearchControl.OptionOne.Content = value; }
        }

        public String OptionTwoText
        {
            get { return SearchControl.OptionTwo.Content as string; }
            set { SearchControl.OptionTwo.Content = value; }
        }

        public String OptionThreeText
        {
            get { return SearchControl.OptionThree.Content as string; }
            set { SearchControl.OptionThree.Content = value; }
        }

        public RadioButton OptionOne
        {
            get { return SearchControl.OptionOne; }
        }

        public RadioButton OptionTwo
        {
            get { return SearchControl.OptionTwo; }
        }

        public RadioButton OptionThree
        {
            get { return SearchControl.OptionThree; }
        }

        public Button SearchButton
        {
            get { return SearchControl.SearchButton; }
        }

        public TextBox SearchTextBox
        {
            get { return SearchControl.SearchTextBox; }
        }

        public DataGrid ResultsGrid
        {
            get { return SearchControl.ResultsGrid; }
        }

        public Object CustomSelectedItem
        {
            get
            {
                return GetValue(CustomSelectedItemProperty);
            }
            set
            {
                SetValue(CustomSelectedItemProperty, value);
            }
        }

        public CustomSearchControl CustomSearchControl
        {
            get
            {
                return SearchControl;
            }
        }

        public void ResetPager()
        {
            SearchControl.CustomPager.ResetPager();
        }

        private static void CustomSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (CustomSearchDropDown)sender;
            if (e.NewValue != null)
                s.CustomCombo.Text = e.NewValue.ToString();
            else
                s.CustomCombo.Text = "";
        }

        public Action<CustomSearchControl> Search
        {
            set { SearchControl.CustomPager.Search = value; }
        }

        public int PagePosition
        {
            get { return SearchControl.CustomPager.PagePosition; }
            set { SearchControl.CustomPager.PagePosition = value; }
        }

        public int PageCount
        {
            get { return SearchControl.CustomPager.PageCount; }
            set { SearchControl.CustomPager.PageCount = value; }
        }

        public int PageSize
        {
            get { return SearchControl.CustomPager.PageSize; }
            set { SearchControl.CustomPager.PageSize = value; }
        }

        public int ResultCount
        {
            get { return SearchControl.CustomPager.ResultCount; }
            set { SearchControl.CustomPager.ResultCount = value; }
        }

        public CustomSearchDropDown()
        {
            InitializeComponent();
            SearchControl.ResultsGrid.IsSynchronizedWithCurrentItem = false;

            SearchControl.ResultsGrid.SelectionChanged += (s, e) =>
            {
                if (SearchControl.ResultsGrid.SelectedItem != null)
                {
                    this.CustomSelectedItem = SearchControl.ResultsGrid.SelectedItem;
                    CustomCombo.IsDropDownOpen = false;
                }
            };

            CustomCombo.DropDownOpened += (s, e) =>
            {
                SearchControl.CustomPager.Search(SearchControl);
            };
        }
    }
}