//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartBiz.MDMAPI.Common.Entities
{
    using System;
    using System.Collections.Generic;
    
    using System.Runtime.Serialization;
    [DataContract(IsReference = true)]
    public partial class HR_LOCATION
    {
    	
        public HR_LOCATION()
        {
            this.HR_EMPLOYEE_EIM_WS = new HashSet<HR_EMPLOYEE_EIM_WS>();
        }
    
    	[DataMember]
    	public string LOC_CODE { get; set; }
    	[DataMember]
    	public string LOC_NAME { get; set; }
    	[DataMember]
    	public Nullable<short> STATUS_FLAG { get; set; }
    
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_EIM_WS> HR_EMPLOYEE_EIM_WS { get; set; }
    }
}
