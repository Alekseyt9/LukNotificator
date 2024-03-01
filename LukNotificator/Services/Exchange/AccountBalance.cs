using Newtonsoft.Json;

namespace LukNotificator.Services.Exchange
{
    public class AccountBalance
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("available")]
        public double Available { get; set; }

        [JsonProperty("holds")]
        public double Holds { get; set; }
    }
}
