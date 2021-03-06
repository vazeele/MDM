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
    public partial class FIN_CostCenterwiseConfiguration
    {
    	[DataMember]
    	public int RevNo { get; set; }
    	[DataMember]
    	public string CostCenterCode { get; set; }
    	[DataMember]
    	public string BaseCurrencyCode { get; set; }
    	[DataMember]
    	public string AdjustmentAccNo { get; set; }
    	[DataMember]
    	public string AdjustmentCostCenter { get; set; }
    	[DataMember]
    	public string AdjustmentCurrCode { get; set; }
    	[DataMember]
    	public string DateDisplayFormat { get; set; }
    	[DataMember]
    	public string APCostCenterCode { get; set; }
    	[DataMember]
    	public string DocCodeForInvoice { get; set; }
    	[DataMember]
    	public string TxnCodeForInvoice { get; set; }
    	[DataMember]
    	public string DocCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string TxnCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string DocCodeForRevalGainLoss { get; set; }
    	[DataMember]
    	public string TxnCodeForRevalGainLoss { get; set; }
    	[DataMember]
    	public string BaseGLAccountForRetainedProfit { get; set; }
    	[DataMember]
    	public string BaseCostCenterForRetainedProfit { get; set; }
    	[DataMember]
    	public string BaseCurrencyCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string PNLAccountCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string PNLCostCenterForRetainedProfit { get; set; }
    	[DataMember]
    	public string PNLCurrencyCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string DbtAccountCodeRetainedProfit { get; set; }
    	[DataMember]
    	public string DbtCostCenterRetainedProfit { get; set; }
    	[DataMember]
    	public string DbtCurrencyCodeForRetainedProfit { get; set; }
    	[DataMember]
    	public string CrdAccountCodeRetainedProfit { get; set; }
    	[DataMember]
    	public string CrdCostCenterRetainedProfit { get; set; }
    	[DataMember]
    	public string CrdCurrencyCodeForRetainedProfit { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual ERP_DocumentAttributes ERP_DocumentAttributes { get; set; }
    	[DataMember]
        public virtual ERP_DocumentAttributes ERP_DocumentAttributes1 { get; set; }
    	[DataMember]
        public virtual ERP_DocumentAttributes ERP_DocumentAttributes2 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter1 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter2 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter3 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter4 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter5 { get; set; }
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter6 { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency1 { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency2 { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount1 { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency3 { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount2 { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount3 { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency4 { get; set; }
    	[DataMember]
        public virtual FIN_Currency FIN_Currency5 { get; set; }
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount4 { get; set; }
    public FIN_CostCenterwiseConfiguration Clone(){
    	return this.MemberwiseClone() as FIN_CostCenterwiseConfiguration;
    }
    public override String ToString(){
       return RevNo.ToString();
    }
    }
}
