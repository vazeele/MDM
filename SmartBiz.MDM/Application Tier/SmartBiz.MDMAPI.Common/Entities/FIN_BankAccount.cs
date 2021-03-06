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
    public partial class FIN_BankAccount
    {
    	
        public FIN_BankAccount()
        {
            this.ERP_DocumentAttributesBankInfo = new HashSet<ERP_DocumentAttributesBankInfo>();
        }
    
    	[DataMember]
    	public string BankCode { get; set; }
    	[DataMember]
    	public string BankBranchCode { get; set; }
    	[DataMember]
    	public int AccountSEQNo { get; set; }
    	[DataMember]
    	public string GLAccountNo { get; set; }
    	[DataMember]
    	public string CurrentACNo { get; set; }
    	[DataMember]
    	public Nullable<decimal> ODLimit { get; set; }
    	[DataMember]
    	public Nullable<System.DateTime> LastChequePrintedDate { get; set; }
    	[DataMember]
    	public string LastChequeNumber { get; set; }
    	[DataMember]
    	public string ChequeDOCCode { get; set; }
    	[DataMember]
    	public string CostCenter { get; set; }
    	[DataMember]
    	public string CurrencyCode { get; set; }
    	[DataMember]
    	public Nullable<decimal> Balance { get; set; }
    	[DataMember]
    	public string ChequePrintFlag { get; set; }
    	[DataMember]
    	public Nullable<int> NoofDepositperSlip { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual ERP_Document ERP_Document { get; set; }
    	[DataMember]
        public virtual ICollection<ERP_DocumentAttributesBankInfo> ERP_DocumentAttributesBankInfo { get; set; }
    	[DataMember]
        public virtual FIN_BankBranch FIN_BankBranch { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount { get; set; }
    public FIN_BankAccount Clone(){
    	return this.MemberwiseClone() as FIN_BankAccount;
    }
    public override String ToString(){
       return BankCode.ToString();
    }
    }
}
