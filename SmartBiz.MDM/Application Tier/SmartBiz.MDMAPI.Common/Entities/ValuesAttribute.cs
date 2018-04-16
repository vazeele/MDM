using System;
using System.Collections.Generic;

namespace SmartBiz.MDMAPI.Common.Entities
{
    public class ValuesAttribute : Attribute
    {
        public List<string> values { get; set; }

        public ValuesAttribute(params String[] values)
        {
            this.values = new List<string>();
            foreach (var v in values)
            {
                this.values.Add(v);
            }
        }
    }
}