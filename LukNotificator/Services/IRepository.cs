
using LukNotificator.Entity;

namespace LukNotificator.Services
{
    internal interface IRepository
    {
        Task<User> GetOrCreateUser(long channelId);

        Task<Currency> AddCurrency(User user, string code, double value);

        Task RemoveCurrency(User user, string code);

        Task<IEnumerable<Currency>> GetCurrencies(User user);

        Task<IEnumerable<Currency>> GetCurrencies();

        Task<User> GetUser(Guid id);

        Task<User> GetUser(long chanId);

        Task<IEnumerable<User>> GetUsers();

    }
}
