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
    using System.ComponentModel.DataAnnotations;
       
    [DataContract(IsReference=true)]
    public partial class HR_JD_CATEGORY
    {
    	
        public HR_JD_CATEGORY()
        {
            this.HR_EMPLOYEE_JS = new HashSet<HR_EMPLOYEE_JS>();
            this.HR_EMPLOYEE_JS1 = new HashSet<HR_EMPLOYEE_JS>();
            this.HR_EMP_JOBSPEC = new HashSet<HR_EMP_JOBSPEC>();
        }
    
    	[DataMember]
    	public string JDCAT_CODE { get; set; }
    	[DataMember]
    	public string JDCAT_NAME { get; set; }
    	[DataMember]
    	public Nullable<short> STATUS_FLAG { get; set; }
    
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_JS> HR_EMPLOYEE_JS { get; set; }
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_JS> HR_EMPLOYEE_JS1 { get; set; }
    	[DataMember]
        public virtual ICollection<HR_EMP_JOBSPEC> HR_EMP_JOBSPEC { get; set; }
    public HR_JD_CATEGORY Clone(){
    	return this.MemberwiseClone() as HR_JD_CATEGORY;
    }
    public override String ToString(){
       return JDCAT_CODE.ToString();
    }
    }
}