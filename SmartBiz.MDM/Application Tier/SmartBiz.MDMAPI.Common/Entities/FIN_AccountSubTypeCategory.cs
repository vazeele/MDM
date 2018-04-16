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
    public partial class FIN_AccountSubTypeCategory
    {
    	
        public FIN_AccountSubTypeCategory()
        {
            this.FIN_GeneralLedgerAccount = new HashSet<FIN_GeneralLedgerAccount>();
        }
    
    	[DataMember]
    	public int AccountType { get; set; }
    	[DataMember]
    	public int AccountSubType { get; set; }
    	[DataMember]
    	public int AccountSubCatType { get; set; }
    	[DataMember]
    	public string SubTypeDescription { get; set; }
    	public string EnteredUser { get; set; }
        public string EnteredFrom { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual FIN_AccountSubType FIN_AccountSubType { get; set; }
    	[DataMember]
        public virtual ICollection<FIN_GeneralLedgerAccount> FIN_GeneralLedgerAccount { get; set; }
    public FIN_AccountSubTypeCategory Clone(){
    	return this.MemberwiseClone() as FIN_AccountSubTypeCategory;
    }
    public override String ToString(){
       return AccountType.ToString();
    }
    }
}
