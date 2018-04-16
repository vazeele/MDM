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
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    [DataContract(IsReference=true)]
    public partial class HR_EMP_WORK_EXPERIENCE
    {
    	[DataMember]
    	public string EMP_NUMBER { get; set; }
    	[DataMember]
    	public string EEXP_COMPANY { get; set; }
    	[DataMember]
    	public string EEXP_ADDRESS1 { get; set; }
    	[DataMember]
    	public string EEXP_ADDRESS2 { get; set; }
    	[DataMember]
    	public string EEXP_ADDRESS3 { get; set; }
    	[DataMember]
    	public string EEXP_DESIG_ON_LEAVE { get; set; }
    	[DataMember]
    	public Nullable<short> EEXP_WORK_RELATED_FLG { get; set; }
        private Nullable<System.DateTime> _EEXP_FROM_DATE;
    	[DataMember]
    	public Nullable<System.DateTime> EEXP_FROM_DATE {
            get
            {
                return _EEXP_FROM_DATE;
            }
            set
            {
                if (EEXP_TO_DATE != null && EEXP_TO_DATE < value)
                    throw new ValidationException("From date cannot be greater than To date");

                _EEXP_FROM_DATE = value;


            }
        }
        private Nullable<System.DateTime> _EEXP_TO_DATE;
        [DataMember]
        public Nullable<System.DateTime> EEXP_TO_DATE
        {
            get
            {
                return _EEXP_TO_DATE;
            }
            set
            {
                if (_EEXP_FROM_DATE != null && _EEXP_FROM_DATE > value)
                    throw new ValidationException("To date cannot be less than From date");

                _EEXP_TO_DATE = value;


            }
        }
    	[DataMember]
    	public Nullable<decimal> EEXP_YEARS { get; set; }
    	[DataMember]
    	public Nullable<short> EEXP_MONTHS { get; set; }
    	[DataMember]
    	public string EEXP_REASON_FOR_LEAVE { get; set; }
    	[DataMember]
    	public string EEXP_CONTACT_PERSON { get; set; }
        private string _EEXP_TELEPHONE;
    	[DataMember]
    	public string EEXP_TELEPHONE { 
            get
            {
                return _EEXP_TELEPHONE;
            }
            set
            {
                long result=0;
                if (!long.TryParse(value, out result))
                {
                    throw new ValidationException("Telephone No. format is incorrect");

                }else if(value.Length<10)
                {
                     
                        throw new ValidationException("No of digits too low for a Telephone No.");
                }

             
                  _EEXP_TELEPHONE = value;
            }
        }
        private string _EEXP_EMAIL;
    	[DataMember]
    	public string EEXP_EMAIL {
            get
            {
                return _EEXP_EMAIL;
            }

            set
            {
                if (value==null || !Regex.IsMatch(value, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    throw new ValidationException("Email format is incorrect");

                else
                    _EEXP_EMAIL = value;
            }


        }
    	[DataMember]
    	public string EEXP_ACCOUNTABILITIES { get; set; }
    	[DataMember]
    	public string EEXP_ACHIEVEMENTS { get; set; }
         private HR_EMPLOYEE _HR_EMPLOYEE;
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE {      
    
            get
            {
                return _HR_EMPLOYEE;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("Employee is required");

                _HR_EMPLOYEE = value;

            }
        }
    public HR_EMP_WORK_EXPERIENCE Clone(){
    	return this.MemberwiseClone() as HR_EMP_WORK_EXPERIENCE;
    }
    public override String ToString(){
       return EMP_NUMBER.ToString();
    }
    }
}
