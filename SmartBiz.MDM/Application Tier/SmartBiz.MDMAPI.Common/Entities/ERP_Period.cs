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
    public partial class ERP_Period
    {
        private int backing_FinancialYear;
    	[DataMember]
    	public string ProductCode { get; set; }
    	[DataMember]
    	public int FinancialYear 
        {
            get
            {
                return backing_FinancialYear;
            }
            set
            {
                string check_value = value.ToString();
                if (Regex.IsMatch(check_value, @"^[0-9]{4}$"))
                {
                    backing_FinancialYear = (int)value;
                }
                else
                {
                    throw new ValidationException("Financial year must be an number of four digits");
                }
            }
        }
    	[DataMember]
    	public int AccountingPeriod { get; set; }
    	[DataMember]
    	public int ProcessPeriod { get; set; }
    	[DataMember]       
        public Nullable<System.DateTime> StardDate { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> EndDate { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    public ERP_Period Clone(){
    	return this.MemberwiseClone() as ERP_Period;
    }
    public override String ToString(){
       return ProductCode.ToString();
    }
    }
}
