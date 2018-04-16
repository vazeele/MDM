using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class ReportingQuery
    {       
        public string SuperiorEmployeeNumber  { get; set; }
        public string SubordinateEmployeeNumber { get; set; }
        public Int16? ReportingMode { get; set; }
    }
}
