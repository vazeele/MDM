namespace SmartBiz.MDMAPI.API.Queries
{
    public class TransactionReferenceQuery
    {
        public string DocCode { get; set; }

        public short? RefSeq { get; set; }

        public string ReferenceText { get; set; }
    }
}