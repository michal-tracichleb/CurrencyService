using CurrencyRates.Repository.Models;

namespace CurrencyRates.Services.Interfaces
{
    public interface INbpApiService
    {
        Task<List<CurrencyRate>> FetchRatesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}