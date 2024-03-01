using Newtonsoft.Json;

namespace LukNotificator.Services.Exchange
{
    public class ApiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public List<AccountBalance> Data { get; set; }
    }
}
