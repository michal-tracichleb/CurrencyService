﻿using CurrencyRates.Repository.Models;

namespace CurrencyRates.Repository.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<CurrencyRate>> GetAllCurrenciesAsync();

        Task SaveRatesAsync(List<CurrencyRate> rates);

        Task<List<CurrencyRate>> GetRatesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}