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
    public partial class HR_EMP_LANGUAGE
    {
    	[DataMember]
    	public string EMP_NUMBER { get; set; }
    	[DataMember]
    	public string LANG_CODE { get; set; }
    	[DataMember]
        [Values("Writing","Speaking","Reading")]
    	public short ELANG_TYPE { get; set; }
    	[DataMember]
    	public string RATING_GRADE_CODE { get; set; }
    
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    	[DataMember]
        public virtual HR_LANGUAGE HR_LANGUAGE { get; set; }
    public HR_EMP_LANGUAGE Clone(){
    	return this.MemberwiseClone() as HR_EMP_LANGUAGE;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
