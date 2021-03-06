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
    [DataContract(IsReference=true)]
    public partial class FIN_ControledTransaction
    {
    	[DataMember]
    	public string DocCode { get; set; }
    	[DataMember]
    	public string TxCode { get; set; }
    	[DataMember]
    	public string GLAccountNo { get; set; }
    	[DataMember]
    	public string CostCenterCode { get; set; }
    	[DataMember]
    	public string CurrencyCode { get; set; }
    	[DataMember]
    	public Nullable<int> CreditDebit_Flag { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }

        private ERP_DocumentAttributes _ERP_DocumentAttributes;
        [DataMember]
        public virtual ERP_DocumentAttributes ERP_DocumentAttributes
        {
            get
            {
                return _ERP_DocumentAttributes;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("Document Attributes is required");

                _ERP_DocumentAttributes = value;
            }
        }
        private FIN_CostCenter _FIN_CostCenter;
    	[DataMember]
        public virtual FIN_CostCenter FIN_CostCenter {
            get
            {
                return _FIN_CostCenter;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("Cost Center is required");

                _FIN_CostCenter = value;
            }
        }
        private FIN_Currency _FIN_Currency;
    	[DataMember]
        public virtual FIN_Currency FIN_Currency
        {
            get
            {
                return _FIN_Currency;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("Currency is required");

                _FIN_Currency = value;
            }
        }
        private FIN_GeneralLedgerAccount _FIN_GeneralLedgerAccount;
    	[DataMember]
        public virtual FIN_GeneralLedgerAccount FIN_GeneralLedgerAccount
        {
            get
            {
                return _FIN_GeneralLedgerAccount;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("General Ledger Account is required");

                _FIN_GeneralLedgerAccount = value;
            }
        }
    public FIN_ControledTransaction Clone(){
    	return this.MemberwiseClone() as FIN_ControledTransaction;
    }
    public override String ToString(){
       return DocCode.ToString();
    }
    }
}
