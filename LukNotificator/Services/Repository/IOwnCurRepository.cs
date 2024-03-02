
using LukNotificator.Entities;

namespace LukNotificator.Services.Repository
{
    public interface IOwnCurRepository
    {
        Task<IEnumerable<OwnCurrency>> GetAll(Guid userId);

        Task<OwnCurrency> GetOrCreate(Guid userId, string code);

        Task SaveProps(Guid id, OwnCurProps props);

        Task RemoveCurrency(Guid id);
    }
}
