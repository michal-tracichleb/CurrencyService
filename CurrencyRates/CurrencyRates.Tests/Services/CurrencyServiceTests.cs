using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Interfaces;
using CurrencyRates.Services.Services;
using Moq;

namespace CurrencyRates.Tests.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyRepository> _mockRepo;
        private readonly Mock<INbpApiService> _mockNbpApiService;
        private readonly CurrencyService _service;

        public CurrencyServiceTests()
        {
            _mockRepo = new Mock<ICurrencyRepository>();
            _mockNbpApiService = new Mock<INbpApiService>();
            _service = new CurrencyService(_mockRepo.Object, _mockNbpApiService.Object);
        }

        [Fact]
        public async Task FetchAndSaveRates_ShouldCallSaveRates_WhenNewRatesExist()
        {
            // Arrange
            var date = DateTime.Now;
            var mockRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = date },
                new CurrencyRate { Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = date }
            };

            _mockNbpApiService.Setup(api => api.FetchRatesForLast7DaysAsync(date))
                .ReturnsAsync(mockRates);

            _mockRepo.Setup(repo => repo.GetRatesByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<CurrencyRate>());

            // Act
            await _service.FetchAndSaveRatesForLast7DaysAsync(date);

            // Assert
            _mockRepo.Verify(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()), Times.Once);
        }

        [Fact]
        public async Task FetchAndSaveRates_ShouldNotCallSaveRates_WhenRatesAlreadyExist()
        {
            // Arrange
            var date = DateTime.Now;
            var mockRates = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = date },
                new CurrencyRate { Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = date }
            };

            _mockNbpApiService.Setup(api => api.FetchRatesForLast7DaysAsync(date))
                .ReturnsAsync(mockRates);

            _mockRepo.Setup(repo => repo.GetRatesByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(mockRates);

            // Act
            await _service.FetchAndSaveRatesForLast7DaysAsync(date);

            // Assert
            _mockRepo.Verify(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()), Times.Never);
        }

        [Fact]
        public async Task GetAllCurrencies_ShouldReturnAllCurrencies()
        {
            // Arrange
            var mockCurrencies = new List<CurrencyRate>
            {
                new CurrencyRate { Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = DateTime.Now },
                new CurrencyRate { Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = DateTime.Now }
            };

            _mockRepo.Setup(repo => repo.GetAllCurrenciesAsync())
                .ReturnsAsync(mockCurrencies);

            // Act
            var result = await _service.GetAllCurrenciesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Code == "USD");
        }
    }
}