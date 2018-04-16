using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Training : UserControl
    {
        private TrainingModel TrainingModel;

        public Training()
        {
            InitializeComponent();
            TrainingModel = new TrainingModel(this);
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            TrainingModel.Save();
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            TrainingModel.Add();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            TrainingModel.Delete();
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            TrainingModel.CancelUpdate();
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            TrainingSearchControl.ResetSearchControl();
            TrainingModel = new TrainingModel(this);
        }

    }
}