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
    public partial class HR_ELECTORATE
    {
    	
        public HR_ELECTORATE()
        {
            this.HR_EMPLOYEE_PC = new HashSet<HR_EMPLOYEE_PC>();
            this.HR_EMPLOYEE_CDWD = new HashSet<HR_EMPLOYEE_CDWD>();
        }
    
    	[DataMember]
    	public string ELECTORATE_CODE { get; set; }
    	[DataMember]
    	public string ELECTORATE_NAME { get; set; }
    
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_PC> HR_EMPLOYEE_PC { get; set; }
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_CDWD> HR_EMPLOYEE_CDWD { get; set; }
    }
}
