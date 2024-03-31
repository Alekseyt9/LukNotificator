
using LukNotificator.Services.Exchange;
using LukNotificator.Services.Exchange.Impl;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class KukoinExchangeServiceTests
    {
        private IConfiguration _configuration { get; }

        public KukoinExchangeServiceTests()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("secrets.json");
            _configuration = builder.Build();
        }

        [Fact]
        public async Task Test1()
        {
            var exServ = new KukoinExchangeService(_configuration);
            var res = await exServ.GetUsdtPairs(new string[] { "PENDLE", "AKT" });
        }

        [Fact]
        public async Task Test2()
        {
            var exServ = new KukoinExchangeService(_configuration);
            var res = await exServ.GetOwnCurrencies();
        }

    }
}
