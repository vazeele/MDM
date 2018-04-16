using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Production : UserControl
    {
        private ProcessControlModel ProcessControlModel;
        private PeriodModel PeriodModel;

        public Production()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Save();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Add();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.Delete();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlModel.CancelUpdate();
            }
            if (tb_Period.IsSelected)
            {
                PeriodModel.CancelUpdate();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_ProcessControl.IsSelected)
            {
                if (ProcessControlModel == null)
                    ProcessControlModel = new ProcessControlModel(this);
            }
            if (tb_Period.IsSelected)
            {
                if (PeriodModel == null)
                    PeriodModel = new PeriodModel(this);
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ProcessControl.IsSelected)
            {
                ProcessControlSearchControl.ResetSearchControl();
                ProcessControlModel = new ProcessControlModel(this);
            }
            else if (tb_Period.IsSelected)
            {
                PeriodSearchControl.ResetSearchControl();
                PeriodModel = new PeriodModel(this);
            }
        }
    }
}