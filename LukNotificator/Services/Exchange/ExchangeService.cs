using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LukNotificator.Services.Exchange
{
    internal class ExchangeService : IExchangeService
    {
        static readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _configuration;

        public ExchangeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
                var arr = ticker["symbol"].ToString().Split('-');
                if (arr[1] != "USDT")
                {
                    continue;
                }
                var s = arr[0];

                if (pairs.ContainsKey(s))
                {
                    var lastPrice = ticker["last"].ToObject<double>();
                    pairs[s] = lastPrice;
                }
            }

            return pairs;
        }

        public async Task<IEnumerable<ExCurInfo>> GetOwnCurrencies()
        {
            var apiKey = _configuration["kc_api_key"];
            var apiSecret = _configuration["kc_secret_key"];
            var apiPassphrase = _configuration["kc_passphrase"];
            var baseUrl = "https://api.kucoin.com";

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var method = "GET";
            var endpoint = "/api/v1/accounts";
            var body = "";
            var strToSign = timestamp + method + endpoint + body;
            var signature = SignWithHmacSHA256(strToSign, apiSecret);
            var passphrase = SignWithHmacSHA256(apiPassphrase, apiSecret);

            _client.DefaultRequestHeaders.Add("KC-API-SIGN", signature);
            _client.DefaultRequestHeaders.Add("KC-API-TIMESTAMP", timestamp);
            _client.DefaultRequestHeaders.Add("KC-API-KEY", apiKey);
            _client.DefaultRequestHeaders.Add("KC-API-PASSPHRASE", passphrase);
            _client.DefaultRequestHeaders.Add("KC-API-KEY-VERSION", "2");

            var response = await _client.GetAsync(baseUrl + endpoint);
            var content = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(content);

            var currencies = jObject["data"].Where(x => x["type"].ToString() == "trade").Select(x => new
            {
                Currency = x["currency"].ToString(),
                Balance = double.Parse(x["balance"].ToString(), CultureInfo.InvariantCulture),
                Available = double.Parse(x["available"].ToString(), CultureInfo.InvariantCulture),
                Holds = double.Parse(x["holds"].ToString(), CultureInfo.InvariantCulture)
            }).ToList();

            return currencies.Where(x => x.Available > 0.01)
                .Select(x => new ExCurInfo() { Code = x.Currency, Value = x.Available });
        }

        private static string SignWithHmacSHA256(string message, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyBytes);
            var hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
        }

        public Task Sell(string code, double value)
        {
            return Task.CompletedTask;
        }

    }
}
