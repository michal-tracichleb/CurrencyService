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

        public async Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date)
        {
            return await _currencyRepository.GetRatesByDateAsync(date);
        }

        public async Task FetchAndSaveRatesForLast7DaysAsync(DateTime dateTime)
        {
            var rates = await _nbpApiService.FetchRatesForLast7DaysAsync(dateTime);
            var existingRates = await _currencyRepository.GetRatesByDateRangeAsync(dateTime.AddDays(-7), dateTime);

            var newRates = rates
                .Where(rate => !existingRates.Any(existing =>
                    existing.Code == rate.Code &&
                    existing.Date == rate.Date &&
                    existing.TableType == rate.TableType))
                .ToList();

            if (newRates.Any())
            {
                await _currencyRepository.SaveRatesAsync(newRates);
            }
        }
    }
}