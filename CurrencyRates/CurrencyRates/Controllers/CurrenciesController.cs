using Microsoft.AspNetCore.Mvc;

namespace CurrencyRates.Controllers
{
    public class CurrenciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}