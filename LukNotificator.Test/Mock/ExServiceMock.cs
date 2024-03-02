
using LukNotificator.Services.Exchange;

namespace LukNotificator.Test.Mock
{
    internal class ExServiceMock : IExchangeService
    {
        public Task<IDictionary<string, double>> GetUsdtPairs(string[] codes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExCurInfo>> GetOwnCurrencies()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Sell(string code, double value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Buy(string code, double value)
        {
            throw new NotImplementedException();
        }
    }
}
