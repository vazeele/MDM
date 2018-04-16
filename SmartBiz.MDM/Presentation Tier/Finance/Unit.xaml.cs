using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Unit : UserControl
    {
        private UnitDefinitionModel ud;
        private UnitConversionModel uc;

        public Unit()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Save();
            }
            else if (tb_UnitConversion.IsSelected)
            {
                uc.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Add();
            }
            else if (tb_UnitConversion.IsSelected)
            {
                uc.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.Delete();
            }
            else if (tb_UnitConversion.IsSelected)
            {
                uc.Delete();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_UnitDefinition.IsSelected)
            {
                if (ud == null)
                    ud = new UnitDefinitionModel(this);
            }
            else if (tb_UnitConversion.IsSelected)
            {
                if (uc == null)
                    uc = new UnitConversionModel(this);

            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                ud.CancelUpdate();
            }
            else if (tb_UnitConversion.IsSelected)
            {
                uc.CancelUpdate();
            }

        }
        
        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_UnitDefinition.IsSelected)
            {
                UDSearchControl.ResetSearchControl();
                ud = new UnitDefinitionModel(this);
            }
            else if (tb_UnitConversion.IsSelected)
            {
                UCSearchControl.ResetSearchControl();
                uc = new UnitConversionModel(this);
            }
        }
    }
}