
using LukNotificator.Services;
using Xunit;

namespace LukNotificator.Test
{
    public class ExchangeServiceTests
    {
        [Fact]
        public async Task Test()
        {
            var exServ = new ExchangeService();
            var res = await exServ.GetUsdtPairs(new string[] { "PENDLE", "AKT" });
        }

    }

}
