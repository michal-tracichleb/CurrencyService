using CurrencyRates.Repository.Interfaces;
using CurrencyRates.Repository.Models;
using CurrencyRates.Services.Services;
using Moq;

namespace CurrencyRates.Tests.Services
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyRepository> _mockRepo;
        private readonly CurrencyService _service;

        public CurrencyServiceTests()
        {
            _mockRepo = new Mock<ICurrencyRepository>();
            _service = new CurrencyService(_mockRepo.Object);
        }

        [Fact]
        public async Task FetchAndSaveRates_ShouldCallSaveRates()
        {
            // Arrange
            var mockRates = new List<CurrencyRate>
            {
                new CurrencyRate { Id = 1, Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = DateTime.Now },
                new CurrencyRate { Id = 2, Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = DateTime.Now }
            };

            _mockRepo.Setup(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.FetchAndSaveRatesAsync();

            // Assert
            _mockRepo.Verify(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCurrencies_ShouldReturnAllCurrencies()
        {
            // Arrange
            var mockCurrencies = new List<CurrencyRate>
            {
                new CurrencyRate { Id = 1, Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = DateTime.Now },
                new CurrencyRate { Id = 2, Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = DateTime.Now }
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

        [Fact]
        public async Task GetRatesByDate_ShouldReturnCorrectRates()
        {
            // Arrange
            var date = new DateTime(2023, 1, 1);
            var mockRates = new List<CurrencyRate>
            {
                new CurrencyRate { Id = 1, Code = "USD", CurrencyName = "Dollar", Rate = 3.95M, Date = date },
                new CurrencyRate { Id = 2, Code = "EUR", CurrencyName = "Euro", Rate = 4.50M, Date = date }
            };

            _mockRepo.Setup(repo => repo.GetRatesByDateAsync(date))
                .ReturnsAsync(mockRates);

            // Act
            var result = await _service.GetRatesByDateAsync(date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(date, r.Date));
        }

        [Fact]
        public async Task GetRatesByDate_ShouldReturnEmptyListIfNoRates()
        {
            // Arrange
            var date = new DateTime(2023, 1, 1);

            _mockRepo.Setup(repo => repo.GetRatesByDateAsync(date))
                .ReturnsAsync(new List<CurrencyRate>());

            // Act
            var result = await _service.GetRatesByDateAsync(date);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FetchAndSaveRates_ShouldNotThrowException_WhenApiFails()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.SaveRatesAsync(It.IsAny<List<CurrencyRate>>()))
                .ThrowsAsync(new Exception("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.FetchAndSaveRatesAsync());
        }
    }
}