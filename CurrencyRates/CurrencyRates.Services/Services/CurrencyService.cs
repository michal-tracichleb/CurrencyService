using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Interfaces;

namespace CurrencyRates.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly INbpApiService _nbpApiService;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            INbpApiService nbpApiService)
        {
            _currencyRepository = currencyRepository;
            _nbpApiService = nbpApiService;
        }

        public async Task<List<CurrencyRate>> GetAllCurrenciesAsync()
        {
            return await _currencyRepository.GetAllCurrenciesAsync();
        }

        public async Task FetchAndSaveMissingRatesAsync(DateTime startDate, DateTime endDate)
        {
            var rates = await _nbpApiService.FetchRatesByDateRangeAsync(startDate, endDate);
            var existingRates = await _currencyRepository.GetRatesByDateRangeAsync(startDate, endDate);

            var newRates = rates
                .Where(rate => !existingRates.Exists(existing =>
                    existing.Code == rate.Code &&
                    existing.Date == rate.Date &&
                    existing.TableType == rate.TableType))
                .ToList();

            if (newRates.Any())
            {
                await _currencyRepository.SaveRatesAsync(newRates);
            }
        }

        public async Task<List<CurrencyRate>> GetCurrenciesByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _currencyRepository.GetRatesByDateRangeAsync(start, end);
        }
    }
}