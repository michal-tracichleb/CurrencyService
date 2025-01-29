using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.DTO;
using CurrencyRates.Services.Interfaces;
using Newtonsoft.Json;

namespace CurrencyRates.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyRate>> GetAllCurrenciesAsync()
        {
            return await _currencyRepository.GetAllCurrenciesAsync();
        }

        public async Task<List<CurrencyRate>> GetRatesByDateAsync(DateTime date)
        {
            return await _currencyRepository.GetRatesByDateAsync(date);
        }

        public async Task FetchAndSaveRatesForLast7DaysAsync(DateTime dateTime)
        {
            using var httpClient = new HttpClient();

            var endDate = dateTime.ToString("yyyy-MM-dd");
            var startDate = dateTime.AddDays(-7).ToString("yyyy-MM-dd");

            string[] tables = { "A", "B", "C" };

            var allRates = new List<CurrencyRate>();
            foreach (var table in tables)
            {
                string apiUrl = $"https://api.nbp.pl/api/exchangerates/tables/{table}/{startDate}/{endDate}/?format=json";

                try
                {
                    var response = await httpClient.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Błąd pobierania danych dla tabeli {table}: {response.StatusCode}");
                        continue;
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var rateTables = JsonConvert.DeserializeObject<List<NBPTableResponse>>(content);

                    if (rateTables != null)
                    {
                        var ratesFromTable = rateTables
                            .SelectMany(tableData => tableData.Rates.Select(rate => new CurrencyRate
                            {
                                CurrencyName = rate.Currency,
                                Code = rate.Code,
                                Rate = rate.Mid,
                                Date = DateTime.Parse(tableData.EffectiveDate),
                                TableType = table
                            }))
                            .ToList();

                        allRates.AddRange(ratesFromTable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas pobierania danych z tabeli {table}: {ex.Message}");
                }
            }

            if (allRates.Any())
            {
                await _currencyRepository.SaveRatesAsync(allRates);
            }
        }
    }
}