using CurrencyRates.Repository.Models;

namespace CurrencyRates.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyRate>> GetAllCurrenciesAsync();

        Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date);

        Task FetchAndSaveRatesAsync();
    }
}