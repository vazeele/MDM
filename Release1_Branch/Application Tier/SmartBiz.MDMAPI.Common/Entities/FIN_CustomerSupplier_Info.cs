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
    public partial class FIN_CustomerSupplier_Info
    {
    	
        public FIN_CustomerSupplier_Info()
        {
            this.FIN_CustomerSupplierBank = new HashSet<FIN_CustomerSupplierBank>();
            this.FIN_CustSupPeriodBalance = new HashSet<FIN_CustSupPeriodBalance>();
        }
    
    	[DataMember]
    	public int CustSupFlag { get; set; }
    	[DataMember]
    	public string CusSupCode { get; set; }
    	[DataMember]
    	public string CustSupName { get; set; }
    	[DataMember]
    	public string AddressLine1 { get; set; }
    	[DataMember]
    	public string AddressLine2 { get; set; }
    	[DataMember]
    	public string City { get; set; }
    	[DataMember]
    	public string Country { get; set; }
    	[DataMember]
    	public Nullable<int> ForeignLocalFlag { get; set; }
    	[DataMember]
    	public Nullable<int> CustSupStatus { get; set; }
    	[DataMember]
    	public string ContactPersonsSales { get; set; }
    	[DataMember]
    	public string ContactPersonsAccount { get; set; }
    	[DataMember]
    	public Nullable<int> TaxFlag { get; set; }
    	[DataMember]
    	public Nullable<int> CustSupType { get; set; }
    	[DataMember]
    	public string TelephoneNo1 { get; set; }
    	[DataMember]
    	public string TelephoneNo2 { get; set; }
    	[DataMember]
    	public string FaxNo1 { get; set; }
    	[DataMember]
    	public string FaxNo2 { get; set; }
    	[DataMember]
    	public string EmailAddress { get; set; }
    	[DataMember]
    	public string WebSite { get; set; }
    	[DataMember]
    	public string TaxRegNo { get; set; }
    	[DataMember]
    	public Nullable<int> PaymentMode { get; set; }
    	[DataMember]
    	public string CurrencyCode { get; set; }
    	[DataMember]
    	public Nullable<int> CreditPeriod { get; set; }
    	[DataMember]
    	public Nullable<int> CreditPeriodUnit { get; set; }
    	[DataMember]
    	public Nullable<decimal> CreditLimit { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual FIN_Currency FIN_Currency { get; set; }
    	[DataMember]
        public virtual ICollection<FIN_CustomerSupplierBank> FIN_CustomerSupplierBank { get; set; }
    	[DataMember]
        public virtual ICollection<FIN_CustSupPeriodBalance> FIN_CustSupPeriodBalance { get; set; }
    public FIN_CustomerSupplier_Info Clone(){
    	return this.MemberwiseClone() as FIN_CustomerSupplier_Info;
    }
    public override String ToString(){
       return CustSupFlag.ToString();
    }
    }
}