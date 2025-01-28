using CurrencyRates.Services.Interfaces;
using CurrencyRates.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRates.Services
{
    public static class ServicesRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyService, CurrencyService>();
        }
    }
}