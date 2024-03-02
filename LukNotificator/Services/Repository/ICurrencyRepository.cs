
using LukNotificator.Entity;

namespace LukNotificator.Services
{
    public interface ICurrencyRepository
    {
        Task<Currency> Add(User user, string code, double value);

        Task Remove(User user, string code);

        Task<IEnumerable<Currency>> GetAll(User user);

        Task<IEnumerable<Currency>> GetAll();

        Task Update(Guid curId, bool isTriggered);
    }
}
