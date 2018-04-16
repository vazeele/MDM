using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class TransactionReferenceQuery
    {
        public string DocCode { get; set; }
        public short? RefSeq { get; set; }
        public string ReferenceText { get; set; }
    }
}
