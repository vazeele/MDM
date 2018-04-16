using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class HRQualificationQuery
    {
        public string EmployeeNumber { get; set; }
        public string QualificationCode { get; set; }

        public int? QualificationYear { get; set; }
    }
}
