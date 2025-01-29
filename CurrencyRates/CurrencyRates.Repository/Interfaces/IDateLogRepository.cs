namespace CurrencyRates.Repository.Interfaces
{
    public interface IDateLogRepository
    {
        Task<List<DateTime>> GetLoggedDatesAsync(DateTime startDate, DateTime endDate);

        Task SaveLoggedDatesAsync(List<DateTime> dates);
    }
}