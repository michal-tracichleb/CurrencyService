using CurrencyRates.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRates.Repository
{
    public class CurrencyRatesDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public CurrencyRatesDbContext(DbContextOptions<CurrencyRatesDbContext> options) : base(options)
        {
        }
    }
}