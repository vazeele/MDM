using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class DocumentAttributesBankInfoQuery
    {
        public string DocCode { get; set; }
        public string TxCode { get; set; }
        public string FixedBankCode { get; set; }
    }
}
