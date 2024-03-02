
using LukNotificator.Services;
using Microsoft.Extensions.Configuration;
using LukNotificator.Services.Repository;
using Xunit;

namespace LukNotificator.Test
{
    public class OwnCurRepositoryTest
    {
        private readonly IConfiguration _configuration;
        private readonly IOwnCurRepository _curRep;
        private readonly IUserRepository _userRep;

        public OwnCurRepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", false, true);

            _configuration = builder.Build();
            _configuration["pgconnstr"] = $"Server=37.77.105.224; Port=5432; Database=lukbot; User Id=alex; Password={_configuration["pgpass"]};";
            _curRep = new OwnCurRepository(_configuration);
            _userRep = new UserRepository(_configuration);
        }

        [Fact]
        public async Task Test1()
        {
            var user = await _userRep.GetOrCreate(-1);
            var cur = await _curRep.GetOrCreate(user.Id, "XRP");

            cur.Props.BuyValue = 1;
            cur.Props.MaxValue = 2;

            await _curRep.SaveProps(cur.Id, cur.Props);
            var cur1 = await _curRep.GetOrCreate(user.Id, "XRP");
        }

    }
}
