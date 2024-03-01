
using LukNotificator.Services.Exchange;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class ExchangeServiceTests
    {
        IConfiguration _configuration { get; set; }

        public ExchangeServiceTests()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("secrets.json");
            _configuration = builder.Build();
        }

        [Fact]
        public async Task Test1()
        {
            var exServ = new ExchangeService(_configuration);
            var res = await exServ.GetUsdtPairs(new string[] { "PENDLE", "AKT" });
        }

        [Fact]
        public async Task Test2()
        {
            var exServ = new ExchangeService(_configuration);
            var res = await exServ.GetOwnCurrencies();
        }

    }
}
