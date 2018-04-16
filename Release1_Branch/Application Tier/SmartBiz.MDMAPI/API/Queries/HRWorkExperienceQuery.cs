using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class HRWorkExperienceQuery
    {
        public string EmployeeNumber { get; set; }
        public string Company { get; set; }

        public int? Years { get; set; }
    }
}
