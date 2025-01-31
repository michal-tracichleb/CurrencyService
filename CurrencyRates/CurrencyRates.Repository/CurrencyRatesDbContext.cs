using CurrencyRates.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRates.Repository
{
    public class CurrencyRatesDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<DateLog> DateLogs { get; set; }

        public CurrencyRatesDbContext(DbContextOptions<CurrencyRatesDbContext> options) : base(options)
        {
        }
    }
}