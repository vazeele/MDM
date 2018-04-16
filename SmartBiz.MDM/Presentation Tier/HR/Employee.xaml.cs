using System.Windows;
using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation
{
    /// <summary>
    /// Interaction logic for EMPLOYEE.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        private EmployeeModel employee;
        private EmployeePDModel pd;
        private EmployeeJIModel ji;
        private EmployeeJSModel js;
        private EmployeeTDModel td;
        private EmployeePCModel pc;
        private EmployeeCDWDModel cdwd;
        private EmployeeOCModel oc;
        private EmployeeEIMWSModel eimws;
        public Employee()
        {
            InitializeComponent();
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
            else if (tb_EmployeeTD.IsSelected)
            {
                td.CancelUpdate();
            }
            else if (tb_EmployeePC.IsSelected)
            {
                pc.CancelUpdate();
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                cdwd.CancelUpdate();
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                oc.CancelUpdate();
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                eimws.CancelUpdate();
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
            else if (tb_EmployeeTD.IsSelected)
            {
                TDSearchControl.ResetSearchControl();
                td = new EmployeeTDModel(this);
            }
            else if (tb_EmployeePC.IsSelected)
            {
                PCSearchControl.ResetSearchControl();
                pc = new EmployeePCModel(this);
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                CDWDSearchControl.ResetSearchControl();
                cdwd = new EmployeeCDWDModel(this);
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                OCSearchControl.ResetSearchControl();
                oc = new EmployeeOCModel(this);
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                EIMWSSearchControl.ResetSearchControl();
                eimws = new EmployeeEIMWSModel(this);
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
            else if (tb_EmployeeTD.IsSelected)
            {
                if (td == null)
                    td = new EmployeeTDModel(this);
            }
            else if (tb_EmployeePC.IsSelected)
            {
                if (pc == null)
                    pc = new EmployeePCModel(this);
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                if (cdwd == null)
                    cdwd = new EmployeeCDWDModel(this);
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                if (oc == null)
                    oc = new EmployeeOCModel(this);
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                if (eimws == null)
                    eimws = new EmployeeEIMWSModel(this);
            }
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Employee.IsSelected)
            {
                employee.Save();
            }
            else if (tb_EmployeePD.IsSelected)
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
            else if (tb_EmployeeTD.IsSelected)
            {
                td.Save();
            }
            else if (tb_EmployeePC.IsSelected)
            {
                pc.Save();
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                cdwd.Save();
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                oc.Save();
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                eimws.Save();
            }
        }

        private void BtAdd_Click(object sender, RoutedEventArgs e)
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
            else if (tb_EmployeeTD.IsSelected)
            {
                td.Add();
            }
            else if (tb_EmployeePC.IsSelected)
            {
                pc.Add();
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                cdwd.Add();
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                oc.Add();
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                eimws.Add();
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
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
            else if (tb_EmployeeTD.IsSelected)
            {
                td.Delete();
            }
            else if (tb_EmployeePC.IsSelected)
            {
                pc.Delete();
            }
            else if (tb_EmployeeCDWD.IsSelected)
            {
                cdwd.Delete();
            }
            else if (tb_EmployeeOC.IsSelected)
            {
                oc.Delete();
            }
            else if (tb_EmployeeEIMWS.IsSelected)
            {
                eimws.Delete();
            }
        }
    }
}