using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Interfaces;
using Newtonsoft.Json;

namespace CurrencyRates.Services.Services
{
    internal class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyRate>> GetAllCurrenciesAsync()
        {
            return await _currencyRepository.GetAllCurrenciesAsync();
        }

        public async Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date)
        {
            return await _currencyRepository.GetRatesByDateAsync(date);
        }

        public async Task FetchAndSaveRatesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://api.nbp.pl/api/exchangerates/tables/A/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rates = JsonConvert.DeserializeObject<List<CurrencyRate>>(content);

            await _currencyRepository.SaveRatesAsync(rates);
        }
    }
}