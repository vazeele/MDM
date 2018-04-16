using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class CostCenterwiseConfigurationQuery
    {
        public string CostCenterCode { get; set; }
        public int? RevNo { get; set; }
        public string BaseCurrencyCode { get; set; }
    }
}
