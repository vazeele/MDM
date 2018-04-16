using SmartBiz.MDMAPI.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBiz.MDMAPI
{
    public class ResultDTO<T>
    {
        public T[] Result { get; set; }
        public int TotalResultCount { get; set; }
        public int LocalResultCount { get; set; }
    }
}
