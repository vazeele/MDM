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
    public partial class HR_EMP_JOBSPEC
    {
    	[DataMember]
    	public string EMP_NUMBER { get; set; }
    	[DataMember]
    	public string JDCAT_CODE { get; set; }
    	[DataMember]
    	public string EJOBSPEC_ATTRIBUTES { get; set; }
    
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    	[DataMember]
        public virtual HR_JD_CATEGORY HR_JD_CATEGORY { get; set; }
    public HR_EMP_JOBSPEC Clone(){
    	return this.MemberwiseClone() as HR_EMP_JOBSPEC;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
