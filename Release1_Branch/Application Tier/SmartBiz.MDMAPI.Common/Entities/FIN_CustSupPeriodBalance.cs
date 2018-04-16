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
    public partial class FIN_CustSupPeriodBalance
    {
    	[DataMember]
    	public int CustSupFlag { get; set; }
    	[DataMember]
    	public string CusSupCode { get; set; }
    	[DataMember]
    	public int FinancialYear { get; set; }
    	[DataMember]
    	public int AccountingPeriod { get; set; }
    	[DataMember]
    	public string CurrencyCode { get; set; }
    	[DataMember]
    	public Nullable<decimal> OpeningBalanceFCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> OpeningBalanceLCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> DebitMovementFCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> DebitMovementLCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> CreditMovementFCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> CreditMovementLCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> ClosingBalanceFCY { get; set; }
    	[DataMember]
    	public Nullable<decimal> ClosingBalanceLCY { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual FIN_Currency FIN_Currency { get; set; }
    	[DataMember]
        public virtual FIN_CustomerSupplier_Info FIN_CustomerSupplier_Info { get; set; }
    public FIN_CustSupPeriodBalance Clone(){
    	return this.MemberwiseClone() as FIN_CustSupPeriodBalance;
    }
    public override String ToString(){
       return CustSupFlag.ToString();
    }
    }
}
