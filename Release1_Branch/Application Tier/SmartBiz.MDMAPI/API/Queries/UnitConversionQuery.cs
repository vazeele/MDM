using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI.API.Queries
{
    public class UnitConversionQuery
    {
        public string FromUnitCode { get; set; }
        public string ToUnitCode { get; set; }
        public decimal? ConversionFactor { get; set; }
    }
}
