namespace SmartBiz.MDMAPI.API.Queries
{
    public class UnitConversionQuery
    {
        public string FromUnitCode { get; set; }

        public string ToUnitCode { get; set; }

        public decimal? ConversionFactor { get; set; }
    }
}