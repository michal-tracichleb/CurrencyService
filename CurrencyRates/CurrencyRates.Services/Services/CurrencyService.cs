using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Interfaces;

namespace CurrencyRates.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly INbpApiService _nbpApiService;
        private readonly IDateLogRepository _dateLogRepository;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            INbpApiService nbpApiService,
            IDateLogRepository dateLogRepository)
        {
            _currencyRepository = currencyRepository;
            _nbpApiService = nbpApiService;
            _dateLogRepository = dateLogRepository;
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

        public async Task<List<CurrencyRate>> GetCurrenciesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var existingDates = await _dateLogRepository.GetLoggedDatesAsync(startDate, endDate);

            var missingDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                         .Select(d => startDate.AddDays(d))
                                         .Where(date => !existingDates.Contains(date))
                                         .OrderBy(d => d)
                                         .ToList();

            while (missingDates.Any())
            {
                var batchStart = missingDates.First();
                var batchEnd = missingDates.Skip(6).FirstOrDefault(batchStart) > endDate
                    ? endDate
                    : missingDates.Skip(6).FirstOrDefault(batchStart);

                await FetchAndSaveMissingRatesAsync(batchStart, batchEnd);
                var fetchedDates = Enumerable.Range(0, (batchEnd - batchStart).Days + 1)
                                             .Select(d => batchStart.AddDays(d))
                                             .ToList();

                await _dateLogRepository.SaveLoggedDatesAsync(fetchedDates);

                missingDates.RemoveAll(d => d >= batchStart && d <= batchEnd);
            }

            return await _currencyRepository.GetRatesByDateRangeAsync(startDate, endDate);
        }
    }
}