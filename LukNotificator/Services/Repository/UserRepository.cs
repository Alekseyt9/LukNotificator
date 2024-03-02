
using Dapper;
using LukNotificator.Entity;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LukNotificator.Services
{
    internal class UserRepository(IConfiguration conf) : IUserRepository
    {
        private readonly string? _conString = conf["pgconnstr"];

        public async Task<User> GetOrCreate(long channelId)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            var user = await con.QueryFirstOrDefaultAsync<User>("select * from public.\"user\" where channelid = @id", new { id = channelId });
            if (user != null)
            {
                return user;
            }
            user = new User() { ChannelId = channelId, Id = Guid.NewGuid() };
            await con.ExecuteAsync("insert into public.\"user\"(id, channelid) values(@id, @channelid)", new { user.Id, user.ChannelId });

            return user;
        }

        public Task<User> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(long chanId)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryFirstAsync<User>("select * from public.\"user\" where channelid = @channelid", new { channelid = chanId });
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<User>("select * from public.\"user\"");
        }

    }
}
