using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class BankAccountQuery
    {
        public string BankCode { get; set; }
        public string BankBranchCode { get; set; }
        public int? AccountSEQNo { get; set; }

        
    }
}
