using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class HRLanguages : UserControl
    {
        private HRLanguageModel hrLanguage;
        private HREmpLanguageModel hrEmpLanguage;
        

        public HRLanguages()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_HRLanguage.IsSelected)
            {
                hrLanguage.Save();
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                hrEmpLanguage.Save();
            }
            
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_HRLanguage.IsSelected)
            {
                hrLanguage.Add();
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                hrEmpLanguage.Add();
            }
            
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_HRLanguage.IsSelected)
            {
                hrLanguage.Delete();
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                hrEmpLanguage.Delete();
            }
            
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_HRLanguage.IsSelected)
            {
                if (hrLanguage == null)
                    hrLanguage = new HRLanguageModel(this);
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                if (hrEmpLanguage == null)
                    hrEmpLanguage = new HREmpLanguageModel(this);
            }
            
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_HRLanguage.IsSelected)
            {
                hrLanguage.CancelUpdate();
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                hrEmpLanguage.CancelUpdate();
            }
            
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_HRLanguage.IsSelected)
            {
                HRLanguageSearchControl.ResetSearchControl();
                hrLanguage = new HRLanguageModel(this);
            }
            else if (tb_HREmpLanguage.IsSelected)
            {
                HREmpLanguageSearchControl.ResetSearchControl();
                hrEmpLanguage = new HREmpLanguageModel(this);
            }
        }
    }
}