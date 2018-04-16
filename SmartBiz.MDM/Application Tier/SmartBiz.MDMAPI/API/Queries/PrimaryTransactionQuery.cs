namespace SmartBiz.MDMAPI.API.Queries
{
    public class PrimaryTransactionQuery
    {
        public string DocumentCode { get; set; }

        public string TransactionCode { get; set; }

        public int? FinancialYear { get; set; }
    }
}