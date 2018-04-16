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
    public partial class ERP_LastTransactionInfo
    {
        private string _ProductCode;
    	[DataMember]
        public string ProductCode {
            get
            {
                return _ProductCode;
            }
            set
            {
                if (value == null)
                    throw new ValidationException("Product code is required");

                _ProductCode = value;
            }

        }
        private string _SubSystemCode;
    	[DataMember]
    	public string SubSystemCode {
            get
            {

                return _SubSystemCode;
            }
            set 
            {
                if (value == null)
                    throw new ValidationException("Subsystem code is required");

                _SubSystemCode = value;
            }
        }
    	[DataMember]
    	public string DocCode { get; set; }
    	[DataMember]
    	public string TxCode { get; set; }
    	[DataMember]
    	public Nullable<int> LastVoucherNo { get; set; }
    	[DataMember]
    	public Nullable<int> LastTxnSerialNo { get; set; }
    	[DataMember]
    	public Nullable<int> TransactionType { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
        private ERP_DocumentAttributes _ERP_DocumentAttributes;
    	[DataMember]
        public virtual ERP_DocumentAttributes ERP_DocumentAttributes {
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
    public ERP_LastTransactionInfo Clone(){
    	return this.MemberwiseClone() as ERP_LastTransactionInfo;
    }
    public override String ToString(){
       return ProductCode.ToString();
    }
    }
}
