using CurrencyRates.Repository.Models;

namespace CurrencyRates.Services.Interfaces
{
    public interface INbpApiService
    {
        Task<List<CurrencyRate>> FetchRatesForLast7DaysAsync(DateTime referenceDate);
    }
}