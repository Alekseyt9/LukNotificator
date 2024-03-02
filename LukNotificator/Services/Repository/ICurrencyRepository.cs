using LukNotificator.Entity;


namespace LukNotificator.Services
{
    public interface ICurrencyRepository
    {
        Task<Currency> AddCurrency(User user, string code, double value);

        Task RemoveCurrency(User user, string code);

        Task<IEnumerable<Currency>> GetCurrencies(User user);

        Task<IEnumerable<Currency>> GetCurrencies();

        Task UpdateCurrency(Guid curId, bool isTriggered);
    }
}
