using CurrencyRates.Repository.Models;

namespace CurrencyRates.Models.Currencies
{
    public class CurrenciesViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CurrencyRate> Items { get; set; }
    }
}