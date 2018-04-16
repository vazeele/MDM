namespace SmartBiz.MDMAPI.API.Queries
{
    public class PeriodQuery
    {
        public string ProductCode { get; set; }

        public int? FinancialYear { get; set; }

        public int? AccountingPeriod { get; set; }
    }
}