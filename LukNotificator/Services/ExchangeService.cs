

using Newtonsoft.Json.Linq;

namespace LukNotificator.Services
{
    internal class ExchangeService : IExchangeService
    {
        static readonly HttpClient _client = new HttpClient();

        public async Task<IDictionary<string, double>> GetUsdtPairs(string[] codes)
        {
            var response = await _client.GetAsync("https://api.kucoin.com/api/v1/market/allTickers");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var pairs = codes.ToDictionary(x => x, y => 0d);
            var json = JObject.Parse(responseBody);
            var tickers = json["data"]["ticker"].Children();

            foreach (var ticker in tickers)
            {
                var s = ticker["symbol"].ToString().Split('-')[0];
                if (pairs.ContainsKey(s))
                {
                    var lastPrice = ticker["last"].ToObject<double>();
                    pairs[s] = lastPrice;
                }
            }

            return pairs;
        }



    }
}
