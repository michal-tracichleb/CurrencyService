using CurrencyRates.Repository.Models;

namespace CurrencyRates.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyRate>> GetAllCurrenciesAsync();

        Task FetchAndSaveMissingRatesAsync(DateTime startDate, DateTime endDate);

        Task<List<CurrencyRate>> GetCurrenciesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}