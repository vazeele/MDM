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
    public partial class HR_CORPORATE_TITLE
    {
    	
        public HR_CORPORATE_TITLE()
        {
            this.HR_DESIGNATION = new HashSet<HR_DESIGNATION>();
            this.HR_EMPLOYEE_JI = new HashSet<HR_EMPLOYEE_JI>();
        }
        public override string ToString()
        {
            return CT_CODE;
        }    

    	[DataMember]
    	public string CT_CODE { get; set; }
    	[DataMember]
    	public string CT_NAME { get; set; }
    	[DataMember]
    	public Nullable<short> CT_TOPLEV_FLG { get; set; }
    	[DataMember]
    	public string SAL_GRD_CODE { get; set; }
    	[DataMember]
    	public Nullable<short> STATUS_FLAG { get; set; }
    
    	[DataMember]
        public virtual HR_SALARY_GRADE HR_SALARY_GRADE { get; set; }
    	[DataMember]
        public virtual ICollection<HR_DESIGNATION> HR_DESIGNATION { get; set; }
    	[DataMember]
        public virtual ICollection<HR_EMPLOYEE_JI> HR_EMPLOYEE_JI { get; set; }
    }
}
