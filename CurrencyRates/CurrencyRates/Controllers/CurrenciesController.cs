using CurrencyRates.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRates.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime start = startDate ?? DateTime.Today;
            DateTime end = endDate ?? DateTime.Today;

            var currencies = await _currencyService.GetCurrenciesByDateRangeAsync(start, end);

            return View(currencies.OrderByDescending(c => c.Date).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> FetchRates()
        {
            await _currencyService.FetchAndSaveMissingRatesAsync(DateTime.UtcNow, DateTime.UtcNow);
            return RedirectToAction("Index");
        }
    }
}