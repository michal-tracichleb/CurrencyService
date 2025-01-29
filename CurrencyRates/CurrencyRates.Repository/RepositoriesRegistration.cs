using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRates.Repository
{
    public static class RepositoriesRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IDateLogRepository, DateLogRepository>();
        }
    }
}