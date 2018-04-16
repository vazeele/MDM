using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class CurrencyExchangeRateQuery
    {
        public string CurrencyCode { get; set; }
        public short? CalenderYear { get; set; }
        public byte? CalenderMonth { get; set; }
    }

}
