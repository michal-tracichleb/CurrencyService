using CurrencyRates.Models.Currencies;
using CurrencyRates.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CurrencyRates.Controllers
{
    [Authorize]
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
            var vm = new CurrenciesViewModel();
            vm.StartDate = startDate ?? DateTime.Today;
            vm.EndDate = endDate ?? DateTime.Today;

            var currencies = await _currencyService.GetCurrenciesByDateRangeAsync(vm.StartDate, vm.EndDate);
            vm.Items = currencies.OrderByDescending(c => c.Date).ToList();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadCsv(DateTime? startDate, DateTime? endDate)
        {
            DateTime start = startDate ?? DateTime.Today;
            DateTime end = endDate ?? DateTime.Today;

            var currencies = await _currencyService.GetCurrenciesByDateRangeAsync(start, end);
            var list = currencies.OrderByDescending(c => c.Date).ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CurrencyName,Code,Rate,Date,TableType");

            foreach (var item in list)
            {
                csvBuilder.AppendLine($"{item.CurrencyName},{item.Code},{item.Rate},{item.Date:yyyy-MM-dd},{item.TableType}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(csvBytes, "text/csv", $"Currencies_{start.ToString("yyyy-MM-dd")}_{end.ToString("yyyy-MM-dd")}.csv");
        }
    }
}