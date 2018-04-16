using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for HR_EMP_WORK_EXPERIENCE.xaml
    /// </summary>
    public partial class HREmployeeWorkExperience : UserControl
    {
        private HRWorkExperienceModel hew;

        public HREmployeeWorkExperience()
        {
            InitializeComponent();
            hew = new HRWorkExperienceModel(this);
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            EMPWORKSearchControl.ResetSearchControl();
            hew = new HRWorkExperienceModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            hew.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            hew.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            hew.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            hew.CancelUpdate();
        }
    }
}