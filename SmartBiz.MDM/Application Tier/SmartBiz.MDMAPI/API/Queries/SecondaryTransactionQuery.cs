namespace SmartBiz.MDMAPI.API.Queries
{
    public class SecondaryTransactionQuery
    {
        public string DocumentCode { get; set; }

        public string TransactionCode { get; set; }

        public int? FinancialYear { get; set; }
    }
}