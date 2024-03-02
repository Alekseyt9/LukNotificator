using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LukNotificator.Services.Exchange
{
    internal class ExchangeService(IConfiguration configuration) : IExchangeService
    {
        static readonly HttpClient _client = new();
        private const string baseUrl = "https://api.kucoin.com";

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
            var apiKey = configuration["kc_api_key"];
            var apiSecret = configuration["kc_secret_key"];
            var apiPassphrase = configuration["kc_passphrase"];

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

        public async Task<bool> Sell(string code, double value)
        {
            return await PlaceOrderAsync(code, "sell", value);
        }

        public async Task<bool> Buy(string code, double value)
        {
            return await PlaceOrderAsync(code, "buy", value);
        }

        private async Task<bool> PlaceOrderAsync(string code, string side, double value)
        {
            var endpoint = "/api/v1/orders";
            var method = "POST";

            var body = side == "sell"
                ? JsonConvert.SerializeObject(new
                {
                    clientOid = Guid.NewGuid().ToString(),
                    side = side,
                    symbol = $"{code.ToUpperInvariant()}-USDT",
                    type = "market",
                    size = value.ToString(CultureInfo.InvariantCulture)
                })
                : JsonConvert.SerializeObject(new
                {
                    clientOid = Guid.NewGuid().ToString(),
                    side = side,
                    symbol = $"{code.ToUpperInvariant()}-USDT",
                    type = "market",
                    funds = value.ToString(CultureInfo.InvariantCulture)
                });

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var signature = SignRequest(method, endpoint, body, timestamp);
            var apiKey = configuration["kc_api_key"];
            var apiPassphrase = configuration["kc_passphrase"];
            var apiSecret = configuration["kc_secret_key"];
            var passphrase = SignWithHmacSHA256(apiPassphrase, apiSecret);
            
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("KC-API-SIGN", signature);
            _client.DefaultRequestHeaders.Add("KC-API-TIMESTAMP", timestamp);
            _client.DefaultRequestHeaders.Add("KC-API-KEY", apiKey);
            _client.DefaultRequestHeaders.Add("KC-API-PASSPHRASE", passphrase);
            _client.DefaultRequestHeaders.Add("KC-API-KEY-VERSION", "2");

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(baseUrl + endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<JObject>(responseContent);
                throw new Exception($"Error placing order: {error["msg"]}");
            }

            var responseData = JsonConvert.DeserializeObject<JObject>(responseContent);
            if (responseData["code"].ToString() == "200000")
            {
                return true;
            }

            throw new Exception($"Error placing order: {responseData["msg"]}");
        }

        private string SignRequest(string method, string endpoint, string body, string timestamp)
        {
            var apiSecret = configuration["kc_secret_key"];
            var bodyString = body ?? "";
            var strToSign = timestamp + method.ToUpper() + endpoint + bodyString;
            var secretBytes = Encoding.UTF8.GetBytes(apiSecret);
            var signBytes = Encoding.UTF8.GetBytes(strToSign);
            using var hmac = new HMACSHA256(secretBytes);
            var hash = hmac.ComputeHash(signBytes);

            return Convert.ToBase64String(hash);
        }

    }
}
