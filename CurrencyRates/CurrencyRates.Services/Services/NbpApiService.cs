﻿namespace CurrencyRates.Services.Services
{
    using CurrencyRates.Repository.Models;
    using CurrencyRates.Services.DTO;
    using CurrencyRates.Services.Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class NbpApiService : INbpApiService
    {
        private readonly HttpClient _httpClient;

        public NbpApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyRate>> FetchRatesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var httpClient = new HttpClient();
            var tables = new[] { "A", "B", "C" };
            var allRates = new List<CurrencyRate>();

            foreach (var table in tables)
            {
                string apiUrl = $"https://api.nbp.pl/api/exchangerates/tables/{table}/{startDate.ToString("yyyy-MM-dd")}/{endDate.ToString("yyyy-MM-dd")}/?format=json";

                try
                {
                    var response = await _httpClient.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode) continue;

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
                    Console.WriteLine($"Error fetching data for table {table}: {ex.Message}");
                }
            }

            return allRates;
        }
    }
}