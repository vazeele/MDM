using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for CustomPager.xaml
    /// </summary>
    public partial class CustomPager : UserControl
    {
        public Action<CustomSearchControl> Search { get; set; }
        public CustomSearchControl SearchControl { get; set; }
        public CustomPager()
        {
           InitializeComponent();
           FastForward.Click += (s, e) =>
            {
               PagePosition = PageCount;
               Search(SearchControl);
               e.Handled = true;
            };
           FastBackward.Click += (s, e) =>
            {
                PagePosition = 1;
                Search(SearchControl);
                e.Handled = true;
            };
          Forward.Click += (s, e) =>
            {
                if (PagePosition <PageCount)
                   PagePosition++;
                Search(SearchControl);
                e.Handled = true;
            };
          Backward.Click += (s, e) =>
            {
                if (PagePosition > 1)
                    PagePosition--;
                Search(SearchControl);
                e.Handled = true;
            };

        }
        private int _PageCount;
        private int _ResultCount;
        public Button btnBackward
        {
            get { return this.Backward; }
        }
        public Button btnForward
        {
            get { return this.Forward; }
        }
        public Button btnFastForward
        {
            get { return this.FastForward; }
        }
        public Button btnFastBackward
        {
            get { return this.FastBackward; }
        }
        public Label lblPageLabel
        {
            get { return this.PageLabel; }
        }
        public int PagePosition
        {
            get;
            set;
        }
        public int PageCount
        {
            get { return _PageCount; }
            set { _PageCount = value; PageLabel.Content = PagePosition + " of " + _PageCount; }
        }
        public int PageSize
        {   
                
            get;
            set;
        }
        public int ResultCount
        {
            get{return _ResultCount;}
            set { 
                _ResultCount=value; 
                PageCount=(int) Math.Ceiling(_ResultCount / Convert.ToDouble(PageSize));
            }
        }
        public void ResetPager()
        {
            PagePosition = 1;
            PageCount = 0;
            ResultCount = 0;
        }
        
    }
}
