using CurrencyRates.Repository.Models;

namespace CurrencyRates.Repository.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRate>> GetAllCurrenciesAsync();

        Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date);

        Task SaveRatesAsync(List<CurrencyRate> rates);

        Task<List<CurrencyRate>> GetRatesByDateRangeAsync(string startDate, string endDate);
    }
}