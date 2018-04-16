using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class ControlledTransactionQuery
    {
        public string DocCode { get; set; }
        public string TxnCode { get; set; }
        public string GLAccountCode { get; set; }
    }
}
