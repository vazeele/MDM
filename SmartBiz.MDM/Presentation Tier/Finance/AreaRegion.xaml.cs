using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class AreaRegion : UserControl
    {
        private RegionModel r;
        private AreaModel a;

        public AreaRegion()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Region.IsSelected)
            {
                r.Save();
            }
            else if (tb_Area.IsSelected)
            {
                a.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Region.IsSelected)
            {
                r.Add();
            }
            else if (tb_Area.IsSelected)
            {
                a.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Region.IsSelected)
            {
                r.Delete();
            }
            else if (tb_Area.IsSelected)
            {
                a.Delete();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Region.IsSelected)
            {
                if (r == null)
                    r = new RegionModel(this);
            }
            else if (tb_Area.IsSelected)
            {
                if (a == null)
                    a = new AreaModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Region.IsSelected)
            {
                r.CancelUpdate();
            }
            else if (tb_Area.IsSelected)
            {
                a.CancelUpdate();
            }
        }

        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Region.IsSelected)
            {
                RSearchControl.ResetSearchControl();
                r = new RegionModel(this);
            }
            else if (tb_Area.IsSelected)
            {
                ASearchControl.ResetSearchControl();
                a = new AreaModel(this);
            }
        }
    }
}