
using LukNotificator.Entities;
using LukNotificator.Services.Repository;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using Newtonsoft.Json;

namespace LukNotificator.Services
{
    internal class OwnCurRepository(IConfiguration conf) : IOwnCurRepository
    {
        private readonly string? _conString = conf["pgconnstr"];

        public async Task<IEnumerable<OwnCurrency>> GetAll(Guid userId)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();

            var curs = await con.QueryAsync<OwnCurrencyDb>("select * from public.\"owncurrency\" where userid = @id", 
                new
                {
                    id = userId
                });
            var res = curs.Select(DbToEntity);
            return res;
        }

        private OwnCurrency DbToEntity(OwnCurrencyDb x)
        {
            return new OwnCurrency()
            {
                Code = x.Code,
                Id = x.Id,
                UserId = x.UserId,
                Props = string.IsNullOrEmpty(x.Props) 
                    ? new OwnCurProps() : 
                    JsonConvert.DeserializeObject<OwnCurProps>(x.Props)
            };
        }

        public async Task<OwnCurrency> GetOrCreate(Guid userId, string code)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();

            var cur = await con.QueryFirstOrDefaultAsync<OwnCurrencyDb>("select * from public.\"owncurrency\" where userid = @id and code = @code", 
                new
                {
                    id = userId, code
                });

            if (cur == null)
            {
                cur = new OwnCurrencyDb() { Code = code, UserId = userId, Id = Guid.NewGuid() };
                await con.ExecuteAsync(
                    "insert into public.\"owncurrency\"(id, code, userid, props) values (@id, @code, @userid, @props)", cur);
            }

            var res = DbToEntity(cur);
            return res;
        }

        public async Task SaveProps(Guid id, OwnCurProps props)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();

            await con.ExecuteAsync(
                "update public.\"owncurrency\" set props = @props where id = @id", new
                {
                    id,
                    props = JsonConvert.SerializeObject(props)
                });
        }

        public async Task RemoveCurrency(Guid id)
        {
            await using var con = new NpgsqlConnection(_conString);
            con.Open();

            await con.ExecuteAsync(
                "delete from public.\"owncurrency\" where id = @id", new { id });
        }

    }
}
