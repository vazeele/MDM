using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    public partial class Document : UserControl
    {
        private DocumentModel d;
        private DocumentAttributesModel da;
        private DocumentAttributesBankInfoModel dabi;
        private DocumentAttributesRefModel dar;

        public Document()
        {
            InitializeComponent();
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.Save();
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                da.Save();
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                dabi.Save();
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                dar.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.Add();
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                da.Add();
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                dabi.Add();
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                dar.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.Delete();
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                da.Delete();
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                dabi.Delete();
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                dar.Delete();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Document.IsSelected)
            {
                if (d == null)
                    d = new DocumentModel(this);
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                if (da == null)
                    da = new DocumentAttributesModel(this);
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                if (dabi == null)
                    dabi = new DocumentAttributesBankInfoModel(this);
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                if (dar == null)
                    dar = new DocumentAttributesRefModel(this);
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.CancelUpdate();
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                da.CancelUpdate();
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                dabi.CancelUpdate();
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                dar.CancelUpdate();
            }
        }
        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                DSearchControl.ResetSearchControl();
                d = new DocumentModel(this);
            }
            else if (tb_DocumentAttributes.IsSelected)
            {
                DASearchControl.ResetSearchControl();
                da = new DocumentAttributesModel(this);
            }
            else if (tb_DocumentAttributesBankInfo.IsSelected)
            {
                DABISearchControl.ResetSearchControl();
                dabi = new DocumentAttributesBankInfoModel(this);
            }
            else if (tb_DocumentAttributesRef.IsSelected)
            {
                DARSearchControl.ResetSearchControl();
                dar = new DocumentAttributesRefModel(this);
            }
        }
    }
}