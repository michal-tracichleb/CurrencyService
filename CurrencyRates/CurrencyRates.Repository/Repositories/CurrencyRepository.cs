using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;

namespace CurrencyRates.Repository.Repositories
{
    internal class CurrencyRepository : ICurrencyRepository
    {
        private readonly List<CurrencyRate> _data = new();

        public async Task<List<CurrencyRate>> GetAllCurrenciesAsync()
        {
            return await Task.FromResult(_data);
        }

        public async Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date)
        {
            return await Task.FromResult(_data.Where(r => r.Date == date).ToList());
        }

        public async Task SaveRatesAsync(List<CurrencyRate> rates)
        {
            _data.AddRange(rates);
            await Task.CompletedTask;
        }
    }
}