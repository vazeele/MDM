using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class BasicSalaryQuery
    {
        public string EmployeeNumber { get; set; }
        public int? Year { get; set; }
        public string SalaryGradeCode { get; set; }
        public string CurrencyID { get; set; }

    }
}
