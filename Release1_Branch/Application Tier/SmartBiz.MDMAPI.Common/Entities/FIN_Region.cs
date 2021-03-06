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
    public partial class FIN_Region
    {
    	
        public FIN_Region()
        {
            this.FIN_Area = new HashSet<FIN_Area>();
        }
    
    	[DataMember]
    	public string RegionCode { get; set; }
    	[DataMember]
    	public string RegionName { get; set; }
    	public string EnteredFrom { get; set; }
    	public string EnteredUser { get; set; }
    	public Nullable<System.DateTime> EnteredDate { get; set; }
    	public string ModifiedFrom { get; set; }
    	public string ModifiedUser { get; set; }
    	public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    	[DataMember]
        public virtual ICollection<FIN_Area> FIN_Area { get; set; }
    public FIN_Region Clone(){
    	return this.MemberwiseClone() as FIN_Region;
    }
    public override String ToString(){
       return RegionCode.ToString();
    }
    }
}
