
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace LukNotificator.Services.Exchange.Impl
{
    internal class BybitExchangeService(IConfiguration configuration) : IExchangeService
    {
        static readonly HttpClient _client = new();
        private const string baseUrl = "https://api.kucoin.com";

        public async Task<IDictionary<string, double>> GetUsdtPairs(string[] codes)
        {
            var response = await _client.GetAsync("https://api.bybit.com/v2/public/tickers");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var pairs = codes.ToDictionary(x => x, y => 0d);
            var json = JObject.Parse(responseBody);
            var tickers = json["result"].Children(); 

            foreach (var ticker in tickers)
            {
                var symbol = ticker["symbol"].ToString();
                if (!symbol.EndsWith("USDT"))
                {
                    continue;
                }

                var s = symbol.Substring(0, symbol.Length - 4);
                if (pairs.ContainsKey(s))
                {
                    var lastPrice = ticker["last_price"].ToObject<double>();
                    pairs[s] = lastPrice;
                }
            }

            return pairs;
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
