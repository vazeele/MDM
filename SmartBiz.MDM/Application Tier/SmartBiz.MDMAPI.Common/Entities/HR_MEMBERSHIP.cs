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
    [DataContract(IsReference=true)]
    public partial class HR_MEMBERSHIP
    {
    	
        public HR_MEMBERSHIP()
        {
            this.HR_EMP_MEMBER_DETAIL = new HashSet<HR_EMP_MEMBER_DETAIL>();
        }
    
    	[DataMember]
    	public string MEMBSHIP_CODE { get; set; }
    	[DataMember]
    	public string MEMBTYPE_CODE { get; set; }
    	[DataMember]
    	public string MEMBSHIP_NAME { get; set; }
    	[DataMember]
    	public Nullable<short> STATUS_FLAG { get; set; }
    
    	[DataMember]
        public virtual ICollection<HR_EMP_MEMBER_DETAIL> HR_EMP_MEMBER_DETAIL { get; set; }
    	[DataMember]
        public virtual HR_MEMBERSHIP_TYPE HR_MEMBERSHIP_TYPE { get; set; }
    public HR_MEMBERSHIP Clone(){
    	return this.MemberwiseClone() as HR_MEMBERSHIP;
    }
    public override String ToString(){
       return MEMBSHIP_CODE.ToString();
    }
    }
}
