
using LukNotificator.Entity;

namespace LukNotificator.Services
{
    internal interface IUserRepository
    {
        Task<User> Get(Guid id);

        Task<User> Get(long chanId);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetOrCreate(long channelId);
    }
}
