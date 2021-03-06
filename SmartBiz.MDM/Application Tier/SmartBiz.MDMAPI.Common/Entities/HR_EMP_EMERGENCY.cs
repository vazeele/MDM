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
    using System.Text.RegularExpressions;
    using System.ComponentModel.DataAnnotations;
    
    using System.Runtime.Serialization;
    [DataContract(IsReference=true)]
    public partial class HR_EMP_EMERGENCY
    {
        private string backing_EEMERG_CONT_PER_FULLNAME;
        private string backing_EEMERG_RES_TELEPHONE;
        private string backing_EEMERG_OFFICE_TELEPHONE;
        private string backing_EEMERG_MOBILE;

    	[DataMember]
    	public string EMP_NUMBER { get; set; }        
        [DataMember]
        public string EEMERG_CONT_PER_FULLNAME
        {
            get
            {
                return backing_EEMERG_CONT_PER_FULLNAME;
            }
            set
            {
                if (Regex.IsMatch(value, @"^[a-zA-Z ]+$"))
                {
                    backing_EEMERG_CONT_PER_FULLNAME = value;
                }
                else
                {
                    throw new ValidationException("Name can not have numbers");
                }
            }
        }
    	[DataMember]
    	public string EEMERG_RELATIONSHIP { get; set; }
    	[DataMember]
    	public string EEMERG_PER_ADDRESS { get; set; }
    	[DataMember]
    	public string EEMERG_OFFICIAL_ADDRESS { get; set; }
    	[DataMember]
    	public string EEMERG_RES_TELEPHONE {
            get
            {
                return backing_EEMERG_RES_TELEPHONE;
            }
            set
            {
                if (Regex.IsMatch(value, @"^[0-9]{10}$"))
                {
                    backing_EEMERG_RES_TELEPHONE = value;
                }
                else
                {
                    throw new ValidationException("Telephone NO must have exactly 10 numbers");
                }
            }
        }
    	[DataMember]
    	public string EEMERG_OFFICE_TELEPHONE {
            get
            {
                return backing_EEMERG_OFFICE_TELEPHONE;
            }
            set
            {
                if (Regex.IsMatch(value, @"^[0-9]{10}$"))
                {
                    backing_EEMERG_OFFICE_TELEPHONE = value;
                }
                else
                {
                    throw new ValidationException("Telephone NO must have exactly 10 numbers");
                }
            }
        }
    	[DataMember]
    	public string EEMERG_MOBILE {
            get
            {
                return backing_EEMERG_MOBILE;
            }
            set
            {
                if (Regex.IsMatch(value, @"^[0-9]{10}$"))
                {
                    backing_EEMERG_MOBILE = value;
                }
                else
                {
                    throw new ValidationException("Telephone NO must have exactly 10 numbers");
                }
            }
        }
    
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    public HR_EMP_EMERGENCY Clone(){
    	return this.MemberwiseClone() as HR_EMP_EMERGENCY;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
