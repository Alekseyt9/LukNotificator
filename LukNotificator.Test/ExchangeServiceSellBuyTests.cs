using LukNotificator.Services.Exchange;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LukNotificator.Test
{
    public class ExchangeServiceSellBuyTests
    {
        private IConfiguration _configuration { get; }

        public ExchangeServiceSellBuyTests()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("secrets.json");
            _configuration = builder.Build();
        }

        [Fact]
        public async Task Test1()
        {
            var exServ = new ExchangeService(_configuration);
            var res = await exServ.Buy("XRP", 0.5);
        }

        [Fact]
        public async Task Test2()
        {
            var exServ = new ExchangeService(_configuration);
            var res = await exServ.Sell("XRP", 0.792);
        }

    }
}
