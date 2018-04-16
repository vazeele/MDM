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
    public partial class ERP_AgentBrokerSalesMan
    {
    	[DataMember]
    	public int AgentBrokerSalesManFlag { get; set; }
    	[DataMember]
    	public string AgentBrokerSalesManCode { get; set; }
    	[DataMember]
    	public string AgentBrokerSalesManName { get; set; }
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
    	public Nullable<int> ActiveStatus { get; set; }
    	[DataMember]
    	public string GLAccount { get; set; }
    	[DataMember]
    	public int TaxFlag { get; set; }
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
    	public Nullable<int> PaymentMode { get; set; }
    	[DataMember]
    	public string CurrencyCode { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency { get; set; }
    public ERP_AgentBrokerSalesMan Clone(){
    	return this.MemberwiseClone() as ERP_AgentBrokerSalesMan;
    }
    public override String ToString(){
       return AgentBrokerSalesManFlag.ToString();
    }
    }
}
