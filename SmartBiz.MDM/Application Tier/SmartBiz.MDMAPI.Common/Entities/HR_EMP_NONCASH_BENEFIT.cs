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
    public partial class HR_EMP_NONCASH_BENEFIT
    {
        string empId;
        string benefitCode;

    	[DataMember]
        public string EMP_NUMBER
        {
            get { return empId; }
            set
            {
                if (value == "")
                {
                    throw new ValidationException("Employee ID is empty! Please Fill!");
                }
                else
                {
                    empId = value;
                }
            }
        }
    	[DataMember]
        public string NBEN_CODE
        {
            get { return benefitCode; }
            set
            {
                if (value == "")
                {
                    throw new ValidationException("Benefit Code is empty! Please Fill!");
                }
                else
                {
                    benefitCode = value;
                }
            }
        }
    	[DataMember]
    	public Nullable<System.DateTime> ENBEN_ISSUE_DATE { get; set; }
    	[DataMember]
    	public Nullable<double> ENBEN_QUANTITY { get; set; }
    	[DataMember]
    	public string ENBEN_COMMENTS { get; set; }
    	[DataMember]
        [Values("No", "Yes")]
    	public Nullable<short> ENBEN_ASSES_MGMT_FLG { get; set; }
    
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    	[DataMember]
        public virtual HR_NONCASH_BENEFIT HR_NONCASH_BENEFIT { get; set; }
    public HR_EMP_NONCASH_BENEFIT Clone(){
    	return this.MemberwiseClone() as HR_EMP_NONCASH_BENEFIT;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
