using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmartBiz.MDMAPI.Common.Entities;
using Service = SmartBiz.MDM.Presentation.ServiceReference;
using ApiAck = SmartBiz.MDM.Presentation.ServiceReference.ApiAck;
using EApiCallStatus = SmartBiz.MDM.Presentation.ServiceReference.EApiCallStatus;

namespace SmartBiz.MDM.Presentation
{
    
    public partial class Document : UserControl
    {
        DocumentModel d;
        DocumentAttributesModel da; 

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
            else if(tb_DocumentAttributes.IsSelected){
                da.Save();
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
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.Delete();
            }
            else if(tb_DocumentAttributes.IsSelected)
            {
                da.Delete();
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
            else if(tb_DocumentAttributes.IsSelected)
            {
                if (da == null)
                    da = new DocumentAttributesModel(this);
            }
        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Document.IsSelected)
            {
                d.CancelUpdate();
            }
            else if(tb_DocumentAttributes.IsSelected)
            {
                da.CancelUpdate();
            }
        }
        private void DSearchControl_SearchClick(object sender, RoutedEventArgs e)
        {
            DSearchControl.ResetPager();
            DocumentModel.Search(DSearchControl);
        }

        private void DASearchControl_SearchClick(object sender, RoutedEventArgs e)
        {
            DASearchControl.ResetPager();
            DocumentAttributesModel.Search(DASearchControl);
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

        }
    }
}