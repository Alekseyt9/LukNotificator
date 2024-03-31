
using LukNotificator.Services.Exchange.Impl;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class BybitExchangeServiceTests
    {
        private IConfiguration _configuration { get; }

        public BybitExchangeServiceTests()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("secrets.json");
            _configuration = builder.Build();
        }

        [Fact]
        public async Task Test1()
        {
            var exServ = new BybitExchangeService(_configuration);
            var res = await exServ.GetUsdtPairs(new string[] { "PENDLE", "SNX" });
        }



    }
}
