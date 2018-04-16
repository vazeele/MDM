using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class GeneralLedgerAccountQuery
    {
        public string AccountNo { get; set; }
        public int? AccountType { get; set; }
        public int? AccountSubtype { get; set; }
    }
}
