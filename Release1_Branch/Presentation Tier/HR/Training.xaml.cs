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
    /// <summary>
    ///     Interaction logic for SystemInstallation.xaml
    /// </summary>
    public partial class Training : UserControl
    {

        TrainingModel TrainingModel;
        public Training()
        {
            InitializeComponent();
          
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Training.IsSelected)
            {
                TrainingModel.Save();
            }

        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Training.IsSelected)
            {
                TrainingModel.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Training.IsSelected)
            {
                TrainingModel.Delete();
            }
        }

        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Training.IsSelected)
            {
                TrainingModel.CancelUpdate();
            }
        }

        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Training.IsSelected)
            {
                if (TrainingModel == null)
                    TrainingModel = new TrainingModel(this);
            }
        }
              
    }
}