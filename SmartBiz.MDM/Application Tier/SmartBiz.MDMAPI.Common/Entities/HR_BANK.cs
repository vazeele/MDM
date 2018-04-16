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
    public partial class HR_BANK
    {
    	
        public HR_BANK()
        {
            this.HR_BRANCH = new HashSet<HR_BRANCH>();
            this.HR_EMP_BANK = new HashSet<HR_EMP_BANK>();
        }
    
    	[DataMember]
    	public string BANK_CODE { get; set; }
    	[DataMember]
    	public string BANK_NAME { get; set; }
    	[DataMember]
    	public string BANK_ADDRESS { get; set; }
    	[DataMember]
    	public Nullable<short> STATUS_FLAG { get; set; }
    
    	[DataMember]
        public virtual ICollection<HR_BRANCH> HR_BRANCH { get; set; }
    	[DataMember]
        public virtual ICollection<HR_EMP_BANK> HR_EMP_BANK { get; set; }
    public HR_BANK Clone(){
    	return this.MemberwiseClone() as HR_BANK;
    }
    public override String ToString(){
       return BANK_CODE.ToString();
    }
    }
}