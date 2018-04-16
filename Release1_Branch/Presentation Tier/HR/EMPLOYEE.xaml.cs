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
    /// Interaction logic for EMPLOYEE.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        EmployeeModel employee;
        EmployeePDModel pd;
        EmployeeJIModel ji;
        EmployeeJSModel js;
        public Employee()
        {
            InitializeComponent();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                employee.Save();
            }
            else if(tb_EmployeePD.IsSelected)
            {
                pd.Save();
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                ji.Save();
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                js.Save();
            }

        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                employee.Add();
            }
            else if (tb_EmployeePD.IsSelected)
            {
                pd.Add();
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                ji.Add();
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                js.Add();
            }

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                employee.Delete();
            }
            else if (tb_EmployeePD.IsSelected)
            {
                pd.Delete();
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                ji.Delete();
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                js.Delete();
            }

        }
        private void BtnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                employee.CancelUpdate();
            }
            else if (tb_EmployeePD.IsSelected)
            {
                pd.CancelUpdate();
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                ji.CancelUpdate();
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                js.CancelUpdate();
            }

        }
        private void BtRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                ESearchControl.ResetSearchControl();
                employee = new EmployeeModel(this);
            }
            else if (tb_EmployeePD.IsSelected)
            {
                PDSearchControl.ResetSearchControl();
                pd = new EmployeePDModel(this);
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                JISearchControl.ResetSearchControl();
                ji = new EmployeeJIModel(this);
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                JSSearchControl.ResetSearchControl();
                js = new EmployeeJSModel(this);
            }

        }
        private void TbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
            {
                return;
            }

            if (tb_Employee.IsSelected)
            {
                if (employee == null)
                    employee = new EmployeeModel(this);
            }
            else if (tb_EmployeePD.IsSelected)
            {
                if (pd == null)
                    pd = new EmployeePDModel(this);
            }
            else if (tb_EmployeeJI.IsSelected)
            {
                if (ji == null)
                    ji = new EmployeeJIModel(this);
            }
            else if (tb_EmployeeJS.IsSelected)
            {
                if (js == null)
                    js = new EmployeeJSModel(this);
            }

        }
    }
}
