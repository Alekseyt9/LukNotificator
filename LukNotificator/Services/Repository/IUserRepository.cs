
using LukNotificator.Entity;

namespace LukNotificator.Services
{
    internal interface IUserRepository
    {
        Task<User> GetUser(Guid id);

        Task<User> GetUser(long chanId);

        Task<IEnumerable<User>> GetUsers();

        Task<User> GetOrCreateUser(long channelId);
    }
}
