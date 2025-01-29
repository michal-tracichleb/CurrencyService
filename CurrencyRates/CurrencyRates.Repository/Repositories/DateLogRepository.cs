using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRates.Repository.Repositories
{
    public class DateLogRepository : IDateLogRepository
    {
        private readonly CurrencyRatesDbContext _context;

        public DateLogRepository(CurrencyRatesDbContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetLoggedDatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DateLogs
                .Where(d => d.LoggedDate >= startDate && d.LoggedDate <= endDate)
                .Select(d => d.LoggedDate)
                .ToListAsync();
        }

        public async Task SaveLoggedDatesAsync(List<DateTime> dates)
        {
            var newLogs = dates.Select(date => new DateLog { LoggedDate = date }).ToList();
            await _context.DateLogs.AddRangeAsync(newLogs);
            await _context.SaveChangesAsync();
        }
    }
}