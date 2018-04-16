using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class SecondaryTransactionQuery
    {       public string DocumentCode { get; set; }
            public string TransactionCode { get; set; }
            public int? FinancialYear { get; set; }
        
    }
}
