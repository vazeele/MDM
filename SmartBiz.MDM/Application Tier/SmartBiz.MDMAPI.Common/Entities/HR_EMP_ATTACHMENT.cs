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
    public partial class HR_EMP_ATTACHMENT
    {
    	[DataMember]
    	public string EMP_NUMBER { get; set; }
    	[DataMember]
    	public decimal EATTACH_ID { get; set; }
    	[DataMember]
    	public string EATTACH_DESC { get; set; }
    	[DataMember]
    	public byte[] EATTACH_ATTACHMENT { get; set; }
    
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    public HR_EMP_ATTACHMENT Clone(){
    	return this.MemberwiseClone() as HR_EMP_ATTACHMENT;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
