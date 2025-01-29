using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Interfaces;
using CurrencyRates.Services.Services;
using Moq;

namespace CurrencyRates.Tests.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyRepository> _mockCurrencyRepo;
        private readonly Mock<INbpApiService> _mockNbpApiService;
        private readonly Mock<IDateLogRepository> _mockDateLogRepo;
        private readonly CurrencyService _currencyService;

        public CurrencyServiceTests()
        {
            _mockCurrencyRepo = new Mock<ICurrencyRepository>();
            _mockNbpApiService = new Mock<INbpApiService>();
            _mockDateLogRepo = new Mock<IDateLogRepository>();

            _currencyService = new CurrencyService(_mockCurrencyRepo.Object, _mockNbpApiService.Object, _mockDateLogRepo.Object);
        }

        [Fact]
        public async Task GetAllCurrenciesAsync_ShouldReturnCurrencies()
        {
            // Arrange
            var mockCurrencies = new List<CurrencyRate>
            {
                new CurrencyRate { Id = 1, Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = DateTime.Today },
                new CurrencyRate { Id = 2, Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = DateTime.Today }
            };

            _mockCurrencyRepo.Setup(repo => repo.GetAllCurrenciesAsync())
                .ReturnsAsync(mockCurrencies);

            // Act
            var result = await _currencyService.GetAllCurrenciesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Code == "USD");
        }

        [Fact]
        public async Task FetchAndSaveMissingRatesAsync_ShouldSaveOnlyNewRates()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today;

            var fetchedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Date = DateTime.Today, TableType = "A", Rate = 3.95M },
                new CurrencyRate { Code = "EUR", Date = DateTime.Today, TableType = "A", Rate = 4.50M }
            };

            var existingRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Date = DateTime.Today, TableType = "A", Rate = 3.95M }
            };

            _mockNbpApiService.Setup(api => api.FetchRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(fetchedRates);

            _mockCurrencyRepo.Setup(repo => repo.GetRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(existingRates);

            // Act
            await _currencyService.FetchAndSaveMissingRatesAsync(startDate, endDate);

            // Assert
            _mockCurrencyRepo.Verify(repo => repo.SaveRatesAsync(It.Is<List<CurrencyRate>>(rates => rates.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task FetchAndSaveMissingRatesAsync_ShouldNotSaveIfAllRatesExist()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today;

            var fetchedRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Date = DateTime.Today, TableType = "A", Rate = 3.95M }
            };

            var existingRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Date = DateTime.Today, TableType = "A", Rate = 3.95M }
            };

            _mockNbpApiService.Setup(api => api.FetchRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(fetchedRates);

            _mockCurrencyRepo.Setup(repo => repo.GetRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(existingRates);

            // Act
            await _currencyService.FetchAndSaveMissingRatesAsync(startDate, endDate);

            // Assert
            _mockCurrencyRepo.Verify(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()), Times.Never);
        }

        [Fact]
        public async Task GetCurrenciesByDateRangeAsync_ShouldReturnExistingRatesIfAllLogged()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today;

            var existingCurrencies = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", Date = DateTime.Today, TableType = "A", Rate = 3.95M }
            };

            _mockDateLogRepo.Setup(repo => repo.GetLoggedDatesAsync(startDate, endDate))
                .ReturnsAsync(new List<DateTime> { startDate, startDate.AddDays(1), startDate.AddDays(2), startDate.AddDays(3), startDate.AddDays(4), startDate.AddDays(5), startDate.AddDays(6), endDate });

            _mockCurrencyRepo.Setup(repo => repo.GetRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(existingCurrencies);

            // Act
            var result = await _currencyService.GetCurrenciesByDateRangeAsync(startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("USD", result.First().Code);
        }

        [Fact]
        public async Task FetchAndSaveMissingRatesAsync_WhenApiReturnsEmpty_ShouldNotSave()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today;

            _mockNbpApiService.Setup(api => api.FetchRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(new List<CurrencyRate>());

            _mockCurrencyRepo.Setup(repo => repo.GetRatesByDateRangeAsync(startDate, endDate))
                .ReturnsAsync(new List<CurrencyRate>());

            // Act
            await _currencyService.FetchAndSaveMissingRatesAsync(startDate, endDate);

            // Assert
            _mockCurrencyRepo.Verify(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()), Times.Never);
        }
    }
}