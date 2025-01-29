using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRates.Repository.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyRatesDbContext _context;

        public CurrencyRepository(CurrencyRatesDbContext context)
        {
            _context = context;
        }

        public async Task<List<CurrencyRate>> GetAllCurrenciesAsync()
        {
            return await _context.CurrencyRates.ToListAsync();
        }

        public async Task SaveRatesAsync(List<CurrencyRate> rates)
        {
            await _context.CurrencyRates.AddRangeAsync(rates);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CurrencyRate>> GetRatesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.CurrencyRates
                .Where(r => r.Date >= startDate && r.Date <= endDate)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }
    }
}