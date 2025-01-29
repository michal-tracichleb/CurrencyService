namespace CurrencyRates.Services.DTO
{
    public class NBPTableResponse
    {
        public string Table { get; set; }
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public List<NBPRate> Rates { get; set; }
    }

    public class NBPRate
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
    }
}