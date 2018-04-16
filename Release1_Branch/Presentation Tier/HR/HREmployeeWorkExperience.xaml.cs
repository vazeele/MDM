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

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for HR_EMP_WORK_EXPERIENCE.xaml
    /// </summary>
    public partial class HREmployeeWorkExperience : UserControl
    {
        HRWorkExperienceModel hew;
        public HREmployeeWorkExperience()
        {
            InitializeComponent();
            hew = new HRWorkExperienceModel(this);
        }
     
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            hew.Save();

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            hew.Add();

        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            hew.CancelUpdate();
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            hew.Delete();

        }
        private void BtRefresh_Click(object sender, RoutedEventArgs e){
            EMPWORKSearchControl.ResetSearchControl();
            hew = new HRWorkExperienceModel(this);
        }
       
      
    }
}
