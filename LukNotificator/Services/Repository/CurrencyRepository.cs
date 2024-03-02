using LukNotificator.Entity;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace LukNotificator.Services
{
    internal class CurrencyRepository(IConfiguration conf) : ICurrencyRepository
    {
        private readonly string? _conString = conf["pgconnstr"];

        public async Task<Currency> Add(User user, string code, double value)
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

        public async Task Remove(User user, string code)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            await con.ExecuteAsync("delete from public.\"currency\" where userid = @userid and code = @code",
                new { userid = user.Id, code = code });
        }

        public async Task<IEnumerable<Currency>> GetAll(User user)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<Currency>("select * from public.\"currency\" where userid = @id", new { id = user.Id });
        }

        public async Task<IEnumerable<Currency>> GetAll()
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            return await con.QueryAsync<Currency>("select * from public.\"currency\"");
        }

        public async Task Update(Guid curId, bool isTriggered)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();
            await con.ExecuteAsync("update public.\"currency\" set istriggered = @istriggered where id = @id",
                new { id = curId, istriggered = isTriggered });
        }
    }
}
