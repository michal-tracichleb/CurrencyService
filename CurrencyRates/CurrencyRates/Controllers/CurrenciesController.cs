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

        public async Task<IActionResult> Index()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return View(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> FetchRates()
        {
            await _currencyService.FetchAndSaveRatesForLast7DaysAsync(DateTime.UtcNow);
            return RedirectToAction("Index");
        }
    }
}