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
    [DataContract(IsReference = true)]
    public partial class HR_EMPLOYEE_JI
    {
        public HR_EMPLOYEE_JI Clone()
        {
            return this.MemberwiseClone() as HR_EMPLOYEE_JI;
        }
        private Nullable<float> _EMP_WORKHOURS;
        Nullable<System.DateTime> _EMP_CONFIRM_DATE;
        Nullable<System.DateTime> _EMP_RESIGN_DATE;
        Nullable<System.DateTime> _EMP_RETIRE_DATE;
        Nullable<short> _EMP_CONFIRM_FLG;
    	[DataMember]
    	public string EMP_NUMBER { get; set; }
    	[DataMember]
    	public Nullable<System.DateTime> EMP_DATE_JOINED { get; set; }
    	[DataMember]
        public Nullable<short> EMP_CONFIRM_FLG { get;set;}
    	[DataMember]
    	public Nullable<System.DateTime> EMP_CONFIRM_DATE 
        {
            get { return _EMP_CONFIRM_DATE; }
            set
            {
                if (EMP_CONFIRM_FLG!=null && EMP_CONFIRM_FLG.Value != 1)
                {
                    throw new ValidationException("Invalid Flag!");
                }
                else if (EMP_DATE_JOINED!=null && value.Value.Year <= EMP_DATE_JOINED.Value.Year)
                {
                    throw new ValidationException("Invalid Date!");
                }
                _EMP_CONFIRM_DATE = value;
            }
        }
    	[DataMember]
    	public Nullable<System.DateTime> EMP_RESIGN_DATE
        {
            get { return _EMP_RESIGN_DATE; }
            set
            {
                if(_EMP_RETIRE_DATE!=null && value!=null && value.Value>=_EMP_RETIRE_DATE.Value)
                {
                    throw new ValidationException("Invalid Date!");
                }
                _EMP_RESIGN_DATE = value;
            } 
        }
    	[DataMember]
    	public Nullable<System.DateTime> EMP_RETIRE_DATE
        {
            get { return _EMP_RETIRE_DATE; }
            set
            {
                _EMP_RETIRE_DATE = value;
            }
        }

    	[DataMember]
    	public string SAL_GRD_CODE { get; set; }
    	[DataMember]
    	public string CT_CODE { get; set; }
    	[DataMember]
    	public string DSG_CODE { get; set; }
    	[DataMember]
    	public Nullable<float> EMP_WORKHOURS 
        {
            get { return _EMP_WORKHOURS; }
            set
            {
                    if(value<=0.0 || value>=24)
                    {
                        throw new ValidationException("Invalid Work Duration!");
                    }
                _EMP_WORKHOURS = value;
            }
        }
    
    	[DataMember]
        public virtual HR_CORPORATE_TITLE HR_CORPORATE_TITLE { get; set; }
    	[DataMember]
        public virtual HR_DESIGNATION HR_DESIGNATION { get; set; }
    	[DataMember]
        public virtual HR_EMPLOYEE HR_EMPLOYEE { get; set; }
    	[DataMember]
        public virtual HR_SALARY_GRADE HR_SALARY_GRADE { get; set; }
    }
}
