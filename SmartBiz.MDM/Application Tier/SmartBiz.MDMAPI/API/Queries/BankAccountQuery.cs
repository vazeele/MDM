namespace SmartBiz.MDMAPI.API.Queries
{
    public class BankAccountQuery
    {
        public string BankCode { get; set; }

        public string BankBranchCode { get; set; }

        public int? AccountSEQNo { get; set; }
    }
}