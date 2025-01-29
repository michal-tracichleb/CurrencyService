namespace CurrencyRates.Repository.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
        public string TableType { get; set; }
    }
}