
using Dapper;
using LukNotificator.Entity;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Channels;
using User = LukNotificator.Entity.User;

namespace LukNotificator.Services
{
    internal class Repository(IConfiguration conf) : IRepository
    {
        private readonly string? _conString = conf["pgconnstr"];

        public async Task<User> GetOrCreateUser(long channelId)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            var user = await con.QueryFirstOrDefaultAsync<User>("select * from public.\"user\" where channelid = @id", new { id = channelId });
            if (user != null)
            {
                return user;
            }
            user = new User() { ChannelId = channelId, Id = Guid.NewGuid() };
            await con.ExecuteAsync("insert into public.\"user\"(id, channelid) values(@id, @channelid)", new {user.Id, user.ChannelId});

            return user;
        }

        public async Task<Currency> AddCurrency(User user, string code, double value)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            var cur = new Currency()
            {
                Code = code, 
                UserId = user.Id, 
                Price = value,
                Id = Guid.NewGuid()
            };
            await con.ExecuteAsync("insert into public.\"currency\"(id, code, userid, price) values(@id, @code, @userid, @price)", cur);
            return cur;
        }

        public async Task RemoveCurrency(User user, string code)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            await con.ExecuteAsync("delete from public.\"currency\" where userid = @userid and code = @code",
                new { userid = user.Id, code = code });
        }

        public async Task<IEnumerable<Currency>> GetCurrencies(User user)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<Currency>("select * from public.\"currency\" where userid = @id", new { id = user.Id });
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<Currency>("select * from public.\"currency\"");
        }

        public Task<User> GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUser(long chanId)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryFirstAsync<User>("select * from public.\"user\" where channelid = @channelid", new { channelid = chanId });
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<User>("select * from public.\"user\"");
        }
    }
}
